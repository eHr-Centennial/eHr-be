using NetCore6.Core.BaseModel;

namespace NetCore6.Model.Models
{
    public class ExamplePerson: BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}