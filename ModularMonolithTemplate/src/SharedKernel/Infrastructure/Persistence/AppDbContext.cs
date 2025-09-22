using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options) { }

    public static readonly JsonSerializerOptions SafeJson = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
}
