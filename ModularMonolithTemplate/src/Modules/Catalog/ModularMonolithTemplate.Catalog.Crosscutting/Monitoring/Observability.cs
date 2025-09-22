using System.Diagnostics;

namespace ModularMonolithTemplate.Catalog.Croscutting;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Catalog");
}
