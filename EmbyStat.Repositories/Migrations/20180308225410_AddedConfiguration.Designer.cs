﻿// <auto-generated />
using EmbyStat.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace EmbyStat.Repositories.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180308225410_AddedConfiguration")]
    partial class AddedConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("EmbyStat.Repositories.Config.Configuration", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken");

                    b.Property<string>("EmbyServerAddress");

                    b.Property<string>("EmbyUserName");

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<string>("UserId");

                    b.Property<string>("Username");

                    b.Property<bool>("WizardFinished");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });
#pragma warning restore 612, 618
        }
    }
}