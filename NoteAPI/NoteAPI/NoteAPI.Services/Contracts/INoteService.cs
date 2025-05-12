using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Services.Contracts
{
    public interface INoteService 
    {
        Task<List<Note>> GetAllNotes();
    }
}
