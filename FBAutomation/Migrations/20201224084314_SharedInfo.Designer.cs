﻿// <auto-generated />
using FBAutomation.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FBAutomation.Migrations
{
    [DbContext(typeof(FBContext))]
    [Migration("20201224084314_SharedInfo")]
    partial class SharedInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("FBAutomation.Models.Assigment", b =>
                {
                    b.Property<int>("AssigmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AssigmentID");

                    b.HasIndex("GroupID");

                    b.HasIndex("UserID");

                    b.ToTable("Assigments");
                });

            modelBuilder.Entity("FBAutomation.Models.Group", b =>
                {
                    b.Property<int>("GroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("FbGroupID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupID");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FBAutomation.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("ContactInitiated")
                        .HasColumnType("bit");

                    b.Property<string>("FbID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFriend")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PageLiked")
                        .HasColumnType("bit");

                    b.Property<bool>("SharedInfo")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FBAutomation.Models.Assigment", b =>
                {
                    b.HasOne("FBAutomation.Models.Group", "Group")
                        .WithMany("Assigments")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FBAutomation.Models.User", "User")
                        .WithMany("Assigments")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FBAutomation.Models.Group", b =>
                {
                    b.Navigation("Assigments");
                });

            modelBuilder.Entity("FBAutomation.Models.User", b =>
                {
                    b.Navigation("Assigments");
                });
#pragma warning restore 612, 618
        }
    }
}
