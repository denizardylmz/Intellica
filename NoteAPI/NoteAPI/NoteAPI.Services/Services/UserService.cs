using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NoteAPI.API.Common.Settings;
using NoteAPI.Repo.SqlDatabase.Context;
using NoteAPI.Services.Contracts;
using SM = NoteAPI.Services.Models;
using SqlM = NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Services.Services
{

    public class UserService : IUserService
    {
        private AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly NoteAPISqlDbContext _context;
        private readonly IValidator<SM.User> _userValidator;

        public UserService(
            IMapper mapper, 
            NoteAPISqlDbContext context, 
            IValidator<SM.User> validator,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _context = context;
            _userValidator = validator;
            var section = configuration.GetSection(nameof(AppSettings));
            _appSettings = section.Get<AppSettings>();
        }

        public async Task<JwtSecurityToken> Login(string username, string password)
        {
            if (_appSettings?.API?.JwtSettings == null)
                throw new Exception("No Jwt settings found.");

            //Neeed Improvement
            if (username == _appSettings.API.Admin.Username && password == _appSettings.API.Admin.Password) 
                return LoginAsAdmin();

            var user = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var dbHashSalt = user.PasswordSalt;
            var dbHashPassword = user.PasswordHash;

            using var recievedPasswordHash = new Rfc2898DeriveBytes(password, dbHashSalt, 100_000, HashAlgorithmName.SHA256);
            byte[] computedrecievedPasswordHash = recievedPasswordHash.GetBytes(32);

            if (!computedrecievedPasswordHash.SequenceEqual(dbHashPassword))
                throw new Exception("Şifre yanlış.");

            var claims = new[]
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub , username),
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.API.JwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _appSettings.API.JwtSettings.Issuer,
                audience: _appSettings.API.JwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds
            );

            return token;
        }
        private JwtSecurityToken LoginAsAdmin()
        {
            var claims = new[]
            {
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, _appSettings.API.Admin.Username),
                    new Claim(ClaimTypes.NameIdentifier, "Admin"),
                    new Claim(ClaimTypes.Role, "Admin"),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.API.JwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _appSettings.API.JwtSettings.Issuer,
                audience: _appSettings.API.JwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return token;
        }

        public async Task<SM.User> CreateUser(SM.User user)
        {
            _userValidator.ValidateAndThrow(user);

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            var SqlUser = _mapper.Map<SqlM.User>(user);

            SqlUser.PasswordHash = hash;
            SqlUser.PasswordSalt = salt;

            await _context.Users.AddAsync(SqlUser);
            await _context.SaveChangesAsync();

            return _mapper.Map<SM.User>(SqlUser);
        }
    }
}
