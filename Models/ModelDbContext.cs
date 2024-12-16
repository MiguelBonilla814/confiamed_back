using Microsoft.EntityFrameworkCore;

namespace API_CONFIAMED_MB.Models
{
	public class ModelDbContext: DbContext
	{
		public DbSet<Item> Items { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }

		public DbSet<Seguimiento> Seguimientos { get; set; }

		public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Usuario>().ToTable("mt_usuarios");

			modelBuilder.Entity<Item>().ToTable("mt_items_trabajo");

			modelBuilder.Entity<Item>().ToTable("mt_seguimiento_items");

			base.OnModelCreating(modelBuilder);
		}
	}
}
