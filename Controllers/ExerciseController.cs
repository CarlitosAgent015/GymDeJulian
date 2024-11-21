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
    public class ExerciseController : ControllerBase
    {
        private readonly GymDbContext _context;

        public ExerciseController(GymDbContext context)
        {
            _context = context;
        }

        // GET: api/Exercise
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _context.Exercises.ToListAsync();
            if (exercises.Any())
            {
                return Ok(new Response<IEnumerable<Exercise>>
                {
                    IsSuccess = true,
                    Result = exercises,
                    Message = "Listed exercises successfully"
                });
            }
            return Ok(new Response<IEnumerable<Exercise>>
            {
                IsSuccess = false,
                Message = "No exercises found",
                Result = Enumerable.Empty<Exercise>()
            });
        }

        // POST: api/Exercise
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUpdateExercise model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<CreateUpdateExercise>
                {
                    IsSuccess = false,
                    Result = model,
                    Message = "The fields are not valid"
                });
            }

            var newExercise = new Exercise
            {
                NameExercise = model.NameExercise,
                Series = model.Series,
                Repetitions = model.Repetitions
            };

            await _context.Exercises.AddAsync(newExercise);
            await _context.SaveChangesAsync();

            return Ok(new Response<Exercise>
            {
                IsSuccess = true,
                Result = newExercise,
                Message = "Exercise created successfully"
            });
        }

        // GET: api/Exercise/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<Exercise>
                {
                    IsSuccess = false,
                    Message = "The ID is required",
                    Result = null
                });
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound(new Response<Exercise>
                {
                    IsSuccess = false,
                    Message = "Exercise not found",
                    Result = null
                });
            }

            return Ok(new Response<Exercise>
            {
                IsSuccess = true,
                Result = exercise,
                Message = "Exercise found"
            });
        }

        // DELETE: api/Exercise/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<Exercise>
                {
                    IsSuccess = false,
                    Message = "The ID is not valid",
                    Result = null
                });
            }

            var exercise = await GetExerciseById(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();

                return Ok(new Response<Exercise>
                {
                    IsSuccess = true,
                    Message = $"The exercise was deleted: {exercise.NameExercise}",
                    Result = exercise
                });
            }

            return NotFound(new Response<Exercise>
            {
                IsSuccess = false,
                Message = "Exercise not found",
                Result = null
            });
        }

        // PUT: api/Exercise/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreateUpdateExercise model)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Response<CreateUpdateExercise>
                {
                    IsSuccess = false,
                    Message = "The ID is required",
                    Result = null
                });
            }

            if (ModelState.IsValid)
            {
                var exercise = await GetExerciseById(id);
                if (exercise != null)
                {
                    exercise.NameExercise = model.NameExercise;
                    exercise.Series = model.Series;
                    exercise.Repetitions = model.Repetitions;

                    _context.Exercises.Update(exercise);
                    await _context.SaveChangesAsync();

                    return Ok(new Response<CreateUpdateExercise>
                    {
                        IsSuccess = true,
                        Message = "Exercise updated successfully",
                        Result = model
                    });
                }
            }

            return BadRequest(new Response<CreateUpdateExercise>
            {
                IsSuccess = false,
                Message = "Could not update the exercise",
                Result = null
            });
        }

        // Método auxiliar para obtener el ejercicio por ID
        private async Task<Exercise> GetExerciseById(Guid id)
        {
            return await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
