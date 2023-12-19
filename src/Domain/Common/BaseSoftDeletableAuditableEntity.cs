﻿namespace Domain.Common
{
    public class BaseSoftDeletableAuditableEntity : BaseSoftDeletableEntity
    {
        public DateTimeOffset Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
