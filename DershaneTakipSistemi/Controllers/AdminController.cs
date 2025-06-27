using DershaneTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DershaneTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return View(userViewModels);
        }

        // YENİ METODLAR BAŞLANGIÇ

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/DeleteUser/5
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Kullanıcıyı silme onay sayfasına gönder
        }

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string Id) // <-- Parametre adı "Id" olarak değiştirildi
        {
            if (Id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id); // <-- Burada da "Id" kullanılıyor
            if (user != null)
            {
                // Bir adminin kendini silmesini engellemek için ek bir kontrol
                if (user.UserName == User.Identity.Name)
                {
                    // Hata mesajı gösterilebilir veya sadece ana sayfaya yönlendirilebilir.
                    return RedirectToAction(nameof(Index));

                }
                var result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}