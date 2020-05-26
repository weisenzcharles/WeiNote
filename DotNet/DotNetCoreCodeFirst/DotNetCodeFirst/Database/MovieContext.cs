using Microsoft.EntityFrameworkCore;
using DotNetCodeFirst.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace DotNetCodeFirst.Database
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./Movies.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ////查找所有 FluentAPI 配置
            //var typesRegister = Assembly.GetExecutingAssembly().GetTypes().Where(q => q.GetInterface(typeof(IEntityTypeConfiguration<>).FullName) != null);

            ////应用 FluentAPI
            //foreach (var type in typesRegister)
            //{
            //    //dynamic 使 C#具有弱语言的特性，在编译时不对类型进行检查
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.ApplyConfiguration(configurationInstance);
            //}

            // 配置电影和电影截图一对多关系
            modelBuilder.Entity<Screenshot>().HasOne(s => s.Movie).WithMany(m => m.Screenshots).HasForeignKey(s => s.MovieId);


            // 配置电影和演员多对多关系联合主键    
            modelBuilder.Entity<MovieActor>().HasKey(m => new { m.ActorId, m.MovieId });

            // 配置演员和电影的一对多关系
            modelBuilder.Entity<MovieActor>().HasOne(t => t.Actor).WithMany(p => p.ActorMovies).HasForeignKey(t => t.MovieId).OnDelete(DeleteBehavior.SetNull);

            // 配置电影和演员的一对多关系
            modelBuilder.Entity<MovieActor>().HasOne(t => t.Movie).WithMany(p => p.MovieActors).HasForeignKey(t => t.ActorId).OnDelete(DeleteBehavior.SetNull);


            // 配置电影和分类多对多关系联合主键    
            modelBuilder.Entity<MovieCategory>().HasKey(m => new { m.CategoryId, m.MovieId });

            // 配置分类和电影的一对多关系
            modelBuilder.Entity<MovieCategory>().HasOne(t => t.Category).WithMany(p => p.CategoryMovies).HasForeignKey(t => t.MovieId).OnDelete(DeleteBehavior.SetNull);

            // 配置电影和分类的一对多关系
            modelBuilder.Entity<MovieCategory>().HasOne(t => t.Movie).WithMany(p => p.MovieCategories).HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
