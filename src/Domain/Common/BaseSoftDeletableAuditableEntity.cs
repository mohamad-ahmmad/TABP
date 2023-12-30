namespace Domain.Common
{
    public class BaseSoftDeletableAuditableEntity : BaseSoftDeletableEntity
    {
        public DateTimeOffset Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}

