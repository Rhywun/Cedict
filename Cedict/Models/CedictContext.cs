using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cedict.Models
{
    public class CedictContext : DbContext
    {
        public CedictContext(DbContextOptions<CedictContext> options) : base(options)
        {
        }

        public virtual DbSet<Entry> Entries { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level)
                                      => category == DbLoggerCategory
                                                    .Database.Command.Name &&
                                         level == LogLevel.Information)
                       .AddConsole();
            });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Comment me to disable logging
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("entries");
                entity.HasIndex(e => new { e.Traditional, e.Simplified }).HasName("entries_index");
                entity.Property(e => e.English).HasColumnName("english");
                entity.Property(e => e.Pinyin).HasColumnName("pinyin");
                entity.Property(e => e.Simplified).HasColumnName("simplified");
                entity.Property(e => e.Traditional).HasColumnName("traditional");
            });

            // OnModelCreatingPartial(modelBuilder);
        }
    }
}
