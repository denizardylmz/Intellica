using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<List<Note>> Get()
        {
            var data = await _service.GetAllNotes();

            if (data != null)
                return _mapper.Map<List<Note>>(data);
            else
                return null;
        }
    }
}
