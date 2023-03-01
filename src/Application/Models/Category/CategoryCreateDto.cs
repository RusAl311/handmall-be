namespace Application.Models.Category;

public class CategoryCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public int DepartmentId { get; set; }
}