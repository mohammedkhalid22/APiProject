namespace MoviesApi.Dtos
{
    public class MoviesDetailsDto
    {
        public int Id { get; set; }
       
        public string Tittle { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
       
        public string Storeline { get; set; }
        public byte[] Poster { get; set; }
        public byte genreId { get; set; }
        public string Genre { get; set; }
    }
}
