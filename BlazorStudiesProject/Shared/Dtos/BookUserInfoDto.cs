namespace BlazorStudiesProject.Shared.Dtos
{
    public class BookUserInfoDto
    {
        public bool Owned { get; set; }
        public int CurrentPage { get; set; }
        public DateTime ValidUntill { get; set; }
    }
}
