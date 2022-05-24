using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController:ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repo,IMapper mapper,ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient  )
        {
            _repo=repo;
            _mapper=mapper;
            _commandDataClient=commandDataClient;
            _messageBusClient=messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> getPlatforms()
        {
            Console.WriteLine("Getting Platforms....");
            var data=_repo.getAllPlatform();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(data));
        }


        [HttpGet("{id}",Name="getPlatformById")]
        public ActionResult<PlatformReadDto> getPlatformById(int id)
        {
            Console.WriteLine("Getting Platforms by Id...."); 
            var platformItem=_repo.GetPlatformById(id);
           
            if(platformItem!=null){
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            
            return NotFound();


        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> createPlaform(PlatformCreateDto platformCreateDto)
        {
              Console.WriteLine("Creating Platforms...."); 
            var plaformModel=_mapper.Map<Platform>(platformCreateDto);
            _repo.CreatePlatform(plaformModel);
            _repo.DataSaveChanges();

            var plaformReadDto=_mapper.Map<PlatformReadDto>(plaformModel);


            //Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(plaformReadDto);
            }
            catch (System.Exception)
            {
                
               Console.WriteLine("Could Not Send Synchrounouly");
            }


            //Send Async Message
            try
            {
               var platforPublisDto=_mapper.Map<PlatformPublishedDto>(plaformReadDto);
               platforPublisDto.Event="Platform_Published"; 
               _messageBusClient.PublishNewPlatform(platforPublisDto);
               

            }
            catch (System.Exception ex)
            {
                
               Console.WriteLine($"Could Not send AsyncL{ex.Message}");
            }
            return CreatedAtRoute(nameof(getPlatformById), new {Id=plaformReadDto.Id} ,plaformReadDto);

        }







    }
}