using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gym.noPainNoGain.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The field Email Address is required.")]
        [EmailAddress(ErrorMessage = "It must be a valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field password is required.")]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters.")]
        public string Password { get; set; }

        public List<Routine> Routines { get; set; } = new List<Routine>();
    }

    public class CreateUpdate
    {
        [Required(ErrorMessage = "The field {0} is required.")]
        [MinLength(3, ErrorMessage = "The field {0} must have at least {1} characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The field Email Address is required.")]
        [EmailAddress(ErrorMessage = "It must be a valid email.")]
        [MinLength(10, ErrorMessage = "The field {0} must have at least {1} characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field password is required.")]
        [MinLength(10, ErrorMessage = "The field {0} must have at least {1} characters.")]
        public string Password { get; set; }

        // Relación con rutinas
        public List<Routine> Routines { get; set; } = new List<Routine>();
    }
}
