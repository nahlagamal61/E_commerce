

namespace EcommerceTask.Profillers
{
    using AutoMapper;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
     public class Profiler : Profile
    {
        public Profiler()
        {
            CreateMap<Order, OrderCreate_VM>().
                ForMember(x => x.OrderLineDetails, x => x.MapFrom(v => v.OrderLineDetails));
            CreateMap<OrderCreate_VM, Order>()
                .ForMember(x => x.OrderLineDetails, v=> v.Ignore());
            CreateMap<OrderLineDetails, OrderLineDetails_VM>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO_Response>().ReverseMap();
            CreateMap<Product, Product_Create_DTO>().ReverseMap();
            CreateMap<Product, Product_Response>().ReverseMap();
            CreateMap<Product, UpdateProduct_VM>().ReverseMap();
            
            CreateMap<Order_Response, Order>().ReverseMap();
            CreateMap< OrderLineDetails, OrderLineDetails_Response>()
                .ForMember(x => x.Name , v => v.MapFrom(res => res.Product.Name))
                .ForMember(x => x.Description , v => v.MapFrom(res => res.Product.Description))
                .ForMember(x => x.StockQuantity , v => v.MapFrom(res => res.Product.StockQuantity))
                .ReverseMap();
          }

    }
}
