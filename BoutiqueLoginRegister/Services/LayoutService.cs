using BoutiqueLoginRegister.Models;
using Microsoft.AspNetCore.Identity;

namespace BoutiqueLoginRegister.Services
{
    public class LayoutService
    {
        public UserManager<AppUser> _userManager { get; }
        public IHttpContextAccessor _httpContextAccessor { get; }

        public LayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppUser> GetUser()
        {
            AppUser appUser = null;

            if(_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                return appUser;
            }

            return null;
        }
    }
}
