using Application.Interfaces;
using Data.Entities;
using Infrastructure.Data;

namespace Application.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(DatabaseContext context) : base(context) {}
}