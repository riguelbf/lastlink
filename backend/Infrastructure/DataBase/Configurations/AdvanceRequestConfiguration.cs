using Domain.AdvanceRequests;
using Domain.AdvanceRequests.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AdvanceRequestConfiguration : IEntityTypeConfiguration<AdvanceRequest>
{
    public void Configure(EntityTypeBuilder<AdvanceRequest> builder)
    {
        builder.ToTable("AdvanceRequests");

        builder.HasKey(ar => ar.Id);
        
        builder.Property(ar => ar.CreatorId)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(ar => ar.RequestedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(ar => ar.NetAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(ar => ar.RequestDate)
            .IsRequired();
            
        builder.Property(ar => ar.Status)
            .HasConversion(
                v => v.ToString(),
                v => (AdvanceRequestStatus)Enum.Parse(typeof(AdvanceRequestStatus), v))
            .IsRequired();
            
        builder.Property(ar => ar.ApprovedDate)
            .IsRequired(false);
            
        builder.Property(ar => ar.RejectedDate)
            .IsRequired(false);
            
        builder.Property(ar => ar.CreatedAt)
            .IsRequired();
            
        builder.Property(ar => ar.ModifiedAt)
            .IsRequired(false);
            
        // Indexes
        builder.HasIndex(ar => ar.CreatorId);
        builder.HasIndex(ar => ar.Status);
    }
}
