﻿using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class CountryDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}