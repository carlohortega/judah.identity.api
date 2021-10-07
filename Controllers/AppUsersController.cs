using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Eis.Identity.Api.AsyncDataServices;
using Eis.Identity.Api.Data;
using Eis.Identity.Api.Dtos;
using Eis.Identity.Api.Models;
using Eis.Identity.Api.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eis.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUserRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public AppUsersController(
            IAppUserRepo repo, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppUserReadDto>> GetAppUsers()
        {
            Console.WriteLine("--> Getting app users...");

            var users = _repo.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<AppUserReadDto>>(users));
        }

        [HttpGet("{id}", Name = "GetAppUserById")]
        public ActionResult<AppUserReadDto> GetAppUserById(int id)
        {
            Console.WriteLine("--> Getting one user...");

            var user = _repo.GetUserById(id);

            if (user != null)
            {
                return Ok(_mapper.Map<AppUserReadDto>(user));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<AppUserReadDto>> CreateAppUser(AppUserCreateDto dto)
        {
            var appUserModel = _mapper.Map<AppUser>(dto);
            _repo.CreateUser(appUserModel);
            _repo.SaveChanges();
            
            var appUserReadDto = _mapper.Map<AppUserReadDto>(appUserModel);

            // Send sync message
            try 
            {
                await _commandDataClient.SendIdentityToCommand(appUserReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send async message
            try 
            {
                var appUserDto = _mapper.Map<AppUserPublishedDto>(appUserReadDto);
                appUserDto.Event = "AppUser_Published";
                _messageBusClient.PublishNewAppUser(appUserDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetAppUserById), new { id = appUserReadDto.Id }, appUserReadDto);
        }
    }
}