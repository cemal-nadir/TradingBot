using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Data;

public class AspNetIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options)
        : base(options)
    {
       
    }
}