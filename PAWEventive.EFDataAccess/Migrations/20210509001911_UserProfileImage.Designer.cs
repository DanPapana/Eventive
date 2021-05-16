﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PAWEventive.EFDataAccess;

namespace PAWEventive.EFDataAccess.Migrations
{
    [DbContext(typeof(EventManagerDbContext))]
    [Migration("20210509001911_UserProfileImage")]
    partial class UserProfileImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CommenterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CommenterId");

                    b.HasIndex("EventId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.ContactDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkToSocialM")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContactDetails");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EventDetailsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageByteArray")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventDetailsId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.EventDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaximumParticipantNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("OccurenceDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ParticipationFee")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("EventDetails");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.Participation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserParticipationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Participations");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ContactDetailsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailsId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.Comment", b =>
                {
                    b.HasOne("PAWEventive.ApplicationLogic.DataModel.User", "Commenter")
                        .WithMany()
                        .HasForeignKey("CommenterId");

                    b.HasOne("PAWEventive.ApplicationLogic.DataModel.Event", null)
                        .WithMany("Comments")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.Event", b =>
                {
                    b.HasOne("PAWEventive.ApplicationLogic.DataModel.EventDetails", "EventDetails")
                        .WithMany()
                        .HasForeignKey("EventDetailsId");
                });

            modelBuilder.Entity("PAWEventive.ApplicationLogic.DataModel.User", b =>
                {
                    b.HasOne("PAWEventive.ApplicationLogic.DataModel.ContactDetails", "ContactDetails")
                        .WithMany()
                        .HasForeignKey("ContactDetailsId");
                });
#pragma warning restore 612, 618
        }
    }
}
