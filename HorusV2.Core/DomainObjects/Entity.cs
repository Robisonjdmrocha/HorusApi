namespace HorusV2.Core.DomainObjects;

public abstract class Entity
{
    protected Entity()
    {
    }

    protected Entity(Guid uniqueIdentifier, int id)
    {
        UniqueIdentifier = uniqueIdentifier;
        Id = id;
    }

    public Guid UniqueIdentifier { get; set; }
    public int Id { get; set; }
}