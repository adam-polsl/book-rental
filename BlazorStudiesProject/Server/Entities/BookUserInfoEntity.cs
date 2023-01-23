namespace BlazorStudiesProject.Server.Entities
{
    public class BookUserInfoEntity
    {
        public int CurrentPage { get; set; }

        public long BookId { get; set; }
        public virtual BookEntity Book { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
