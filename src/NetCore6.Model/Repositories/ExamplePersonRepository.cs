using NetCore6.Model.Contexts;
using NetCore6.Model.Models;

namespace NetCore6.Model.Repositories
{
    public interface IExamplePersonRepository : IBaseRepository<ExamplePerson>{}

    public class ExamplePersonRepository: BaseRepository<ExamplePerson>, IExamplePersonRepository
    {
        public ExamplePersonRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}