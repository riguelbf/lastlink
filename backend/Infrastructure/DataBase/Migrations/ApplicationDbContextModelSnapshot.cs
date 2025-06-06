﻿// <auto-generated />
using System;
using Infrastructure.DataBase.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.backend.Infrastructure.DataBase.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.AdvanceRequests.AdvanceRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("approved_date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("creator_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_at");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("modified_by");

                    b.Property<decimal>("NetAmount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("net_amount");

                    b.Property<DateTime?>("RejectedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("rejected_date");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("request_date");

                    b.Property<decimal>("RequestedAmount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("requested_amount");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_advance_requests");

                    b.HasIndex("CreatorId")
                        .HasDatabaseName("ix_advance_requests_creator_id");

                    b.HasIndex("Status")
                        .HasDatabaseName("ix_advance_requests_status");

                    b.ToTable("AdvanceRequests", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
