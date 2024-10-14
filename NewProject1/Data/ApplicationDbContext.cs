using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Banktransactions.Models;
 using System.ComponentModel.DataAnnotations;
namespace Banktransactions.Data
{
    public class ApplicationDbContext: IdentityDbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {

        }
        public DbSet<Branch> Branches { get; set; } 
        public DbSet<Transaction> Transactions { get; set; }
    }
}
