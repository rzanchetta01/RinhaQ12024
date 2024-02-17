﻿// <auto-generated />
using System;
using Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Api.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Limite")
                        .HasColumnType("integer");

                    b.Property<int>("Saldo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Api.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<int?>("IdCliente")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Realizada_em")
                        .HasColumnType("timestamp with time zone");

                    b.Property<char>("Tipo")
                        .HasMaxLength(1)
                        .HasColumnType("character(1)");

                    b.Property<int>("Valor")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdCliente");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Api.Models.Transaction", b =>
                {
                    b.HasOne("Api.Models.Client", "Cliente")
                        .WithMany("Transactions")
                        .HasForeignKey("IdCliente");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("Api.Models.Client", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
