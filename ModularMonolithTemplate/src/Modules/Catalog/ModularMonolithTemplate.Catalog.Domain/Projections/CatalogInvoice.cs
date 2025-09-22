namespace ModularMonolithTemplate.Billing.Domain.Projections;

public sealed class CatalogInvoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BillingInvoiceId { get; set; }
    public string CustomerId { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
