using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Stock.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Stock.API klasöründeki appsettings.json dosyasını okumak için yolu ayarla
            string apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "../Stock.API");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json")
                // Geliştirme ortamı için appsettings.Development.json dosyasını da ekleyebiliriz (opsiyonel)
                .AddJsonFile("appsettings.Development.json", optional: true)
                // User Secrets'ı ekle - doğru UserSecretsId ile
                .AddUserSecrets("6c100518-0687-4967-890e-d7cf3ea45f59")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
} 