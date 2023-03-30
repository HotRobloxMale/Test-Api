using Microsoft.EntityFrameworkCore;
using testapi.Models;
namespace testapi.Data
{
    public class ApiDbContext:DbContext
    {
        public ApiDbContext(DbContextOptions option) : base(option)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
    }
}
