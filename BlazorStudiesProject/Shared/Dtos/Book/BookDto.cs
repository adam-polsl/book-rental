namespace BlazorStudiesProject.Shared.Dtos.Book
{
    public class BookDto
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
    }
}
