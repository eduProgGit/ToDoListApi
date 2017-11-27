using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApi.Model;

namespace ToDoListApi
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItem> Items { get; set; }

        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options) { }
    }
}
