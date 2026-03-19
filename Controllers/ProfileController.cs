using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praktika.Data;
using praktika.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace praktika.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.IdUser == userId);
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            ViewBag.Orders = orders;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string email)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(email))
            {
                if (email.Length > 200)
                {
                    TempData["Error"] = "Email не может быть длиннее 200 символов";
                    return RedirectToAction(nameof(Index));
                }

                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(email))
                {
                    TempData["Error"] = "Введите корректный email";
                    return RedirectToAction(nameof(Index));
                }
            }

            user.Email = email;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Данные успешно обновлены!";
            return RedirectToAction(nameof(Index));
        }
    }
}