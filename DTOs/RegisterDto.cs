﻿using DatingApp.DB.Models;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public Sex Sex { get; set; }

        [Required]
        public IEnumerable<Sex> SexPreferences { get; set; }

        [Required]
        public string Description { get; set; }
    }
}