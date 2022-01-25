using AutoMapper;
using NetCore6.Bl.DTOs;
using NetCore6.Model.Models;

namespace NetCore6.Bl.Mapper
{
    public class MainProfile: Profile
    {
        public MainProfile()
        {
            CreateMap<ExamplePersonDTO, ExamplePerson>();
            CreateMap<ExamplePerson, ExamplePersonDTO>();
        }  
    }
}