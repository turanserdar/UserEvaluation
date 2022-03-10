using Microsoft.EntityFrameworkCore;
using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserEvaluation.Data.Entities;

namespace UserEvaluation.Data
{
   public class UserEvaluationDbContext:DbContext
    {

        public UserEvaluationDbContext(DbContextOptions<UserEvaluationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Evaluation>().Property(x => x.IsActive).HasDefaultValue(true);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }


    }
}
