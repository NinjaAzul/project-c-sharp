
namespace Project_C_Sharp.Shared.AuditableEntity.Entities;

public abstract class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}