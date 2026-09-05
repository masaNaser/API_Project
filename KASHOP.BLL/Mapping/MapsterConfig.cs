using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.User, src=>src.CreatedBy.UserName);
        }
    }
}
