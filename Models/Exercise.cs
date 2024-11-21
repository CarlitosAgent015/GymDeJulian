using System;
using System.ComponentModel.DataAnnotations;

namespace Gym.noPainNoGain
{
    public class Exercise
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Exercise name is required.")]
        public string NameExercise { get; set; }

        [Required(ErrorMessage = "Series are required.")]
        public string Series { get; set; }

        [Required(ErrorMessage = "Repetitions are required.")]
        public string Repetitions { get; set; }
    }

    public class CreateUpdateExercise
    {
        [Required(ErrorMessage = "Exercise name is required.")]
        public string NameExercise { get; set; }

        [Required(ErrorMessage = "Series are required.")]
        public string Series { get; set; }

        [Required(ErrorMessage = "Repetitions are required.")]
        public string Repetitions { get; set; }
    }
}
