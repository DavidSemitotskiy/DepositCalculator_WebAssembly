using DepositCalculator.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DepositCalculator.BLL.Tests.Extensions
{
    internal class ServiceCollectionExtensionsTests
    {
        private Mock<IServiceCollection> _serviceCollectionMock;

        [SetUp]
        public void Setup()
        {
            _serviceCollectionMock = new Mock<IServiceCollection>();
        }

        [Test]
        public void AddBusinessLogicServices_AddingServicesToServiceCollection_ShouldAddExpectedServices()
        {
            // Arrange

            // Act
            BLL.Extensions.ServiceCollectionExtensions.AddBusinessLogicServices(_serviceCollectionMock.Object);

            // Assert
            ContainsService<IDepositCalculationStrategy, SimpleDepositCalculationStrategy>(ServiceLifetime.Scoped);

            ContainsService<IDepositCalculationStrategy, CompoundDepositCalculationStrategy>(ServiceLifetime.Scoped);

            ContainsService<IDepositCalculationStrategyResolver, DepositCalculationStrategyResolver>(ServiceLifetime.Scoped);

            ContainsService<IDepositCalculationStrategyFactory, DepositCalculationStrategyFactory>(ServiceLifetime.Scoped);

            ContainsService<IDepositCalculatorService, DepositCalculatorService>(ServiceLifetime.Scoped);
        }

        private void ContainsService<TService, TImplementation>(ServiceLifetime serviceLifetime)
        {
            _serviceCollectionMock.Verify(
                services => services.Add(It.Is<ServiceDescriptor>(serviceDescriptor =>
                    IsDescriptorRegisterred<TService, TImplementation>(serviceDescriptor, serviceLifetime))),
                Times.Once);
        }

        private bool IsDescriptorRegisterred<TService, TImplementation>(ServiceDescriptor serviceDescriptor, ServiceLifetime serviceLifetime)
        {
            return serviceDescriptor.ServiceType == typeof(TService)
                && serviceDescriptor.ImplementationType == typeof(TImplementation)
                && serviceDescriptor.Lifetime == serviceLifetime;
        }
    }
}