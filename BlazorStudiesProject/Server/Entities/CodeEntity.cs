namespace BlazorStudiesProject.Server.Entities
{
    public class CodeEntity
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public BookEntity Book { get; set; }
        public string Code { get; set; }
        public int MoneyPaid { get; set; }
    }
}
