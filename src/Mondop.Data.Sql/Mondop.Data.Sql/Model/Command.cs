using System;
using System.Collections.Generic;
using System.Text;

namespace Mondop.Data.Sql.Model
{
    public class Command
    {
        public string CommandText { get; set; }
        public List<CommandParameter> Parameters { get; set; } = new List<CommandParameter>();
    }
}
