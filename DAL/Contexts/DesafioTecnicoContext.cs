namespace DAL.Contexts
{
    public class DesafioTecnicoContext : DbContext
    {
        #region TABELAS
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<TbStatus> TbStatus { get; set; }
        public DbSet<Vendas> Vendas { get; set; }
        public DbSet<VendasHistoricos> VendasHistoricos { get; set; }
        public DbSet<VendasItens> VendasItens { get; set; }
        public DbSet<Vendedores> Vendedores { get; set; }
        #endregion

        public DesafioTecnicoContext(DbContextOptions<DesafioTecnicoContext> options) : base(options) { }

        //Quando for fazer migrations deixar descomentado
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var builder = new ConfigurationBuilder()
                                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                  .AddJsonFile("appsettings.json")
                                  .AddJsonFile($"appsettings.{environmentName}.json", true)
                                  .AddEnvironmentVariables();

                var config = builder.Build();

                optionsBuilder.UseSqlServer(config.GetConnectionString("Server1"), b => b.MigrationsAssembly("Api")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");
        }
    }
}
