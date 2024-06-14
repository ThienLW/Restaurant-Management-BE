using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HokkaidoBackend.Models;
using Microsoft.EntityFrameworkCore;
using HokkaidoBackend.Data;

namespace HokkaidoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private static List<User> Users = new List<User>();


        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, ApplicationDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        //public UserController(ILogger<UserController> logger)
        //{
        //    _logger = logger;

        //    // Initializing with sample data for demonstration
        //    if (!Users.Any())
        //    {
        //        Users.Add(new User { Id = 1, Name = "User1", Password = "password123", PhoneNumber = "123-456-7891", Email = "user1@example.com", Address = "Address 1", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
        //        Users.Add(new User { Id = 2, Name = "User2", Password = "password123", PhoneNumber = "123-456-7892", Email = "user2@example.com", Address = "Address 2", CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
        //    }
        //}

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            return Users;
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult Post([FromBody] User newUser)
        {
            newUser.Id = Users.Count > 0 ? Users.Max(u => u.Id) + 1 : 1;
            newUser.CreateDate = DateTime.UtcNow;
            newUser.UpdateDate = DateTime.UtcNow;
            Users.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult Put(int id, [FromBody] User updatedUser)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Name = updatedUser.Name;
            existingUser.Password = updatedUser.Password;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.Email = updatedUser.Email;
            existingUser.Address = updatedUser.Address;
            existingUser.UpdateDate = DateTime.UtcNow;

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public IActionResult Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            Users.Remove(user);
            return NoContent();
        }

        // API GET USERS LIST
        [HttpGet("userlist")]
        public IActionResult GetUsers(int page = 1, int pageSize = 10)
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.PhoneNumber,
                    u.Email,
                    u.Address,
                    u.CreateDate,
                    u.UpdateDate
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalUsers = _context.Users.Count();

            return Ok(new
            {
                status = 200,
                message = "success",
                data = users,
                totalUsers = totalUsers,
                page = page,
                pageSize = pageSize
            });
        }

        // API SEARCH USER

        [HttpGet("searchusers")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] string? phone,
            [FromQuery] string? address,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                usersQuery = usersQuery.Where(u => u.PhoneNumber.Contains(phone));
            }

            if (!string.IsNullOrEmpty(address))
            {
                usersQuery = usersQuery.Where(u => u.Address.Contains(address));
            }

            var totalUsers = await usersQuery.CountAsync();
            var users = await usersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.PhoneNumber,
                    u.Email,
                    u.Address,
                    u.CreateDate,
                    u.UpdateDate
                })
                .ToListAsync();

            return Ok(new
            {
                totalUsers = totalUsers,
                users = users
            });
        }


    }
}
