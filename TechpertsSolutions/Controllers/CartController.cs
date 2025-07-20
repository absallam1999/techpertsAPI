using Core.DTOs.Cart;
using Microsoft.AspNetCore.Mvc;
using TechpertsSolutions.Core.DTOs;
using System;
using System.Threading.Tasks;
using Core.Interfaces.Services;

namespace TechpertsSolutions.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService _cartService)
        {
            cartService = _cartService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || !Guid.TryParse(customerId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Customer ID.",
                    Data = null
                });
            }

            try
            {
                var cart = await cartService.GetCartByCustomerIdAsync(customerId);

                if (cart == null)
                {
                    return NotFound(new GeneralResponse<string>
                    {
                        Success = false,
                        Message = $"❌ Cart not found for customer ID {customerId}.",
                        Data = null
                    });
                }

                return Ok(new GeneralResponse<CartReadDTO>
                {
                    Success = true,
                    Message = "✅ Cart retrieved successfully.",
                    Data = cart
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("{customerId}/items")]
        public async Task<IActionResult> AddItem(string customerId, [FromForm] CartItemDTO itemDto)
        {
            if (itemDto == null || string.IsNullOrWhiteSpace(itemDto.ProductId) || itemDto.Quantity <= 0)
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid item data. ProductId and Quantity (> 0) are required.",
                    Data = null
                });
            }

            var resultMessage = await cartService.AddItemAsync(customerId, itemDto);
            var isSuccess = resultMessage.StartsWith("✅");

            return isSuccess
                ? Ok(new GeneralResponse<string> { Success = true, Message = resultMessage.TrimStart('✅', ' ').Trim(), Data = null })
                : BadRequest(new GeneralResponse<string> { Success = false, Message = resultMessage.TrimStart('❌', ' ').Trim(), Data = null });
        }

        [HttpPut("{customerId}/items")]
        public async Task<IActionResult> UpdateItemQuantity(string customerId, [FromForm] CartUpdateItemQuantityDTO updateDto)
        {
            if (updateDto == null || string.IsNullOrWhiteSpace(updateDto.ProductId) || updateDto.Quantity <= 0)
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid update data. ProductId and Quantity (> 0) are required.",
                    Data = null
                });
            }

            var resultMessage = await cartService.UpdateItemQuantityAsync(customerId, updateDto);
            var isSuccess = resultMessage.StartsWith("✅");

            return isSuccess
                ? Ok(new GeneralResponse<string> { Success = true, Message = resultMessage.TrimStart('✅', ' ').Trim(), Data = null })
                : NotFound(new GeneralResponse<string> { Success = false, Message = resultMessage.TrimStart('❌', ' ').Trim(), Data = null });
        }

        [HttpDelete("{customerId}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(string customerId, string productId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(productId))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Customer ID and Product ID are required.",
                    Data = null
                });
            }

            var resultMessage = await cartService.RemoveItemAsync(customerId, productId);
            var isSuccess = resultMessage.StartsWith("✅");

            return isSuccess
                ? Ok(new GeneralResponse<string> { Success = true, Message = resultMessage.TrimStart('✅', ' ').Trim(), Data = null })
                : NotFound(new GeneralResponse<string> { Success = false, Message = resultMessage.TrimStart('❌', ' ').Trim(), Data = null });
        }

        [HttpDelete("{customerId}/clear")]
        public async Task<IActionResult> ClearCart(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Customer ID is required.",
                    Data = null
                });
            }

            var resultMessage = await cartService.ClearCartAsync(customerId);
            var isSuccess = resultMessage.StartsWith("✅");

            return isSuccess
                ? Ok(new GeneralResponse<string> { Success = true, Message = resultMessage.TrimStart('✅', ' ').Trim(), Data = null })
                : NotFound(new GeneralResponse<string> { Success = false, Message = resultMessage.TrimStart('❌', ' ').Trim(), Data = null });
        }

        [HttpPost("{customerId}/checkout")]
        public async Task<IActionResult> Checkout(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || !Guid.TryParse(customerId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Customer ID.",
                    Data = null
                });
            }

            var result = await cartService.PlaceOrderAsync(customerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            if (result.Success)
                return Ok(result);

            if (result.Message.Contains("not found") || result.Message.Contains("empty"))
                return NotFound(result);

            if (result.Message.Contains("stock") || result.Message.Contains("validation"))
                return Conflict(result);

            return BadRequest(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckoutWithDetails([FromBody] CartCheckoutDTO checkoutDto)
        {
            if (checkoutDto == null || string.IsNullOrWhiteSpace(checkoutDto.CustomerId))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Customer ID is required.",
                    Data = null
                });
            }

            if (!Guid.TryParse(checkoutDto.CustomerId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Customer ID format.",
                    Data = null
                });
            }

            if (!string.IsNullOrWhiteSpace(checkoutDto.DeliveryId) && !Guid.TryParse(checkoutDto.DeliveryId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Delivery ID format.",
                    Data = null
                });
            }

            if (!string.IsNullOrWhiteSpace(checkoutDto.SalesManagerId) && !Guid.TryParse(checkoutDto.SalesManagerId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Sales Manager ID format.",
                    Data = null
                });
            }

            if (!string.IsNullOrWhiteSpace(checkoutDto.ServiceUsageId) && !Guid.TryParse(checkoutDto.ServiceUsageId, out _))
            {
                return BadRequest(new GeneralResponse<string>
                {
                    Success = false,
                    Message = "❌ Invalid Service Usage ID format.",
                    Data = null
                });
            }

            var result = await cartService.PlaceOrderAsync(
                checkoutDto.CustomerId,
                checkoutDto.DeliveryId,
                checkoutDto.SalesManagerId,
                checkoutDto.ServiceUsageId
            );

            if (result.Success)
                return Ok(result);

            if (result.Message.Contains("not found") || result.Message.Contains("empty"))
                return NotFound(result);

            if (result.Message.Contains("stock") || result.Message.Contains("validation"))
                return Conflict(result);

            return BadRequest(result);
        }
    }
}