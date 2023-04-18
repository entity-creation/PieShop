using Microsoft.EntityFrameworkCore;
using System;

namespace PieShop.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly PieShopDbContext _pieShopDbContext;

        public string? ShoppingCartId { get; set; }
        public ShoppingCart(PieShopDbContext pieShopDbContext)
        {
            _pieShopDbContext = pieShopDbContext;
        }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        public void AddToCart(Pie pie)
        {
            //var ShoppingCartItem = (from shoppingCartItem in _pieShopDbContext.ShoppingCartItems
            //                       select shoppingCartItem).SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            var shoppingCartItem = _pieShopDbContext.ShoppingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem == null)
            {
                //_pieShopDbContext.Add( new ShoppingCartItem { ShoppingCartId = ShoppingCartId, Pie = pie, Amount = 1 } );
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };
                _pieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _pieShopDbContext.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = from shoppingCartItem in _pieShopDbContext.ShoppingCartItems
                            where shoppingCartItem.ShoppingCartId == ShoppingCartId
                            select shoppingCartItem;

            _pieShopDbContext.RemoveRange(cartItems);
            _pieShopDbContext.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _pieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId).Include(s => s.Pie).ToList();
        }

        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem = _pieShopDbContext.ShoppingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId ==ShoppingCartId);
            var localAmount = 0;
            if (shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _pieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _pieShopDbContext.SaveChanges();
            return localAmount;
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _pieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId).Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            PieShopDbContext context = services.GetService<PieShopDbContext>() ?? throw new Exception("Error Initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            
            return new ShoppingCart(context) { ShoppingCartId = cartId};
        }
    }
}
