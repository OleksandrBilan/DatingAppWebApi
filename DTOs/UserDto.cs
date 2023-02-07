﻿using DatingApp.Models;

namespace DatingApp.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public Sex SexPreferences { get; set; }

        public string Description { get; set; }

        public City City { get; set; }
    }
}
