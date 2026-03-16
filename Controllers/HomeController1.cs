using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praktika.Data;
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
        public IActionResult Add(int id)
        {
            var cart = GetCart();
            if (cart.ContainsKey(id)) cart[id]++;
            else cart[id] = 1;
            SaveCart(cart);
            return RedirectToAction("Index", "Products");
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

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction(nameof(Index));
            HttpContext.Session.Remove(CartKey);
            TempData["Success"] = "Заказ успешно оформлен! Мы свяжемся с вами в ближайшее время.";
            return RedirectToAction("Index", "Home");
        }
    }
}