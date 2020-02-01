namespace Mondop.Data.Sql.Migrations
{
    public abstract class Migration
    {
        public abstract void Up();
        public abstract void Down();

        protected TableBuilder CreateTable(string tableName)
        {
            return new TableBuilder(tableName);
        }
    }
}
