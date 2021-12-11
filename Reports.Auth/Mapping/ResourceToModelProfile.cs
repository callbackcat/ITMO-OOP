using AutoMapper;
using Reports.Auth.Controllers.Resources;
using Reports.Auth.Core.Models;

namespace Reports.Auth.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile() =>
            CreateMap<UserCredentialsResource, User>();
    }
}