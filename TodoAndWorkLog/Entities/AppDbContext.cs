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

        public virtual DbSet<WorkLog> WorkLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.RecordTime)
                    .IsRequired();

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.ProjectId)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.RecordTime)
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

            modelBuilder.Entity<WorkLog>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.RecordTime)
                    .IsRequired();

                entity.Property(e => e.Date)
                    .IsRequired();

                entity.Property(e => e.Hours)
                    .IsRequired();

                entity.HasIndex(e => new
                {
                    e.TodoId,
                    e.Date
                }).IsUnique();

                entity.HasOne(workLog => workLog.Todo)
                    .WithMany(project => project.WorkLogs)
                    .HasForeignKey(workLog => workLog.TodoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
