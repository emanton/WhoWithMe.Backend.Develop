using AutoMapper;
using AutoMapper.Configuration;
using Services.Model.Mappings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model.Mappings
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// Registers this instance.
        /// </summary>
        public static void Register()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            LoadStandardMappings(types);
            //LoadCustomMappings(types);
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            Mapper.Initialize(cfg => {
                var maps = (from t in types
                            from i in t.GetInterfaces()
                            where i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select new
                            {
                                Source = i.GetGenericArguments()[0],
                                Destination = t
                            }).ToArray();

                foreach (var map in maps)
                {
                    cfg.CreateMap(map.Source, map.Destination);
                }

                var maps1 = (from t in types
                            from i in t.GetInterfaces()
                            where typeof(ICustomMapping).IsAssignableFrom(t) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select (ICustomMapping)Activator.CreateInstance(t)).ToArray();
                foreach (var map in maps1)
                {
                    //var cfg = new MapperConfigurationExpression();
                    map.CreateMappings(cfg);
                }
            });
        }

        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(ICustomMapping).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (ICustomMapping)Activator.CreateInstance(t)).ToArray();
            foreach (var map in maps)
            {
                var cfg = new MapperConfigurationExpression();
                map.CreateMappings(cfg);
            }
        }
    }
}
