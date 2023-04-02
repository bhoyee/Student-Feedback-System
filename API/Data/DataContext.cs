using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        //public DbSet<AppUser> Users { get; set; }
        public DbSet<Department> Departments { get; set; }

        //public DbSet<UserLike> Likes {get; set;}

        public DbSet<Petition> Petitions { get; set; }

        public DbSet<Vote> Votes {get; set;}

        public DbSet<PetitionReply> PetitionReplies { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

                 builder.Entity<AppUser>()
                    .HasMany(d => d.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                
                builder.Entity<AppRole>()
                    .HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.Role)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Department>()
                    .HasMany(d => d.Petitions)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Department>()
                    .HasMany(d => d.Users)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Petition>()
                    .HasOne(p => p.AppUser)
                    .WithMany(u => u.Petitions)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Petition>()
                    .HasOne(p => p.Department)
                    .WithMany(d => d.Petitions)
                    .HasForeignKey(p => p.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Petition>()
                    .HasMany(p => p.Votes)
                    .WithOne(v => v.Petition)
                    .HasForeignKey(v => v.PetitionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                builder.Entity<PetitionReply>()
                    .HasOne<Petition>(p => p.Petition)
                    .WithMany(pr => pr.PetitionReply)
                    .HasForeignKey(p => p.PetitionId)
                    .OnDelete(DeleteBehavior.Cascade);
                

                builder.Entity<Vote>()
                    .HasOne<AppUser>(u => u.User)
                    .WithMany(v => v.Votes)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                

             

            //   builder.Entity<Vote>()
            //     .HasKey(k => new {k.UserId, k.PetitionId});

            // builder.Entity<PetitiionVote>()
            //     .HasKey(k => new {k.SourcePetitionId, k.VotedUserId});

            // builder.Entity<PetitiionVote>()
            //     .HasOne(s => s.SourcePetition)
            //    // .WithMany(l => l.Votes)
            //     .HasForeignKey(s => s.SourcePetitionId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<Vote>()
            //     .HasOne(s => s.User)
            //     .WithMany(l => l.Votes)
            //     .HasForeignKey(s => s.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);


            // builder.Entity<PetitiionVote>()
            //     .HasOne(s => s.VotedUser)
            //     .WithMany(l => l.LikedByUsers)
            //     .HasForeignKey(s => s.SourcePetitionId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}