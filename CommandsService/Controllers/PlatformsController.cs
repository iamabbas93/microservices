using System;
using System.Collections.Generic;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
            
        }


        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatform(){
            Console.WriteLine("Getting Platofrm FromCommand Services");
            var platformItems=_repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
            
        }


        [HttpPost]
        public ActionResult testInboundconnection(){
            Console.WriteLine("Inbound Post Command Service");
            return Ok("Test Okay");
        }


        
        

    }
}