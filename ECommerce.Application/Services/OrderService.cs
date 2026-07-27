using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository ,IUnitOfWork unitOfWork , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            //items [order items] basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId, ct);
            if (basket is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound(Description: "Basket Not Found! "));
            if(basket.Items.Count ==0)
                return Result<OrderToReturnDto>.Fail(Error.Validation(Description: "Basket Has No Products"));

            var orderItems = new List<OrderItem>(basket.Items.Count);
            var ProuctsIds = basket.Items.Select(X => X.Id).ToHashSet();//
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(new ProductWithIdSpecification(ProuctsIds), ct)).ToDictionary(X=>X.Id); // get all is a overhad performance and not needed data we need just Ids of specific products // ToDictionary(X=>X.Id) with key id //o(1)

            foreach (var item in basket.Items)//must loop in basket not products to get the quantity
            {
                if (!products.TryGetValue(item.Id , out var product))
                    return Result<OrderToReturnDto>.Fail(Error.NotFound(Description: "Product Not Found! "));//لو المنتج اتمسح من الداتابيز بعد ما المستخدم حطه في الباسكت
                //add product to orderItems list 
                orderItems.Add(new OrderItem
                {
                    Price = product.Price,//database
                    Quantity = item.Quantity, //basket
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Description = product.Description,
                        PictureUrl = product.PictureUrl
                    }

                });
            }

            //ship to address

            var OrderAddress = new OrderAddress()
            {
                FirstName = orderDto.ShipToAddress.FirstName,
                LastName = orderDto.ShipToAddress.LastName,
                City = orderDto.ShipToAddress.City,
                Street = orderDto.ShipToAddress.Street,
                Country = orderDto.ShipToAddress.Country     

            }; //or map with mapper

            //deliverymethod
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if(deliveryMethod is null) 
                return Result<OrderToReturnDto>.Fail(Error.NotFound(Description: "DeliveryMethod Not Found! "));

            //subtotal => price of eachitem + quantity
            var subtotal = orderItems.Sum(X => X.Price * X.Quantity);
            //createOrder
            var order = new Order(email, OrderAddress, orderItems, subtotal, deliveryMethod);
            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result == 0)
            {
                return Result<OrderToReturnDto>.Fail(Error.Failure(Description: "Failed to create order !"));
            }
            else
            {
                await _basketRepository.DeleteBasketAsync(orderDto.BasketId,ct);
                return Result<OrderToReturnDto>.OK(_mapper.Map<OrderToReturnDto>(order));
            }
        }
    }
}
