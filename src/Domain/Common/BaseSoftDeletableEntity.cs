namespace Domain.Common
{
    public class BaseSoftDeletableEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}
