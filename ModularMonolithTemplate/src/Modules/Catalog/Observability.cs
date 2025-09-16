using System.Diagnostics;

namespace WebApp.Modules.Catalog;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Catalog");
}
