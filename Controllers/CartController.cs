using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praktika.Data;
using praktika.Models;
using System.Security.Claims;
using System.Text.Json;

namespace praktika.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        const string CartKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private Dictionary<int, int> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return json == null
                ? new Dictionary<int, int>()
                : JsonSerializer.Deserialize<Dictionary<int, int>>(json)!;
        }

        private void SaveCart(Dictionary<int, int> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => cart.Keys.Contains(p.IdProduct))
                .ToListAsync();
            ViewBag.Cart = cart;
            return View(products);
        }

        [HttpPost]
        public IActionResult Add(int id, string returnUrl = "/")
        {
            var cart = GetCart();
            if (cart.ContainsKey(id)) cart[id]++;
            else cart[id] = 1;
            SaveCart(cart);
            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.Remove(id);
            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            if (quantity <= 0) cart.Remove(id);
            else cart[id] = quantity;
            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartKey);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction(nameof(Index));

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var products = await _context.Products
                .Where(p => cart.Keys.Contains(p.IdProduct))
                .ToListAsync();

            var order = new Order
            {
                IdUser = userId,
                CreatedAt = DateTime.Now,
                Status = "Новый",
                TotalPrice = products.Sum(p => p.Price * cart[p.IdProduct]),
                Items = products.Select(p => new OrderItem
                {
                    IdProduct = p.IdProduct,
                    Quantity = cart[p.IdProduct],
                    Price = p.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);
            TempData["Success"] = "Заказ №" + order.IdOrder + " успешно оформлен! Мы свяжемся с вами в ближайшее время.";
            return RedirectToAction("Index", "Home");
        }
    }
}