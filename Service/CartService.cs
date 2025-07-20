using Core.DTOs.Cart;
using Core.DTOs.Orders;
using Core.Interfaces;
using Core.Interfaces.Services;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechpertsSolutions.Core.DTOs;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> cartRepo;
        private readonly IRepository<CartItem> cartItemRepo;
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Customer> customerRepo;
        private readonly IRepository<Order> orderRepo;

        public CartService(
            IRepository<Cart> _cartRepo,
            IRepository<CartItem> _cartItemRepo,
            IRepository<Product> _productRepo,
            IRepository<Customer> _customerRepo,
            IRepository<Order> _orderRepo,
            IRepository<OrderItem> _orderItemRepo)
        {
            cartRepo = _cartRepo;
            cartItemRepo = _cartItemRepo;
            productRepo = _productRepo;
            customerRepo = _customerRepo;
            orderRepo = _orderRepo;
        }

        public async Task<CartReadDTO?> GetCartByCustomerIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return null;
            }

            if (!Guid.TryParse(customerId, out _))
            {
                return null;
            }

            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                return null;
            }

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems.Product"
            );

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };

                await cartRepo.AddAsync(cart);
                await cartRepo.SaveChangesAsync();
            }

            return CartMapper.MapToCartReadDTO(cart);
        }

     
        public async Task<string> AddItemAsync(string customerId, CartItemDTO itemDto)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return "❌ Customer ID cannot be null or empty.";

            if (!Guid.TryParse(customerId, out _))
                return "❌ Invalid Customer ID format. Expected GUID format.";

            if (itemDto == null)
                return "❌ Item data cannot be null.";

            if (string.IsNullOrWhiteSpace(itemDto.ProductId))
                return "❌ Product ID cannot be null or empty.";

            if (!Guid.TryParse(itemDto.ProductId, out _))
                return "❌ Invalid Product ID format. Expected GUID format.";

            if (itemDto.Quantity <= 0)
                return "❌ Quantity must be greater than zero.";

            if (itemDto.Quantity > 1000)
                return "❌ Quantity cannot exceed 1000 items.";

            var customer = await customerRepo.GetByIdAsync(customerId);
            if (customer == null)
                return $"❌ Customer with ID {customerId} not found.";

            var product = await productRepo.GetByIdAsync(itemDto.ProductId);
            if (product == null)
                return $"❌ Product with ID {itemDto.ProductId} not found.";

            if (product.Stock < itemDto.Quantity)
                return $"❌ Not enough stock for product '{product.Name}'. Available: {product.Stock}, Requested: {itemDto.Quantity}.";

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems"
            );

            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };
                await cartRepo.AddAsync(cart);
                await cartRepo.SaveChangesAsync(); 
            }

            var existingItem = cart.CartItems?.FirstOrDefault(i => i.ProductId == itemDto.ProductId);

            if (existingItem != null)
            {
                int newQuantity = existingItem.Quantity + itemDto.Quantity;
                if (product.Stock < newQuantity)
                    return $"❌ Cannot add {itemDto.Quantity} more. Total requested ({newQuantity}) exceeds available stock ({product.Stock}).";

                existingItem.Quantity = newQuantity;
                cartItemRepo.Update(existingItem);
                await cartItemRepo.SaveChangesAsync();
                return "✅ Item quantity updated successfully.";
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = itemDto.ProductId,
                    CartId = cart.Id,
                    Quantity = itemDto.Quantity
                };

                await cartItemRepo.AddAsync(newItem);
                await cartItemRepo.SaveChangesAsync();
                return "✅ Item added successfully.";
            }
        }

       
        public async Task<string> UpdateItemQuantityAsync(string customerId, CartUpdateItemQuantityDTO updateDto)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return "❌ Customer ID cannot be null or empty.";

            if (!Guid.TryParse(customerId, out _))
                return "❌ Invalid Customer ID format. Expected GUID format.";

            if (updateDto == null)
                return "❌ Update data cannot be null.";

            if (string.IsNullOrWhiteSpace(updateDto.ProductId))
                return "❌ Product ID cannot be null or empty.";

            if (!Guid.TryParse(updateDto.ProductId, out _))
                return "❌ Invalid Product ID format. Expected GUID format.";

            if (updateDto.Quantity <= 0)
                return "❌ Quantity must be greater than zero. To remove, use the remove endpoint.";

            if (updateDto.Quantity > 1000)
                return "❌ Quantity cannot exceed 1000 items.";

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems.Product"
            );

            if (cart == null)
                return $"❌ Cart not found for customer ID {customerId}.";

            var itemToUpdate = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == updateDto.ProductId);
            if (itemToUpdate == null)
                return $"❌ Product with ID {updateDto.ProductId} not found in cart.";

            if (itemToUpdate.Product == null)
                return "❌ Product details not available for stock check.";

            if (itemToUpdate.Product.Stock < updateDto.Quantity)
                return $"❌ Not enough stock for product '{itemToUpdate.Product.Name}'. Available: {itemToUpdate.Product.Stock}, Requested: {updateDto.Quantity}.";

            itemToUpdate.Quantity = updateDto.Quantity;
            cartItemRepo.Update(itemToUpdate);
            await cartItemRepo.SaveChangesAsync();

            return "✅ Item quantity updated successfully.";
        }


        public async Task<string> RemoveItemAsync(string customerId, string productId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return "❌ Customer ID cannot be null or empty.";

            if (!Guid.TryParse(customerId, out _))
                return "❌ Invalid Customer ID format. Expected GUID format.";

            if (string.IsNullOrWhiteSpace(productId))
                return "❌ Product ID cannot be null or empty.";

            if (!Guid.TryParse(productId, out _))
                return "❌ Invalid Product ID format. Expected GUID format.";

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems"
            );

            if (cart == null)
                return $"❌ Cart not found for customer ID {customerId}.";

            var item = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null)
                return $"❌ Product with ID {productId} not found in cart.";

            cart.CartItems.Remove(item);
            cartItemRepo.Remove(item);
            await cartItemRepo.SaveChangesAsync();

            return "✅ Item removed successfully.";
        }

 
        public async Task<string> ClearCartAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return "❌ Customer ID cannot be null or empty.";

            if (!Guid.TryParse(customerId, out _))
                return "❌ Invalid Customer ID format. Expected GUID format.";

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems"
            );

            if (cart == null)
                return $"❌ Cart not found for customer ID {customerId}.";

            if (cart.CartItems == null || !cart.CartItems.Any())
                return "ℹ️ Cart is already empty.";

            foreach (var item in cart.CartItems.ToList())
            {
                cart.CartItems.Remove(item);
                cartItemRepo.Remove(item);
            }
            await cartItemRepo.SaveChangesAsync();

            return "✅ Cart cleared successfully.";
        }

        public async Task<GeneralResponse<OrderReadDTO>> PlaceOrderAsync(string customerId, string deliveryId, string salesManagerId, string serviceUsageId)
        {
            if (string.IsNullOrWhiteSpace(customerId) || !Guid.TryParse(customerId, out _))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Invalid Customer ID.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(deliveryId) || !Guid.TryParse(deliveryId, out _))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Invalid Delivery ID.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(salesManagerId) || !Guid.TryParse(salesManagerId, out _))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Invalid Sales Manager ID.",
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(serviceUsageId) || !Guid.TryParse(serviceUsageId, out _))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Invalid Service Usage ID.",
                    Data = null
                };
            }

            var cart = await cartRepo.GetFirstOrDefaultAsync(
                c => c.CustomerId == customerId,
                includeProperties: "CartItems.Product,Customer"
            );

            if (cart == null)
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Cart not found for this customer.",
                    Data = null
                };
            }

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Cart is empty. Please add items before placing an order.",
                    Data = null
                };
            }

            if (cart.Customer == null)
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "❌ Customer information not found.",
                    Data = null
                };
            }

            var validationErrors = new List<string>();
            foreach (var item in cart.CartItems)
            {
                if (item.Product == null)
                {
                    validationErrors.Add($"❌ Product with ID {item.ProductId} not found.");
                    continue;
                }

                if (item.Quantity <= 0)
                {
                    validationErrors.Add($"❌ Invalid quantity ({item.Quantity}) for '{item.Product.Name}'.");
                    continue;
                }

                if (item.Product.Stock < item.Quantity)
                {
                    validationErrors.Add($"❌ Not enough stock for '{item.Product.Name}'. Available: {item.Product.Stock}, Requested: {item.Quantity}.");
                }
            }

            if (validationErrors.Any())
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = $"❌ Stock validation failed:\n{string.Join("\n", validationErrors)}",
                    Data = null
                };
            }

            try
            {
                var newOrder = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerId = customerId,
                    CartId = cart.Id,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    DeliveryId = deliveryId,
                    SalesManagerId = salesManagerId,
                    ServiceUsageId = serviceUsageId,
                    OrderItems = new List<OrderItem>()
                };

                decimal totalAmount = 0;

                foreach (var item in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price,
                        ItemTotal = item.Quantity * item.Product.Price
                    };

                    newOrder.OrderItems.Add(orderItem);
                    totalAmount += orderItem.ItemTotal;

                    item.Product.Stock -= item.Quantity;
                    productRepo.Update(item.Product);
                    await productRepo.SaveChangesAsync();
                }

                newOrder.TotalAmount = totalAmount;

                await orderRepo.AddAsync(newOrder);
                await orderRepo.SaveChangesAsync();

                await ClearCartAsync(customerId);

                return new GeneralResponse<OrderReadDTO>
                {
                    Success = true,
                    Message = $"✅ Order placed successfully! Total: ${totalAmount:F2}",
                    Data = CartMapper.MapToOrderReadDTO(newOrder)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = $"❌ Error creating order: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
