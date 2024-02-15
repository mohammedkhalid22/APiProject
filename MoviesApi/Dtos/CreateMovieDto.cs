namespace MoviesApi.Dtos
{
    public class CreateMovieDto
    {
       
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string Storeline { get; set; }

        //IFormFile To send img from File
        public IFormFile Poster { get; set; }
        public byte genreId { get; set; }
    }
}
