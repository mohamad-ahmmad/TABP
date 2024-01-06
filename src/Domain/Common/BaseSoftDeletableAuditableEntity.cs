namespace Domain.Common
{
    public class BaseSoftDeletableAuditableEntity : BaseSoftDeletableEntity
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}

