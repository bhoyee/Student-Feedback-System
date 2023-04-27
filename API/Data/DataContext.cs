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

        public DbSet<Feedback> Feedbacks {get; set;}
        public DbSet<FeedbackReply> FeedbackReplies {get; set;}

        public DbSet<FeedbackRecipient> FeedbackRecipients { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

                builder.Entity<Feedback>()
                    .Property(f => f.Status)
                    .HasConversion<int>();

                 builder.Entity<AppUser>()
                    .HasMany(d => d.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<AppUser>()
                    .HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId);
                
                builder.Entity<AppUser>()
                    .HasMany(u => u.Feedbacks)
                    .WithOne(d => d.Sender)
                    .HasForeignKey(u => u.SenderId);

                
                builder.Entity<AppRole>()
                    .HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.Role)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<Department>()
                    .HasMany(d => d.Petitions)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

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
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Petition>()
                    .HasMany(p => p.Votes)
                    .WithOne(v => v.Petition)
                    .HasForeignKey(v => v.PetitionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                builder.Entity<PetitionReply>()
                    .HasOne(pr => pr.Petition)
                    .WithMany()
                    .HasForeignKey(pr => pr.PetitionId)
                    .OnDelete(DeleteBehavior.Restrict);

                

                builder.Entity<Vote>()
                    .HasOne<AppUser>(u => u.User)
                    .WithMany(v => v.Votes)
                    .HasForeignKey(p => p.UserId);

                builder.Entity<Vote>()
                    .HasOne(v => v.Petition)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(v => v.PetitionId)
                    .OnDelete(DeleteBehavior.NoAction);

                    
                
                builder.Entity<Feedback>()
                    .HasOne(f => f.Sender)
                    .WithMany()
                    .HasForeignKey(f => f.SenderId);

                builder.Entity<Feedback>()
                    .HasOne(f => f.AssignedTo)
                    .WithMany()
                    .HasForeignKey(f => f.AssignedToId);

                builder.Entity<Feedback>()
                    .HasOne(f => f.Department)
                    .WithMany(d => d.Feedbacks)
                    .HasForeignKey(f => f.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Feedback>()
                    .HasMany(f => f.Replies)
                    .WithOne(r => r.Feedback)
                    .HasForeignKey(r => r.FeedbackId);

                builder.Entity<FeedbackReply>()
                    .HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId);

                builder.Entity<FeedbackReply>()
                    .HasOne(fr => fr.Feedback)
                    .WithMany(f => f.Replies)
                    .HasForeignKey(fr => fr.FeedbackId)
                    .OnDelete(DeleteBehavior.NoAction);


                builder.Entity<Department>()
                    .HasMany(d => d.Feedbacks)
                    .WithOne(f => f.Department)
                    .HasForeignKey(f => f.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Feedback>()
                    .HasKey(f => f.Id);

                builder.Entity<FeedbackRecipient>()
                    .HasKey(fr => new { fr.FeedbackId, fr.RecipientId });

                builder.Entity<FeedbackRecipient>()
                    .HasOne(fr => fr.Feedback)
                    .WithMany(f => f.Recipients)
                    .HasForeignKey(fr => fr.FeedbackId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<FeedbackRecipient>()
                    .HasOne(fr => fr.Recipient)
                    .WithMany(r => r.FeedbackRecipients)
                    .HasForeignKey(fr => fr.RecipientId)
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