using NetCore6.Bl.DTOs;
using NetCore6.Model.Models;
using NetCore6.Services.Services;

namespace NetCore6.Api.Controllers
{
    public class ExamplePersonController: BaseController<ExamplePerson, ExamplePersonDTO>
    {
        public ExamplePersonController(IExamplePersonService service) : base(service)
        {
            
        }
    }
}