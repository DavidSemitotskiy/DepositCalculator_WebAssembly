namespace DepositCalculator.DAL.Utils
{
    /// <summary>
    /// The options of database for deposit information.
    /// </summary>
    public class DepositDatabaseOptions
    {
        /// <summary>
        /// The section name with options for database.
        /// </summary>
        public const string SectionName = "DepositDatabaseOptions";

        /// <summary>
        /// Gets or sets connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly containing database migrations.
        /// </summary>
        public string MigrationsAssembly { get; set; }
    }
}