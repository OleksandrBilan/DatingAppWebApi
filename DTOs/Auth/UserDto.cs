﻿using DatingApp.DTOs.Lookup;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Auth
{
    public class UserDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int SexId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public CountryDto Country { get; set; }

        [Required]
        public CityDto City { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
