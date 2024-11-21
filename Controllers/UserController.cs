using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Gym.noPainNoGain.Data;
using Gym.noPainNoGain.Models;
using Gym.noPainNoGain.Responses;

namespace Gym.noPainNoGain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GymDbContext _context;

        public UserController(GymDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Any())
            {
                return Ok(new Response<IEnumerable<User>>
                {
                    IsSuccess = true,
                    Result = users,
                    Message = "List of users"
                });
            }
            return NotFound(new Response<IEnumerable<User>>
            {
                IsSuccess = false,
                Message = "No results to show",
                Result = Enumerable.Empty<User>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<CreateUpdate>
                {
                    IsSuccess = false,
                    Result = model,
                    Message = "The fields are not correct"
                });
            }

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new Response<User>
            {
                IsSuccess = true,
                Result = newUser,
                Message = "User created successfully"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<User>
                {
                    IsSuccess = false,
                    Message = "The ID is necessary",
                    Result = null
                });
            }

            var user = await GetUser(id);
            if (user == null)
            {
                return NotFound(new Response<User>
                {
                    IsSuccess = false,
                    Message = "There are no matches",
                    Result = null
                });
            }

            return Ok(new Response<User>
            {
                IsSuccess = true,
                Result = user,
                Message = "User found"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<User>
                {
                    IsSuccess = false,
                    Message = "The ID is not correct",
                    Result = null
                });
            }

            var user = await GetUser(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new Response<User>
                {
                    Result = user,
                    IsSuccess = true,
                    Message = $"The user was deleted: {user.Username}"
                });
            }

            return NotFound(new Response<User>
            {
                IsSuccess = false,
                Message = "There are no matches",
                Result = null
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreateUpdate model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<CreateUpdate>
                {
                    IsSuccess = false,
                    Message = "The ID is necessary",
                    Result = null
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<CreateUpdate>
                {
                    IsSuccess = false,
                    Message = "The fields are not correct",
                    Result = model
                });
            }

            var user = await GetUser(id);
            if (user != null)
            {
                user.Username = model.Username;
                user.Email = model.Email;
                user.Password = model.Password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new Response<CreateUpdate>
                {
                    IsSuccess = true,
                    Message = "User updated",
                    Result = model
                });
            }

            return NotFound(new Response<CreateUpdate>
            {
                IsSuccess = false,
                Message = "Could not update the record, user not found",
                Result = null
            });
        }

        // Método privado para obtener un usuario por ID
        private async Task<User> GetUser(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
