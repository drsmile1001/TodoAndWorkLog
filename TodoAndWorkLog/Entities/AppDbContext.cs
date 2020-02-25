using Microsoft.EntityFrameworkCore;

namespace TodoAndWorkLog.Entities
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Todo> Todo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.HasIndex(e => new
                {
                    e.ProjectId,
                    e.Name
                }).IsUnique();

                entity.HasOne(todo => todo.Project)
                    .WithMany(project => project.Todos)
                    .HasForeignKey(todo => todo.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
