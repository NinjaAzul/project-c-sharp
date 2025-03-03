namespace Project_C_Sharp.Shared.AuditableEntity.Entities;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}