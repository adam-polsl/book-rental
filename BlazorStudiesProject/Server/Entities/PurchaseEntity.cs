namespace BlazorStudiesProject.Server.Entities
{
    public abstract class PurchaseEntity
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long BookId { get; set; }
        public BookEntity Book { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime ValidUntill { get; set; }

        public int MoneyPaid { get; set; }
    }


    public class ActivePurchaseEntity : PurchaseEntity
    {

    }

    public class ArchivePurchaseEntity : PurchaseEntity
    {

    }

}
