using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NoteAPI.Repo.SqlDatabase.Context;
using NoteAPI.Repo.SqlDatabase.Contracts;
using NoteAPI.Repo.SqlDatabase.DTO;
using NoteAPI.Tools.Logs;

namespace NoteAPI.Repo.SqlDatabase.Repositories
{
    public class UserRepository : IUserRepository
    {
        NoteAPISqlDbContext _sourceXSqlDBContext;
        private ILogger<UserRepository> _logger;

        public UserRepository(
            NoteAPISqlDbContext sourceXSqlDBContext,
            ILogger<UserRepository> logger
            )
        {
            _sourceXSqlDBContext = sourceXSqlDBContext;
            _logger = logger;
        }

        public async Task<int> DeleteAsync(string id)
        {
            int result = 0;

            User? existingProject = _sourceXSqlDBContext.Users.SingleOrDefault(p => p.Id == id);

            if (existingProject != default(User))
            {
                _sourceXSqlDBContext.Users.Remove(existingProject);
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "DeleteAsync")}::ProjectId: {id}:: Found. Deleted.");

                result = await _sourceXSqlDBContext.SaveChangesAsync();
            }
            else
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "DeleteAsync")}::ProjectId: {id}:: Not found.");

            return result;
        }

        public async Task<User> CreateAsync(User item)
        {
            User? existingProject = await _sourceXSqlDBContext.FindAsync<User>(item.Id);

            if (existingProject == default(User))
            {
                //Create
                _sourceXSqlDBContext.Users.Add(item);
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "CreateAsync")}::Id: {item.Id}:: Not found. Created.");

                int result = await _sourceXSqlDBContext.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "CreateAsync")}::Id: {item.Id}:: Saved.");
                    return item;
                }
                else
                {
                    _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "CreateAsync")}::Id: {item.Id}:: Not saved.");
                    return default(User);
                }
            }
            else
            {
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "CreateAsync")}::Id: {item.Id}:: Already existing project.");
                throw new Exception($"User with id {item.Id} already exists.");
            }
        }

        public async Task<User> GetAsync(string id)
        {
            User? existingItem = _sourceXSqlDBContext.Users.AsNoTracking().SingleOrDefault(p => p.Id == id);

            if (existingItem != default(User))
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "GetAsync")}::Id: {id}:: Found.");
            else
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "GetAsync")}::Id: {id}:: Not found.");

            return existingItem;
        }

        public async Task<User> UpdateAsync(User item)
        {
            User? existingItem = await _sourceXSqlDBContext.FindAsync<User>(item.Id);

            if (existingItem == null)
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpdateAsync")}::Id: {item.Id}:: Not found. The item cannot be updated.");
            else
            {
                //Update
                _sourceXSqlDBContext.Entry(existingItem).State = EntityState.Detached;

                _sourceXSqlDBContext.Users.Attach(item);
                _sourceXSqlDBContext.Entry(item).State = EntityState.Modified;

                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpdateAsync")}::Id: {item.Id}:: Found. Updated.");
            }

            int result = await _sourceXSqlDBContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpdateAsync")}::Id: {item.Id}:: Saved.");
                return item;
            }
            else
            {
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpdateAsync")}::Id: {item.Id}:: Not saved.");
                return default(User);
            }
        }

        public async Task<User> UpsertAsync(User item)
        {
            User? existingItem = await _sourceXSqlDBContext.FindAsync<User>(item.Id);

            if (existingItem == default(User))
            {
                //Create
                _sourceXSqlDBContext.Users.Add(item);
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpsertAsync")}::Id: {item.Id}:: Not found. Created.");
            }
            else
            {
                //Update
                _sourceXSqlDBContext.Entry(existingItem).State = EntityState.Detached;

                _sourceXSqlDBContext.Users.Attach(item);
                _sourceXSqlDBContext.Entry(item).State = EntityState.Modified;

                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpsertAsync")}::Id: {item.Id}:: Found. Updated.");
            }

            int result = await _sourceXSqlDBContext.SaveChangesAsync();

            if (result > 0)
            {
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpsertAsync")}::Id: {item.Id}:: Saved.");
                return item;
            }
            else
            {
                _logger.LogDebug($"{LogsHelper.BuildLogPrefix("UserRepository", "UpsertAsync")}::Id: {item.Id}:: Not saved.");
                return default(User);
            }
        }
    }
}
