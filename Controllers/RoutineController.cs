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
    public class RoutineController : ControllerBase
    {
        private readonly GymDbContext _context;

        public RoutineController(GymDbContext context)
        {
            _context = context;
        }

        // GET: api/Routine
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var routines = await _context.Routines.ToListAsync();
            if (routines.Any())
            {
                return Ok(new Response<IEnumerable<Routine>>
                {
                    IsSuccess = true,
                    Result = routines,
                    Message = "Listed routines successfully"
                });
            }
            return Ok(new Response<IEnumerable<Routine>>
            {
                IsSuccess = false,
                Message = "No routines found",
                Result = Enumerable.Empty<Routine>()
            });
        }

        // POST: api/Routine
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUpdateRoutine model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<CreateUpdateRoutine>
                {
                    IsSuccess = false,
                    Result = model,
                    Message = "The fields are not valid"
                });
            }

            var newRoutine = new Routine
            {
                NameRoutine = model.NameRoutine,
                Day = model.Day,
                Frequency = model.Frequency,
                Exercises = model.Exercises,
                UserId = model.UserId
            };

            await _context.Routines.AddAsync(newRoutine);
            await _context.SaveChangesAsync();

            return Ok(new Response<Routine>
            {
                IsSuccess = true,
                Result = newRoutine,
                Message = "Routine created successfully"
            });
        }

        // GET: api/Routine/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<Routine>
                {
                    IsSuccess = false,
                    Message = "The ID is required",
                    Result = null
                });
            }

            var routine = await _context.Routines.FindAsync(id);
            if (routine == null)
            {
                return NotFound(new Response<Routine>
                {
                    IsSuccess = false,
                    Message = "Routine not found",
                    Result = null
                });
            }

            return Ok(new Response<Routine>
            {
                IsSuccess = true,
                Result = routine,
                Message = "Routine found"
            });
        }

        // DELETE: api/Routine/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<Routine>
                {
                    IsSuccess = false,
                    Message = "The ID is not valid",
                    Result = null
                });
            }

            var routine = await GetRoutineById(id);
            if (routine != null)
            {
                _context.Routines.Remove(routine);
                await _context.SaveChangesAsync();

                return Ok(new Response<Routine>
                {
                    IsSuccess = true,
                    Message = $"The routine was deleted: {routine.NameRoutine}",
                    Result = routine
                });
            }

            return NotFound(new Response<Routine>
            {
                IsSuccess = false,
                Message = "Routine not found",
                Result = null
            });
        }

        // PUT: api/Routine/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreateUpdateRoutine model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<CreateUpdateRoutine>
                {
                    IsSuccess = false,
                    Message = "The ID is required",
                    Result = null
                });
            }

            if (ModelState.IsValid)
            {
                var routine = await GetRoutineById(id);
                if (routine != null)
                {
                    routine.NameRoutine = model.NameRoutine;
                    routine.Day = model.Day;
                    routine.Frequency = model.Frequency;
                    routine.Exercises = model.Exercises;

                    _context.Routines.Update(routine);
                    await _context.SaveChangesAsync();

                    return Ok(new Response<CreateUpdateRoutine>
                    {
                        IsSuccess = true,
                        Message = "Routine updated successfully",
                        Result = model
                    });
                }
            }

            return BadRequest(new Response<CreateUpdateRoutine>
            {
                IsSuccess = false,
                Message = "Could not update the routine",
                Result = null
            });
        }

        // Método auxiliar para obtener la rutina por ID
        private async Task<Routine> GetRoutineById(Guid id)
        {
            return await _context.Routines.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
