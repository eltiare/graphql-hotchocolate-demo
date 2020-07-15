using Microsoft.EntityFrameworkCore;

namespace HotChocolateServer
{
    public class DataStore : DbContext
    {
        public DataStore(DbContextOptions<DataStore> opts) : base(opts)
        {
               
        }
        
       public DbSet<Post> Posts { get; set; }
       public DbSet<Comment> Comments { get; set; }
    }
}