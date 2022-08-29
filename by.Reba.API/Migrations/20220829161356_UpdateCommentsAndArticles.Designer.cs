﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using by.Reba.API;

#nullable disable

namespace by.Reba.API.Migrations
{
    [DbContext(typeof(RebaDbContext))]
    [Migration("20220829161356_UpdateCommentsAndArticles")]
    partial class UpdateCommentsAndArticles
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("by.Reba.API.Entities.T_Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int?>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("OriginUrl")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("RatingId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int?>("BaseCommentId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Likes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("BaseCommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_PositivityRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PositivityRating");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("T_ArticleT_User", b =>
                {
                    b.Property<int>("BookmarksId")
                        .HasColumnType("int");

                    b.Property<int>("UserBookmarksId")
                        .HasColumnType("int");

                    b.HasKey("BookmarksId", "UserBookmarksId");

                    b.HasIndex("UserBookmarksId");

                    b.ToTable("Bookmarks", (string)null);
                });

            modelBuilder.Entity("T_ArticleT_User1", b =>
                {
                    b.Property<int>("HistoryId")
                        .HasColumnType("int");

                    b.Property<int>("UserHistoryId")
                        .HasColumnType("int");

                    b.HasKey("HistoryId", "UserHistoryId");

                    b.HasIndex("UserHistoryId");

                    b.ToTable("History", (string)null);
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Article", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.API.Entities.T_PositivityRating", "Rating")
                        .WithMany("Articles")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Comment", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.API.Entities.T_Comment", "BaseComment")
                        .WithMany("InnerComments")
                        .HasForeignKey("BaseCommentId");

                    b.Navigation("Article");

                    b.Navigation("BaseComment");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Notification", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Comment", "Comment")
                        .WithOne("T_Notification")
                        .HasForeignKey("by.Reba.API.Entities.T_Notification", "CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.API.Entities.T_User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_User", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("T_ArticleT_User", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Article", null)
                        .WithMany()
                        .HasForeignKey("BookmarksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.API.Entities.T_User", null)
                        .WithMany()
                        .HasForeignKey("UserBookmarksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("T_ArticleT_User1", b =>
                {
                    b.HasOne("by.Reba.API.Entities.T_Article", null)
                        .WithMany()
                        .HasForeignKey("HistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.API.Entities.T_User", null)
                        .WithMany()
                        .HasForeignKey("UserHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Category", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Comment", b =>
                {
                    b.Navigation("InnerComments");

                    b.Navigation("T_Notification")
                        .IsRequired();
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_PositivityRating", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("by.Reba.API.Entities.T_User", b =>
                {
                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
