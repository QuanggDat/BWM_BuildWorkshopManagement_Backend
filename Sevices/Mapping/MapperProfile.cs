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
            // Chỉ xài mapper cho model get, create
            // Những model để xài cho api uodate thì không nên xài map như thế này, nên tự map bằng tay vè rất dễ bị update cả id.

            // User
            CreateMap<UserCreateModel, User>();
            CreateMap<User, UserModel>();

            // Item
            CreateMap<Item, ItemModel>().ReverseMap();

            // Material
            CreateMap<MaterialCategory, MaterialCategoryModel>().ReverseMap();

        }
    }
}
