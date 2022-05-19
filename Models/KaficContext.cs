using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kafic.Models
{
    public partial class KaficContext : DbContext
    {

        public KaficContext(DbContextOptions<KaficContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Konobar> Konobar { get; set; }
        public virtual DbSet<KonobarSmjena> KonobarSmjena { get; set; }
        public virtual DbSet<Kontakt> Kontakt { get; set; }
        public virtual DbSet<Korisnik> Korisnik { get; set; }
        public virtual DbSet<Pice> Pice { get; set; }
        public virtual DbSet<Racun> Racun { get; set; }
        public virtual DbSet<Skladiste> Skladiste { get; set; }
        public virtual DbSet<Smjena> Smjena { get; set; }
        public virtual DbSet<StavkaRacuna> StavkaRacuna { get; set; }
        public virtual DbSet<Vlasnik> Vlasnik { get; set; }
        public virtual DbSet<VrstaPica> VrstaPica { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Konobar>(entity =>
            {
                entity.HasKey(e => e.IdKonobar)
                    .HasName("pkKonobar");

                entity.ToTable("Konobar");

                entity.Property(e => e.IdKonobar)
                    .ValueGeneratedNever()
                    .HasColumnName("idKonobar");

                entity.Property(e => e.DatumIstekaUgovora)
                    .HasColumnType("date")
                    .HasColumnName("datumIstekaUgovora");

                entity.Property(e => e.DatumZaposlenja)
                    .HasColumnType("date")
                    .HasColumnName("datumZaposlenja");

                entity.Property(e => e.Placa)
                    .HasColumnType("money")
                    .HasColumnName("placa");

                entity.Property(e => e.Staz).HasColumnName("staz");

                entity.HasOne(d => d.IdKonobarNavigation)
                    .WithOne(p => p.Konobar)
                    .HasForeignKey<Konobar>(d => d.IdKonobar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkKonobarKorisnik");
            });

            modelBuilder.Entity<KonobarSmjena>(entity =>
            {
                entity.HasKey(e => new { e.IdKonobar, e.IdSmjena, e.Datum })
                    .HasName("pkKonobarSmjena");

                entity.ToTable("KonobarSmjena");

                entity.Property(e => e.IdKonobar).HasColumnName("idKonobar");

                entity.Property(e => e.IdSmjena).HasColumnName("idSmjena");

                entity.Property(e => e.Datum)
                    .HasColumnType("date")
                    .HasColumnName("datum");

                entity.HasOne(d => d.IdKonobarNavigation)
                    .WithMany(p => p.KonobarSmjenas)
                    .HasForeignKey(d => d.IdKonobar)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkKonobarSmjenaKonobar");

                entity.HasOne(d => d.IdSmjenaNavigation)
                    .WithMany(p => p.KonobarSmjenas)
                    .HasForeignKey(d => d.IdSmjena)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkKonobarSmjenaSmjena");
            });

            modelBuilder.Entity<Kontakt>(entity =>
            {
                entity.HasKey(e => new { e.IdKorisnik, e.Kontakt1 })
                    .HasName("pkKontakt");

                entity.ToTable("Kontakt");

                entity.Property(e => e.IdKorisnik).HasColumnName("idKorisnik");

                entity.Property(e => e.Kontakt1)
                    .HasMaxLength(50)
                    .HasColumnName("kontakt");

                entity.Property(e => e.Vrsta)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("vrsta");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.Kontakts)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkKontaktKorisnik");
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.HasKey(e => e.IdKorisnik)
                    .HasName("PK__Korisnik__80AA4063DE2EDDC7");

                entity.ToTable("Korisnik");

                entity.Property(e => e.IdKorisnik).HasColumnName("idKorisnik");

                entity.Property(e => e.Adresa)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("adresa");

                entity.Property(e => e.DatumRodenja)
                    .HasColumnType("date")
                    .HasColumnName("datumRodenja");

                entity.Property(e => e.Drzava)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("drzava");

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ime");

                entity.Property(e => e.KorisnickoIme)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("korisnickoIme");

                entity.Property(e => e.Lozinka)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("lozinka");

                entity.Property(e => e.Mjesto)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("mjesto");

                entity.Property(e => e.Oib)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("OIB");

                entity.Property(e => e.PostanskiBroj)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("postanskiBroj");

                entity.Property(e => e.Prezime)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("prezime");

                entity.Property(e => e.Spol)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("spol")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Pice>(entity =>
            {
                entity.HasKey(e => e.IdPice)
                    .HasName("PK__Pice__BB719932AACFFB27");

                entity.ToTable("Pice");

                entity.HasIndex(e => e.IdVrstaPica, "iPiceVrsta");

                entity.Property(e => e.IdPice).HasColumnName("idPice");

                entity.Property(e => e.IdVrstaPica).HasColumnName("idVrstaPica");

                entity.Property(e => e.JedCijena)
                    .HasColumnType("money")
                    .HasColumnName("jedCijena");

                entity.Property(e => e.NabavnaCijena)
                    .HasColumnType("money")
                    .HasColumnName("nabavnaCijena");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("naziv");

                entity.HasOne(d => d.IdVrstaPicaNavigation)
                    .WithMany(p => p.Pices)
                    .HasForeignKey(d => d.IdVrstaPica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkPiceVrsta");
            });

            modelBuilder.Entity<Racun>(entity =>
            {
                entity.HasKey(e => e.IdRacun)
                    .HasName("PK__Racun__8CCB5A471E75B042");

                entity.ToTable("Racun");

                entity.HasIndex(e => e.IdKorisnik, "iRacunKorisnik");

                entity.Property(e => e.IdRacun).HasColumnName("idRacun");

                entity.Property(e => e.Datum)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("datum");

                entity.Property(e => e.IdKorisnik).HasColumnName("idKorisnik");

                entity.Property(e => e.UkupanIznos).HasColumnName("ukupanIznos");

                entity.HasOne(d => d.IdKorisnikNavigation)
                    .WithMany(p => p.Racuns)
                    .HasForeignKey(d => d.IdKorisnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkRacunKorisnik");
            });

            modelBuilder.Entity<Skladiste>(entity =>
            {
                entity.HasKey(e => e.IdPice)
                    .HasName("pkSkladiste");

                entity.ToTable("Skladiste");

                entity.Property(e => e.IdPice)
                    .ValueGeneratedNever()
                    .HasColumnName("idPice");

                entity.Property(e => e.Kapacitet).HasColumnName("kapacitet");

                entity.Property(e => e.TrenutnaKolicina).HasColumnName("trenutnaKolicina");

                entity.HasOne(d => d.IdPiceNavigation)
                    .WithOne(p => p.Skladiste)
                    .HasForeignKey<Skladiste>(d => d.IdPice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkSkladistePice");
            });

            modelBuilder.Entity<Smjena>(entity =>
            {
                entity.HasKey(e => e.IdSmjena)
                    .HasName("PK__Smjena__60137224640730D5");

                entity.ToTable("Smjena");

                entity.Property(e => e.IdSmjena).HasColumnName("idSmjena");

                entity.Property(e => e.VrijemeDo).HasColumnName("vrijemeDo");

                entity.Property(e => e.VrijemeOd).HasColumnName("vrijemeOd");
            });

            modelBuilder.Entity<StavkaRacuna>(entity =>
            {
                entity.HasKey(e => new { e.IdRacun, e.IdPice })
                    .HasName("pkStavkaRacuna");

                entity.ToTable("StavkaRacuna");

                entity.Property(e => e.IdRacun).HasColumnName("idRacun");

                entity.Property(e => e.IdPice).HasColumnName("idPice");

                entity.Property(e => e.Iznos).HasColumnName("iznos");

                entity.Property(e => e.JedCijena)
                    .HasColumnType("money")
                    .HasColumnName("jedCijena");

                entity.Property(e => e.Kolicina).HasColumnName("kolicina");

                entity.HasOne(d => d.IdPiceNavigation)
                    .WithMany(p => p.StavkaRacunas)
                    .HasForeignKey(d => d.IdPice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkStavkaRacunaPice");

                entity.HasOne(d => d.IdRacunNavigation)
                    .WithMany(p => p.StavkaRacunas)
                    .HasForeignKey(d => d.IdRacun)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkStavkaRacunaRacun");
            });

            modelBuilder.Entity<Vlasnik>(entity =>
            {
                entity.HasKey(e => e.IdVlasnik)
                    .HasName("pkVlasnik");

                entity.ToTable("Vlasnik");

                entity.Property(e => e.IdVlasnik)
                    .ValueGeneratedNever()
                    .HasColumnName("idVlasnik");

                entity.Property(e => e.DatumKupnjeKafica)
                    .HasColumnType("date")
                    .HasColumnName("datumKupnjeKafica");

                entity.Property(e => e.NazivKafica)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nazivKafica");

                entity.HasOne(d => d.IdVlasnikNavigation)
                    .WithOne(p => p.Vlasnik)
                    .HasForeignKey<Vlasnik>(d => d.IdVlasnik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkVlasnikKorisnik");
            });

            modelBuilder.Entity<VrstaPica>(entity =>
            {
                entity.HasKey(e => e.IdVrstaPica)
                    .HasName("PK__VrstaPic__F2689BB039AE296B");

                entity.ToTable("VrstaPica");

                entity.Property(e => e.IdVrstaPica).HasColumnName("idVrstaPica");

                entity.Property(e => e.Vrsta)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("vrsta");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
