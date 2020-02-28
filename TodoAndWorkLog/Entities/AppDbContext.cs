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

        public virtual DbSet<WorkItem> WorkItem { get; set; }

        public virtual DbSet<WorkLog> WorkLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .IsRequired();

                entity.Property(e => e.RecordTime)
                    .IsRequired();

                entity.HasIndex(e => new
                {
                    e.ParentId,
                    e.Name
                }).IsUnique();

                entity.HasOne(workItem=>workItem.Parent)
                    .WithMany(workItem => workItem.Children)
                    .HasForeignKey(todo => todo.ParentId)
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
                    e.WorkItemId,
                    e.Date
                }).IsUnique();

                entity.HasOne(workLog => workLog.WorkItem)
                    .WithMany(project => project.WorkLogs)
                    .HasForeignKey(workLog => workLog.WorkItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
