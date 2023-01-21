using BoutiqueLoginRegister.DataContext;
using BoutiqueLoginRegister.Models;
using BoutiqueLoginRegister.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoutiqueLoginRegister.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<AppUser> _userManager { get; }
        public AppDbContext _appDbContext { get; }
        public RoleManager<IdentityRole> _roleManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

        public AccountController(UserManager<AppUser> userManager, AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole superAdmin = new IdentityRole("SuperAdmin");
        //    IdentityRole admin = new IdentityRole("Admin");
        //    IdentityRole member = new IdentityRole("Member");

        //    await _roleManager.CreateAsync(superAdmin);
        //    await _roleManager.CreateAsync(admin);
        //    await _roleManager.CreateAsync(member);

        //    return Ok("Roles was created");
        //}

        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser appUser = await _userManager.FindByNameAsync("muradali");

        //    await _userManager.AddToRoleAsync(appUser, "Admin");

        //    return Ok("Role was added");
        //}


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterViewModel)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = null;

            appUser = await _userManager.FindByNameAsync(memberRegisterViewModel.Username);

            if (appUser != null)
            {
                ModelState.AddModelError("Username", "Already exists!");
                return View();
            }

            //appUser = await _userManager.FindByEmailAsync(memberRegisterViewModel.Email);

            appUser = _appDbContext.Users.FirstOrDefault(x => x.NormalizedEmail == memberRegisterViewModel.Username.ToUpper());

            if (appUser != null)
            {
                ModelState.AddModelError("Email", "Already exists!");
                return View();
            }

            appUser = new AppUser
            {
                UserName = memberRegisterViewModel.Username,
                Email = memberRegisterViewModel.Email,
            };

            var result = await _userManager.CreateAsync(appUser, memberRegisterViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginViewModel)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByNameAsync(memberLoginViewModel.Username);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, memberLoginViewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }

    }
}
