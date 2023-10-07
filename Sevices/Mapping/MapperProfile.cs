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
            CreateMap<UserCreateModel, User>();
            CreateMap<User, UserModel>();
            CreateMap<Item, ItemModel>();
            CreateMap<ItemCategory, ItemCategoryModel>();
            CreateMap<Material, MaterialModel>();
            CreateMap<MaterialCategory, MaterialCategoryModel>();
            CreateMap<Squad, SquadModel>();
        }
    }
}
