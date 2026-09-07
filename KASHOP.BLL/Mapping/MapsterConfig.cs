using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using System.Globalization;

namespace KASHOP.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.User, src => src.CreatedBy != null ? src.CreatedBy.UserName : null)
                .Map(dest => dest.Name, src => src.Translations.FirstOrDefault(t => t.Language == CultureInfo.CurrentUICulture.Name).Name); ;

            //.Map(dest => dest.Name, src => src.Translations.Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString().Select(t => t.Name).FirstOrDefault());
            //بالكود  الغير معتمد رح نحتاج نرسل اللغة مع كل ريكوست عشان يجيب الترجمة حسب اللغة المرسلة بالريكوست
            //اما بتعديل الجديد رح يجيب اللغة بشكل تلقائي بدون ما نرسل اي قيمة بالريكوست
        }
    }
}