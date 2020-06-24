﻿using Iwentys.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Context
{
    public class IwentysDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<GuildProfile> GuildProfiles { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }

        public IwentysDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildProfile>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<GuildMember>().HasKey(g => new { g.GuildId, g.MemberId});
            modelBuilder.Entity<GuildMember>().HasIndex(g => g.MemberId).IsUnique();
        }
    }
}