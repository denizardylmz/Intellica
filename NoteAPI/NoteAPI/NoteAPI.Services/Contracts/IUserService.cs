using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = NoteAPI.Services.Models;
using SqlM = NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Services.Contracts
{
    public interface IUserService
    {
        Task<JwtSecurityToken> Login(string username, string password);
        Task<SM.User> CreateUser(SM.User user);
    }
}
