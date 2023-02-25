using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API;

public partial class PlaceBookingContext : DbContext
{
    public PlaceBookingContext()
    {
    }

    public PlaceBookingContext(DbContextOptions<PlaceBookingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=placeBooking;Username=postgres;Password=1379");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("user_pkey");

            entity.ToTable("account");

            entity.Property(e => e.IdAccount)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_account");
            entity.Property(e => e.DateOfBirthday).HasColumnName("date_of_birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(70)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("account_id_image_fkey");
        });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.IdActor).HasName("actor_pkey");

            entity.ToTable("actor");

            entity.Property(e => e.IdActor)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_actor");
            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.Actors)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("actor_id_image_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("booking_pkey");

            entity.ToTable("booking");

            entity.Property(e => e.IdBooking)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_booking");
            entity.Property(e => e.BookingCode).HasColumnName("booking_code")
                .HasMaxLength(30);
                
            entity.Property(e => e.DateTime).HasColumnName("date_time")
                .HasColumnType("timestamp without time zone"); 
            entity.Property(e => e.IdAccount).HasColumnName("id_account");
            entity.Property(e => e.IdPlace).HasColumnName("id_place");
            entity.Property(e => e.IdSession).HasColumnName("id_session");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("booking_id_account_fkey");

            entity.HasOne(d => d.IdPlaceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdPlace)
                .HasConstraintName("booking_id_place_fkey");

            entity.HasOne(d => d.IdSessionNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdSession)
                .HasConstraintName("booking_id_session_fkey");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.IdCinema).HasName("Cinema_pkey");

            entity.ToTable("cinema");

            entity.Property(e => e.IdCinema)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_cinema");
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .HasColumnName("address");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("city_name");
            entity.Property(e => e.Name)
                .HasMaxLength(44)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.IdFilm).HasName("film_pkey");

            entity.ToTable("film");

            entity.Property(e => e.IdFilm)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_film");
            entity.Property(e => e.AgeRating).HasColumnName("age_rating");
            entity.Property(e => e.Description)
                .HasMaxLength(1600)
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.Name)
                .HasMaxLength(113)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(e => e.IdHall).HasName("hall_pkey");

            entity.ToTable("hall");

            entity.Property(e => e.IdHall)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_hall");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.IdCinema).HasColumnName("id_cinema");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.IdCinemaNavigation).WithMany(p => p.Halls)
                .HasForeignKey(d => d.IdCinema)
                .HasConstraintName("hall_id_cinema_fkey");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.IdImage)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_image");
            entity.Property(e => e.IdEntity).HasColumnName("id_entity");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Url)
                .HasMaxLength(5000)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.IdPlace).HasName("place_pkey");

            entity.ToTable("place");

            entity.Property(e => e.IdPlace)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_place");
            entity.Property(e => e.IdHall).HasColumnName("id_hall");
            entity.Property(e => e.SeatNumber).HasColumnName("seat_number");
            entity.Property(e => e.Row).HasColumnName("row");

            entity.HasOne(d => d.IdHallNavigation).WithMany(p => p.Places)
                .HasForeignKey(d => d.IdHall)
                .HasConstraintName("place_id_hall_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.IdRole)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_role");
            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.IdFilm).HasColumnName("id_film");
            entity.Property(e => e.NamePersonage)
                .HasMaxLength(80)
                .HasColumnName("name_personage");

            entity.HasOne(d => d.IdActorNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.IdActor)
                .HasConstraintName("role_id_actor_fkey");

            entity.HasOne(d => d.IdFilmNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.IdFilm)
                .HasConstraintName("role_id_film_fkey");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.IdSession).HasName("session_pkey");

            entity.ToTable("session");

            entity.Property(e => e.IdSession)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_session");
            entity.Property(e => e.DateTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_time");
            entity.Property(e => e.IdFilm).HasColumnName("id_film");
            entity.Property(e => e.IdHall).HasColumnName("id_hall");

            entity.HasOne(d => d.IdFilmNavigation).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.IdFilm)
                .HasConstraintName("session_id_film_fkey");

            entity.HasOne(d => d.IdHallNavigation).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.IdHall)
                .HasConstraintName("session_id_hall_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}