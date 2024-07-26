using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Phat_Blogger_Website.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<MainComment> MainComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<SubComment>().HasOne(mc => mc.MainComment)
        //        .WithMany(sc => sc.SubComments)
        //        .HasForeignKey(sc => sc.MainCommentId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<SubComment>().HasOne(c => c.User)
        //        .WithMany(sc => sc.SubComments)
        //        .HasForeignKey(sc => sc.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<MainComment>().HasOne(c => c.User)
        //        .WithMany(sc => sc.MainComments)
        //        .HasForeignKey(sc => sc.UserId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}
    }
}
