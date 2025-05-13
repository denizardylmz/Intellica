using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteAPI.API.Common.Settings;
using NoteAPI.Repo.SqlDatabase.Context;
using SqlM = NoteAPI.Repo.SqlDatabase.DTO;
using NoteAPI.Services.Contracts;
using SM = NoteAPI.Services.Models;

namespace NoteAPI.Services.Services
{
    public class NoteService : INoteService
    {
        private AppSettings _settings;
        private readonly IMapper _mapper;
        private readonly NoteAPISqlDbContext _context;
        private readonly IValidator<SM.Note> _noteValidator;


        public NoteService(
            IOptions<AppSettings> settings, 
            IMapper mapper, 
            NoteAPISqlDbContext context,
            IValidator<SM.Note> noteValidator
            )
        {
            _settings = settings?.Value;
            _mapper = mapper;
            _context = context;
            _noteValidator = noteValidator;
        }


        public async Task<List<SM.Note>> GetAllNotes()
        {
            var notes = await _context.Notes.ToListAsync();
            return _mapper.Map<List<SM.Note>>(notes);
        }

        public async Task<SM.Note> CreateNote(SM.Note note)
        {
            var validationResult = await _noteValidator.ValidateAsync(note);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToString();
                throw new Exception("Validation Error : " + errors);
            }

            var mappedNote = _mapper.Map<SqlM.Note>(note);
            await _context.Notes.AddAsync(mappedNote);
            await _context.SaveChangesAsync();

            return _mapper.Map<SM.Note>(mappedNote);
        }

        public async Task<SM.Note> UpdateNote(SM.Note note)
        {
            var validationResult = await _noteValidator.ValidateAsync(note);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                throw new Exception("Validation Error : " + errors);
            }

            var mappedNote = _mapper.Map<SqlM.Note>(note);
            _context.Notes.Update(mappedNote);
            await _context.SaveChangesAsync();

            return _mapper.Map<SM.Note>(mappedNote);
        }

        public async Task<bool> DeleteNote(int id)
        {
            var res = await _context.Notes.Where(x => x.NoteId == id).ExecuteDeleteAsync();
            return res == 1;
        }

        public async Task<SM.Note> GetNote(int id)
        {
            var note = await _context.Notes.Where(x => x.NoteId == id).FirstOrDefaultAsync();
            var mappedNote = _mapper.Map<SM.Note>(note);

            return mappedNote;
        }
    }
}
