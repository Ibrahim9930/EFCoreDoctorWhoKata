﻿// <auto-generated />
using System;
using DoctorWho.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoctorWho.Db.Migrations
{
    [DbContext(typeof(DoctorWhoCoreDbContext))]
    [Migration("20210804153918_join_entity_naming_fixes")]
    partial class join_entity_naming_fixes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CompanionEpisode", b =>
                {
                    b.Property<Guid>("EpisodeCompanionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanionsCompanionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CompanionId");

                    b.Property<Guid>("EpisodesEpisodeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EpisodeId");

                    b.HasKey("EpisodeCompanionId");

                    b.HasIndex("CompanionsCompanionId");

                    b.HasIndex("EpisodesEpisodeId");

                    b.ToTable("EpisodeCompanion");
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Author", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthorName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Companion", b =>
                {
                    b.Property<Guid>("CompanionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WhoPlayed")
                        .HasColumnType("int");

                    b.HasKey("CompanionId");

                    b.ToTable("Companions");
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Doctor", b =>
                {
                    b.Property<Guid>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DoctorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DoctorNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("FirstEpisodeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEpisodeDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DoctorId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Enemy", b =>
                {
                    b.Property<Guid>("EnemyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnemyName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EnemyId");

                    b.ToTable("Enemies");
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Episode", b =>
                {
                    b.Property<Guid>("EpisodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EpisodeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("int");

                    b.Property<string>("EpisodeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeriesNumber")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EpisodeId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("EnemyEpisode", b =>
                {
                    b.Property<Guid>("EpisodeEnemyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EnemiesEnemyId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EnemyId");

                    b.Property<Guid>("EpisodesEpisodeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EpisodeId");

                    b.HasKey("EpisodeEnemyId");

                    b.HasIndex("EnemiesEnemyId");

                    b.HasIndex("EpisodesEpisodeId");

                    b.ToTable("EpisodeEnemy");
                });

            modelBuilder.Entity("CompanionEpisode", b =>
                {
                    b.HasOne("DoctorWho.Db.Domain.Companion", null)
                        .WithMany()
                        .HasForeignKey("CompanionsCompanionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorWho.Db.Domain.Episode", null)
                        .WithMany()
                        .HasForeignKey("EpisodesEpisodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Episode", b =>
                {
                    b.HasOne("DoctorWho.Db.Domain.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorWho.Db.Domain.Doctor", "Doctor")
                        .WithMany("Episodes")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("EnemyEpisode", b =>
                {
                    b.HasOne("DoctorWho.Db.Domain.Enemy", null)
                        .WithMany()
                        .HasForeignKey("EnemiesEnemyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorWho.Db.Domain.Episode", null)
                        .WithMany()
                        .HasForeignKey("EpisodesEpisodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DoctorWho.Db.Domain.Doctor", b =>
                {
                    b.Navigation("Episodes");
                });
#pragma warning restore 612, 618
        }
    }
}
