using GemmaTodoData.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemmaTodoData.My_DbContext
{
    public class GemmaDbContext : DbContext
    {
        public GemmaDbContext()
            :base("name = conn")
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
