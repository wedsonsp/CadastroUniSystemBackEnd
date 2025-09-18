using System;

namespace Sistemaws.Domain.Entities;

public abstract class BaseEntity<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
    public bool Deleted { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
