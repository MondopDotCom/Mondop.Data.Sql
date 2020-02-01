using System.Collections.Generic;

namespace Mondop.Data.Sql.Model
{
    public class Command
    {
        public string CommandText { get; set; }
        public List<CommandParameter> Parameters { get; set; } = new List<CommandParameter>();
    }
}
