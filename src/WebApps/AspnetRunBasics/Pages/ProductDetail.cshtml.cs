﻿using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public ProductDetailModel(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            Product = await _catalogService.GetCatalog(productId);
            
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            CatalogModel product = await _catalogService.GetCatalog(productId);

            string userName = "pepe";

            BasketModel basket = await _basketService.GetBasket(userName);

            BasketItemExtendedModel item = basket.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != default)
            {
                item.Quantity++;
            }
            else
            {
                basket.Items.Add(new BasketItemExtendedModel
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    Color = "black"
                });
            }

            BasketModel basketUpdated = await _basketService.UpdateBasket(basket);

            return RedirectToPage("Cart");

        }
    }
}