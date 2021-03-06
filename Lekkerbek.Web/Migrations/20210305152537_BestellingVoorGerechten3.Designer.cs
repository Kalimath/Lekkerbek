﻿// <auto-generated />
using System;
using Lekkerbek.Web.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lekkerbek.Web.Migrations
{
    [DbContext(typeof(BestellingDbContext))]
    [Migration("20210305152537_BestellingVoorGerechten3")]
    partial class BestellingVoorGerechten3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<DateTime>("Leverdatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opmerkingen")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int?>("BestellingId")
                        .HasColumnType("int");

                    b.Property<string>("CategorieId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("KlantId")
                        .HasColumnType("int");

                    b.Property<double>("Prijs")
                        .HasColumnType("float");

                    b.HasKey("Naam");

                    b.HasIndex("BestellingId");

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

                    b.Property<string>("Adres")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Geboortedatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("Getrouwheidsscore")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Klanten");
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
                    b.HasOne("Lekkerbek.Web.Models.Bestelling", null)
                        .WithMany("GerechtenLijst")
                        .HasForeignKey("BestellingId");

                    b.HasOne("Lekkerbek.Web.Models.Categorie", "Categorie")
                        .WithMany()
                        .HasForeignKey("CategorieId");

                    b.HasOne("Lekkerbek.Web.Models.Klant", null)
                        .WithMany("Voorkeursgerechten")
                        .HasForeignKey("KlantId");

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Bestelling", b =>
                {
                    b.Navigation("GerechtenLijst");
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
