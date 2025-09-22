using System.Diagnostics;

namespace ModularMonolithTemplate.Billing;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Billing");
}
