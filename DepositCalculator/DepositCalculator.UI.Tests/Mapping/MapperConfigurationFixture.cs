using AutoMapper;
using DepositCalculator.UI.Mapping;

namespace DepositCalculator.UI.Tests.Mapping
{
    public class MapperConfigurationFixture
    {
        internal readonly MapperConfiguration _configuration;

        public MapperConfigurationFixture()
        {
            _configuration = new MapperConfiguration(config =>
            {
                config.AddMaps(typeof(DepositMappingsProfile).Assembly);
            });
        }
    }
}