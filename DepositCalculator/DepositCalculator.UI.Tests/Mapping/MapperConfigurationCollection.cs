namespace DepositCalculator.UI.Tests.Mapping
{
    [CollectionDefinition(MapperConfigurationCollection.Name)]
    public class MapperConfigurationCollection : ICollectionFixture<MapperConfigurationFixture>
    {
        internal const string Name = "Mapping collection";
    }
}