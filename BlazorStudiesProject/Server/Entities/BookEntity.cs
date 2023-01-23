namespace BlazorStudiesProject.Server.Entities
{
    public class BookEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }

        public virtual AdditionalInfoEntity AdditionalInfo { get; set; }
        public virtual ContentEntity Content { get; set; }
        public virtual IList<BookUserInfoEntity> BookUserInfos { get; set; }
    }
}
