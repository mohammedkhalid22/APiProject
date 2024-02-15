﻿using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Model
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Tittle { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storeline { get; set; }
        public byte[] Poster { get; set; }
        public byte genreId { get; set; }
        public Genre Genre  { get; set; }
    }
}
