﻿// <auto-generated />
using System;
using Lekkerbek.Web.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lekkerbek.Web.Migrations
{
    [DbContext(typeof(BestellingDbContext))]
    partial class BestellingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BestellingGerecht", b =>
                {
                    b.Property<int>("BestellingenId")
                        .HasColumnType("int");

                    b.Property<string>("GerechtenLijstNaam")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BestellingenId", "GerechtenLijstNaam");

                    b.HasIndex("GerechtenLijstNaam");

                    b.ToTable("BestellingGerecht");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Bestelling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AantalMaaltijden")
                        .HasColumnType("int");

                    b.Property<int>("KlantId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Levertijd")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opmerkingen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Tijdslot")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("KlantId");

                    b.ToTable("Bestellingen");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Categorie", b =>
                {
                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Naam");

                    b.ToTable("Categorie");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Gerecht", b =>
                {
                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategorieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("KlantId")
                        .HasColumnType("int");

                    b.Property<double>("Prijs")
                        .HasColumnType("float");

                    b.HasKey("Naam");

                    b.HasIndex("CategorieId");

                    b.HasIndex("KlantId");

                    b.ToTable("Gerecht");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Klant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Adres")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Geboortedatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("Getrouwheidsscore")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Klanten");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Tijdslot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsVrij")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Tijdstip")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tijdsloten");
                });

            modelBuilder.Entity("BestellingGerecht", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Bestelling", null)
                        .WithMany()
                        .HasForeignKey("BestellingenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lekkerbek.Web.Models.Gerecht", null)
                        .WithMany()
                        .HasForeignKey("GerechtenLijstNaam")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Bestelling", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Klant", "Klant")
                        .WithMany("Bestellingen")
                        .HasForeignKey("KlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Klant");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Gerecht", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Categorie", "Categorie")
                        .WithMany()
                        .HasForeignKey("CategorieId");

                    b.HasOne("Lekkerbek.Web.Models.Klant", null)
                        .WithMany("Voorkeursgerechten")
                        .HasForeignKey("KlantId");

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Klant", b =>
                {
                    b.Navigation("Bestellingen");

                    b.Navigation("Voorkeursgerechten");
                });
#pragma warning restore 612, 618
        }
    }
}
