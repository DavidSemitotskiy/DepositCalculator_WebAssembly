using AutoMapper;
using DepositCalculator.BLL.Mapping;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal abstract class BaseMappingTests
    {
        protected MapperConfiguration _mapperConfiguration;

        protected IMapper _mapper;

        [OneTimeSetUp]
        public void BaseOneTimeSetup()
        {
            _mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddMaps(typeof(DepositMappingsProfile).Assembly);
            });
        }

        [SetUp]
        public void BaseSetup()
        {
            _mapper = new Mapper(_mapperConfiguration);
        }
    }
}
