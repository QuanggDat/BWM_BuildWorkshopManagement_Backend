using AutoMapper;
using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //User
            CreateMap<UserCreateModel, User>();
            CreateMap<User, UserModel>();

            //Item
            CreateMap<Item, ItemModel>();
            CreateMap<ItemMaterial, ItemMaterialModel>();

            //Material
            CreateMap<Material, ResponeMaterialModel>();
            CreateMap<MaterialCategory, ResponeMaterialCategoryModel>();

            // Order
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<Order, CreateOrderModel>().ReverseMap();
            CreateMap<Order, QuoteMaterialOrderModel>().ReverseMap();

            // Order Detail
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();

            //HumanResources
            CreateMap<Squad, SquadModel>();
            CreateMap<Group, GroupModel>();

            // Notification
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Notification, NotificationCreateModel>().ReverseMap();

            // Area
            CreateMap<Area, AreaModel>().ReverseMap();

            // Floor
            CreateMap<Floor, FloorModel>().ReverseMap();

        }
    }
}
