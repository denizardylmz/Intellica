using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using NoteAPI.API.DataContracts.Responses;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models;
using NoteAPI.Services.Services;

namespace NoteAPI.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notes")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<NoteController> _logger;
        
        public NoteController(INoteService service, IMapper mapper, ILogger<NoteController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("list")]
        public async Task<ActionResult<Response<List<Note>>>> GetAll()
        {
            var response = new Response<List<Note>>(_service.GetAllNotes());
            await response.ExecuteTask();

            if (!response.IsSuccessfull)
                return BadRequest(response);
            if (response.ResponseContent == null)
                return NoContent();    
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<int, Note>>> Get(int id)
        {
            var response = new Response<int, Note>(request:id, task:_service.GetNote(id));
            await response.ExecuteTask();
            
            if (!response.IsSuccessfull)
                return BadRequest(response);
            if (response.ResponseContent == null)
                return NoContent();    
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("")]
        public async Task<ActionResult<Response<Note,Note>>> Create(Note note)
        {
            var response = new Response<Note,Note>(request:note, task:_service.CreateNote(note));
            await response.ExecuteTask();
            
            if (!response.IsSuccessfull)
                return BadRequest(response);
            if (response.ResponseContent == null)
                return NoContent();    
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("")]
        public async Task<ActionResult<Response<Note,Note>>> Update(Note note)
        {
            var response = new Response<Note,Note>(request:note, task:_service.UpdateNote(note));
            await response.ExecuteTask();
            
            if (!response.IsSuccessfull)
                return BadRequest(response);
            if (response.ResponseContent == null)
                return NoContent();    
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<int, bool>>> Delete(int id)
        {
            var response = new Response<int, bool>(request: id, task: _service.DeleteNote(id));
            await response.ExecuteTask();
            
            if (!response.IsSuccessfull)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
