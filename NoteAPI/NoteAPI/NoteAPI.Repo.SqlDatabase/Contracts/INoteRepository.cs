using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Repo.SqlDatabase.Contracts
{
    public interface INoteRepository
    {
        Task<Note> GetAsync(string id);

        Task<Note> CreateAsync(Note project);

        Task<Note> UpdateAsync(Note project);

        Task<Note> UpsertAsync(Note project);

        Task<int> DeleteAsync(string projectId);

    }
}
