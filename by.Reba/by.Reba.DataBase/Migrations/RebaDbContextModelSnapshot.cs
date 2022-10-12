﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using by.Reba.DataBase;

#nullable disable

namespace by.Reba.DataBase.Migrations
{
    [DbContext(typeof(RebaDbContext))]
    partial class RebaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Assessment")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PosterUrl")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RatingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("RatingId");

                    b.HasIndex("SourceId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Assessment")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentCommentId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CommentId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_PositivityRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PositivityRatings");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Source", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_UserPreference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PositivityRatingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PositivityRatingId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("T_ArticleT_User", b =>
                {
                    b.Property<Guid>("BookmarksId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserBookmarksId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BookmarksId", "UserBookmarksId");

                    b.HasIndex("UserBookmarksId");

                    b.ToTable("Bookmarks", (string)null);
                });

            modelBuilder.Entity("T_ArticleT_User1", b =>
                {
                    b.Property<Guid>("HistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserHistoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("HistoryId", "UserHistoryId");

                    b.HasIndex("UserHistoryId");

                    b.ToTable("History", (string)null);
                });

            modelBuilder.Entity("T_CategoryT_UserPreference", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserPreferencesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "UserPreferencesId");

                    b.HasIndex("UserPreferencesId");

                    b.ToTable("UsersPreferences", (string)null);
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Article", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_PositivityRating", "Rating")
                        .WithMany("Articles")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_Source", "Source")
                        .WithMany("Articles")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Rating");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Comment", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_Comment", "ParentComment")
                        .WithMany("InnerComments")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("ParentComment");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Notification", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Comment", "Comment")
                        .WithOne("T_Notification")
                        .HasForeignKey("by.Reba.DataBase.Entities.T_Notification", "CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_User", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_UserPreference", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_PositivityRating", "MinPositivityRating")
                        .WithOne("UserPreference")
                        .HasForeignKey("by.Reba.DataBase.Entities.T_UserPreference", "PositivityRatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_User", "User")
                        .WithOne("Preference")
                        .HasForeignKey("by.Reba.DataBase.Entities.T_UserPreference", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MinPositivityRating");

                    b.Navigation("User");
                });

            modelBuilder.Entity("T_ArticleT_User", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Article", null)
                        .WithMany()
                        .HasForeignKey("BookmarksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_User", null)
                        .WithMany()
                        .HasForeignKey("UserBookmarksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("T_ArticleT_User1", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Article", null)
                        .WithMany()
                        .HasForeignKey("HistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_User", null)
                        .WithMany()
                        .HasForeignKey("UserHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("T_CategoryT_UserPreference", b =>
                {
                    b.HasOne("by.Reba.DataBase.Entities.T_Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("by.Reba.DataBase.Entities.T_UserPreference", null)
                        .WithMany()
                        .HasForeignKey("UserPreferencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Category", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Comment", b =>
                {
                    b.Navigation("InnerComments");

                    b.Navigation("T_Notification")
                        .IsRequired();
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_PositivityRating", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("UserPreference")
                        .IsRequired();
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_Source", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("by.Reba.DataBase.Entities.T_User", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Preference")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
