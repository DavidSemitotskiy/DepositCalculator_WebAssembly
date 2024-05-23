namespace DepositCalculator.Entities
{
    /// <summary>
    /// The base abstract entity with a generic identifier for all entities.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
    public abstract class BaseEntity<TId>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public TId Id { get; set; }
    }
}