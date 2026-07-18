using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class BasketController :ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        //Get :: baseurl/api/basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasket(string id ,CancellationToken ct)
        {
            var result = await _basketService.GetBasketAsync(id ,ct);
            return ToActionResult(result);
        }

        //Post ::  baseurl/api/basket =>body

        [HttpPost]
        public async Task<ActionResult<BasketDto>>CreateOrUpdateBasket(BasketDto basket ,CancellationToken ct)
        {
            var result = await _basketService.CreateOrUpdateAsync(basket ,ct:ct);
            return ToActionResult(result);
        }
        // Delete ::  baseurl/api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await _basketService.DeleteBasketAsync(id ,ct);
            return ToActionResult(result);
        }
    }
}
