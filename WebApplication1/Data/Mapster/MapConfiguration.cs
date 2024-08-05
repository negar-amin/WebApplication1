using Mapster;
using System.Reflection;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Mapster
{
    public class MapConfiguration
    {
        private static void UpdateMappingNotNullChecker(object src, object dest)
        {
            foreach (PropertyInfo property in src.GetType().GetProperties())
            {
                if (property.GetValue(src) == null)
                {
                    if (dest.GetType().GetProperty(property.Name) is PropertyInfo propertyInfo)
                    {
                        property.SetValue(src, propertyInfo.GetValue(dest));
                    }
                }
            }
        }
        public static void RegisterMapping()
        {
            TypeAdapterConfig<UpdateProductDTO, Product>
                .NewConfig()
                .BeforeMapping((src, dest) =>
                {
                    UpdateMappingNotNullChecker(src, dest);
                }
                );

            TypeAdapterConfig<UpdateUserDTO, User>
                .NewConfig()
                .BeforeMapping((src, dest) =>
                {
                    UpdateMappingNotNullChecker(src, dest);
                }
                );
        }
    }
}
