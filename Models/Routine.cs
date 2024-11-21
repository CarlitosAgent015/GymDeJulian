using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gym.noPainNoGain.Models
{
    public class Routine
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Routine name is required.")]
        [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string NameRoutine { get; set; }

        [Required(ErrorMessage = "Day is required.")]
        [StringLength(20, ErrorMessage = "The day cannot be longer than 20 characters.")]
        public string Day { get; set; }

        [Required(ErrorMessage = "Frequency is required.")]
        [Range(1, 7, ErrorMessage = "Frequency must be between 1 and 7.")]
        public int Frequency { get; set; }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        public Guid UserId { get; set; }
    }

    // Clase para crear o actualizar rutinas
    public class CreateUpdateRoutine
    {
        [Required(ErrorMessage = "Routine name is required.")]
        [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string NameRoutine { get; set; }

        [Required(ErrorMessage = "Day is required.")]
        [StringLength(20, ErrorMessage = "The day cannot be longer than 20 characters.")]
        public string Day { get; set; }

        [Required(ErrorMessage = "Frequency is required.")]
        [Range(1, 7, ErrorMessage = "Frequency must be between 1 and 7.")]
        public int Frequency { get; set; }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; } // ID del usuario al que pertenece la rutina
    }
}
