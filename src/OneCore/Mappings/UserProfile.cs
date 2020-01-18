using AutoMapper;
using OneCore.Data.Models;
using OneCore.ViewModels;

namespace OneCore.Mappings
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
        }
    }
}
