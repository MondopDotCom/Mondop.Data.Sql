namespace Mondop.Data.Sql
{
    public class StatementBuilderOptions
    {
        public bool UseRowVersionForUpdate { get; set; } = true;
        public bool UseRowVersionForDelete { get; set; } = true;
    }
}
