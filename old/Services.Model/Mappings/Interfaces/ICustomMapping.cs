using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model.Mappings.Interfaces
{
    public interface ICustomMapping
    {
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
