﻿// <auto-generated />
using System;
using FreeWheel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FreeWheel.Migrations
{
    [DbContext(typeof(FreeWheelContext))]
    [Migration("20190628133908_mg1")]
    partial class mg1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FreeWheel.Models.Genres", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("FreeWheel.Models.Movies", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("runningTime")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .HasColumnType("varchar(max)");

                    b.Property<int>("yearOfRelease")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("FreeWheel.Models.MoviesGenres", b =>
                {
                    b.Property<Guid>("GenresId");

                    b.Property<Guid>("MoviesId");

                    b.HasKey("GenresId", "MoviesId");

                    b.HasIndex("MoviesId");

                    b.ToTable("MoviesGenres");
                });

            modelBuilder.Entity("FreeWheel.Models.Ratings", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("moviesid");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.Property<Guid?>("usersid");

                    b.HasKey("id");

                    b.HasIndex("moviesid");

                    b.HasIndex("usersid");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("FreeWheel.Models.Users", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FreeWheel.Models.MoviesGenres", b =>
                {
                    b.HasOne("FreeWheel.Models.Genres", "Genres")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FreeWheel.Models.Movies", "Movies")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FreeWheel.Models.Ratings", b =>
                {
                    b.HasOne("FreeWheel.Models.Movies", "movies")
                        .WithMany("ratings")
                        .HasForeignKey("moviesid");

                    b.HasOne("FreeWheel.Models.Users", "users")
                        .WithMany()
                        .HasForeignKey("usersid");
                });
#pragma warning restore 612, 618
        }
    }
}
