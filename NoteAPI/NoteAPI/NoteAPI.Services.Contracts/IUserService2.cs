using System.Threading.Tasks;

using NoteAPI.Services.Model;
using NoteAPI.Services.Model.Requests;
using NoteAPI.Services.Model.Responses;

namespace NoteAPI.Services.Contracts
{
    public interface IUserService2
    {
        Task<Response<Request<UserCreation>, User>> CreateAsync(Request<UserCreation> request);

        Task<Response<Request<User>, User>> UpdateAsync(Request<User> request);

        Task<Response<Request<string>, int>> DeleteAsync(Request<string> request);

        Task<User> GetAsync(string id);
    }
}
