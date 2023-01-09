namespace Data.Entities.Product;

public class Variation : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int OptionId { get; set; }
    public Option Option { get; set; }
}