using Application;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class SeedRuntime
{
    public static async Task EnsureDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BlhDbContext>();
        await db.Database.MigrateAsync();

        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var admin = await db.Usuarios.FirstOrDefaultAsync(x => x.Email == "admin@blh.local");
        if (admin is not null && admin.SenhaHash == "seed-admin")
        {
            admin.SenhaHash = hasher.Hash("Admin@123");
            await db.SaveChangesAsync();
        }
    }
}
