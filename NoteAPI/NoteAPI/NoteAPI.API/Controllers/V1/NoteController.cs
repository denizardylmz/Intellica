using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models;
using NoteAPI.Services.Services;

namespace NoteAPI.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/notes")]//required for default versioning
    [Route("api/v{version:apiVersion}/notes")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<NoteController> _logger;

#pragma warning disable CS1591
        public NoteController(INoteService service, IMapper mapper, ILogger<NoteController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
#pragma warning restore CS1591


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Note))]
        [HttpGet("fetchAll")]
        public async Task<List<Note>> GetAll()
        {
            var data = await _service.GetAllNotes();
            return data;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Note))]
        [HttpGet("{id}")]
        public async Task<Note> Get(int id)
        {
            var data = await _service.GetNote(id);
            return data;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("")]
        public async Task<Note> Create(Note note)
        {
            var data = await _service.CreateNote(note);

            return data;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("")]
        public async Task<Note> Update(Note note)
        {
            var data = await _service.UpdateNote(note);

            return data;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var data = await _service.DeleteNote(id);

            return data;
        }
    }
}
