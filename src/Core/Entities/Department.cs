namespace Data.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<Category> Categories { get; set; }
}