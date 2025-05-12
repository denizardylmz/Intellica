using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Repo.SqlDatabase.Contracts;
using NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Repo.SqlDatabase.Repositories
{
    public class NoteRepository : INoteRepository
    {
        public Task<Note> CreateAsync(Note project)
        {




            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Note> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Note> UpdateAsync(Note project)
        {
            throw new NotImplementedException();
        }

        public Task<Note> UpsertAsync(Note project)
        {
            throw new NotImplementedException();
        }
    }
}
