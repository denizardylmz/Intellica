using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteAPI.API.Common.Settings;
using NoteAPI.Repo.SqlDatabase.Context;
using NoteAPI.Repo.SqlDatabase.DTO;
using NoteAPI.Services.Contracts;

namespace NoteAPI.Services.Services
{
    public class NoteService : INoteService
    {
        private AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly NoteAPISqlDbContext _context;

        public NoteService(IOptions<AppSettings> settings, IMapper mapper, NoteAPISqlDbContext context)
        {
            _settings = settings?.Value;
            _mapper = mapper;
            _context = context;
        }


        public async Task<List<Note>> GetAllNotes()
        {
            var notes = await _context.Notes.ToListAsync();

            return notes;
        }


    }
}
