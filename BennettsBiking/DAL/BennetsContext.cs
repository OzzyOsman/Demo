using BennettsBiking.DomainModels;
using System.Data.Entity;

namespace BennettsBiking.DAL
{
    public class BennetsContext : DbContext
    {
        public BennetsContext(): base()
        {

        }

        public DbSet<User> User { get; set; }
    }
}