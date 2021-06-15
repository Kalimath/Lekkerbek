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
    [DbContext(typeof(IdentityContext))]
    [Migration("20210615121627_AddedIsGesloten")]
    partial class AddedIsGesloten
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
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

            modelBuilder.Entity("GebruikerGerecht", b =>
                {
                    b.Property<string>("VoorkeursgerechtenNaam")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VoorkeursgerechtenVanKlantenId")
                        .HasColumnType("int");

                    b.HasKey("VoorkeursgerechtenNaam", "VoorkeursgerechtenVanKlantenId");

                    b.HasIndex("VoorkeursgerechtenVanKlantenId");

                    b.ToTable("GebruikerGerecht");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Beoordeling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Commentaar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GebruikerId")
                        .HasColumnType("int");

                    b.Property<int>("KlantId")
                        .HasColumnType("int");

                    b.Property<int?>("ScoreLijstId")
                        .HasColumnType("int");

                    b.Property<string>("Titel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotaalScore")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("GebruikerId");

                    b.HasIndex("ScoreLijstId");

                    b.ToTable("Beoordelingen");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Bestelling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AantalMaaltijden")
                        .HasColumnType("int");

                    b.Property<bool>("IsAfgerond")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAfhaling")
                        .HasColumnType("bit");

                    b.Property<int>("KlantId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Levertijd")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opmerkingen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TijdslotId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("KlantId");

                    b.HasIndex("TijdslotId");

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
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Prijs")
                        .HasColumnType("float");

                    b.HasKey("Naam");

                    b.HasIndex("CategorieId");

                    b.ToTable("Gerecht");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Identity.Gebruiker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BtwNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirmaNaam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Geboortedatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Geslacht")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Getrouwheidsscore")
                        .HasColumnType("int");

                    b.Property<bool>("IsProfessional")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

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
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.Dag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VerlofDagenVanGebruikerId")
                        .HasColumnType("int");

                    b.Property<int?>("ZiekteDagenVanGebruikerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VerlofDagenVanGebruikerId");

                    b.HasIndex("ZiekteDagenVanGebruikerId");

                    b.ToTable("Dag");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.OpeningsUur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Dag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGesloten")
                        .HasColumnType("bit");

                    b.Property<DateTime>("SluitingsUur")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Startuur")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("OpeningsUren");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.VerlofDagenVanGebruiker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GebruikerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VerlofDagenVanGebruikers");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.ZiekteDagenVanGebruiker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GebruikerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ZiekteDagenVanGebruikers");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Klacht", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAfgehandeld")
                        .HasColumnType("bit");

                    b.Property<int?>("KlantId")
                        .HasColumnType("int");

                    b.Property<string>("Omschrijving")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Onderwerp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Tijdstip")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("KlantId");

                    b.ToTable("Klachten");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.ScoreLijst", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BeoordelingsId")
                        .HasColumnType("int");

                    b.Property<double>("EtenEnDrinkenScore")
                        .HasColumnType("float");

                    b.Property<double>("HygieneScore")
                        .HasColumnType("float");

                    b.Property<double>("PrijsKwaliteitScore")
                        .HasColumnType("float");

                    b.Property<double>("ServiceScore")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ScoreLijst");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Tijdslot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("InGebruikDoorKokId")
                        .HasColumnType("int");

                    b.Property<bool>("IsVrij")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Tijdstip")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InGebruikDoorKokId");

                    b.ToTable("Tijdslot");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
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

            modelBuilder.Entity("GebruikerGerecht", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Gerecht", null)
                        .WithMany()
                        .HasForeignKey("VoorkeursgerechtenNaam")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("VoorkeursgerechtenVanKlantenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Beoordeling", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany("Beoordelingen")
                        .HasForeignKey("GebruikerId");

                    b.HasOne("Lekkerbek.Web.Models.ScoreLijst", "ScoreLijst")
                        .WithMany()
                        .HasForeignKey("ScoreLijstId");

                    b.Navigation("ScoreLijst");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Bestelling", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", "Klant")
                        .WithMany("Bestellingen")
                        .HasForeignKey("KlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lekkerbek.Web.Models.Tijdslot", "Tijdslot")
                        .WithMany()
                        .HasForeignKey("TijdslotId");

                    b.Navigation("Klant");

                    b.Navigation("Tijdslot");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Gerecht", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Categorie", "Categorie")
                        .WithMany()
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.Dag", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Kalender.VerlofDagenVanGebruiker", null)
                        .WithMany("Dagen")
                        .HasForeignKey("VerlofDagenVanGebruikerId");

                    b.HasOne("Lekkerbek.Web.Models.Kalender.ZiekteDagenVanGebruiker", null)
                        .WithMany("Dagen")
                        .HasForeignKey("ZiekteDagenVanGebruikerId");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Klacht", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", "Klant")
                        .WithMany()
                        .HasForeignKey("KlantId");

                    b.Navigation("Klant");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Tijdslot", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", "InGebruikDoorKok")
                        .WithMany()
                        .HasForeignKey("InGebruikDoorKokId");

                    b.Navigation("InGebruikDoorKok");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Lekkerbek.Web.Models.Identity.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Identity.Gebruiker", b =>
                {
                    b.Navigation("Beoordelingen");

                    b.Navigation("Bestellingen");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.VerlofDagenVanGebruiker", b =>
                {
                    b.Navigation("Dagen");
                });

            modelBuilder.Entity("Lekkerbek.Web.Models.Kalender.ZiekteDagenVanGebruiker", b =>
                {
                    b.Navigation("Dagen");
                });
#pragma warning restore 612, 618
        }
    }
}
