using Application.Models.Category;

namespace Application.Models.Department;

public class DepartmentDto
{
    public string Name { get; set; }
    public List<CategoryDto> Categories { get; set; }
}