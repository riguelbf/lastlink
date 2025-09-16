using System.Diagnostics;

namespace WebApp.Modules.Billing;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Billing");
}
