using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VideoDownloaderApp.DAL.Models;

namespace VideoDownloaderApp.DAL;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<MediaFile> MediaFiles { get; set; }

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connection = _configuration.GetConnectionString("DevConnection");
        base.OnConfiguring(optionsBuilder.UseSqlite(connection));
    }
}
