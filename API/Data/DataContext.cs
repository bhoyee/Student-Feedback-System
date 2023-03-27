using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Department> Departments { get; set; }

        //public DbSet<UserLike> Likes {get; set;}

        public DbSet<Petition> Petitions { get; set; }

        public DbSet<PetitiionVote> PetitiionVotes {get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PetitiionVote>()
                .HasKey(k => new {k.SourcePetitionId, k.VotedUserId});

            builder.Entity<PetitiionVote>()
                .HasOne(s => s.SourcePetition)
                .WithMany(l => l.VotedPetitionUser)
                .HasForeignKey(s => s.SourcePetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<PetitiionVote>()
            //     .HasOne(s => s.VotedUser)
            //     .WithMany(l => l.LikedByUsers)
            //     .HasForeignKey(s => s.SourcePetitionId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}