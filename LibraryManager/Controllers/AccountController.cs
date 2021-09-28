using LibraryManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //POST api/account/RegisterUser
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
            //return CreatedAtRoute(nameof(GetLibraryByID), new { Id = libraryReadDTO.Id, libraryReadDTO });

        }

        //POST api/account/User/IsSignedIn
        [HttpPost]
        [Route("User/IsSignedIn")]
        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return signInManager.IsSignedIn(user);            
        }

        //POST api/account/User/SignOut
        [HttpPost]
        [Route("User/SignOut")]
        public async Task SignOutUser()
        {
            await signInManager.SignOutAsync();
        }
    }
}
