using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Services.Models;
using SM = NoteAPI.Services.Models;

namespace NoteAPI.Services.Contracts
{
    public interface INoteService 
    {
        Task<List<Note>> GetAllNotes();
        Task<SM.Note> CreateNote(SM.Note note);
        Task<SM.Note> UpdateNote(SM.Note note);
        Task<bool> DeleteNote(int id);
        Task<SM.Note> GetNote(int id);
    }
}
