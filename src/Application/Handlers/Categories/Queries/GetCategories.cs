using Data.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Categories.Queries;

public class GetCategories
{
    public class Query : IRequest<IEnumerable<Category>> {}
    
    public class QueryHandler : IRequestHandler<Query, IEnumerable<Category>>
    {
        private readonly DatabaseContext _databaseContext;

        public QueryHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Category>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _databaseContext.Categories.ToListAsync(cancellationToken);
        }
    }
}