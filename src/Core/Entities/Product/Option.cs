namespace Data.Entities.Product;

public class Option : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Variation> Variations { get; set; }
}