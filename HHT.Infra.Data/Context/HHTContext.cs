using HHT.Domain.Entities;
using HHT.Infra.Data.EntityConfig;
using HHT.Infra.Data.Migrations;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace HHT.Infra.Data.Context
{
    public class HHTContext : DbContext
    {
        public HHTContext() : base("HHTConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HHTContext, Configuration>());
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Local> Locais { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ArquivoEmpresa> ArquivosEmpresa { get; set; }
        public DbSet<ArquivoContratado> ArquivosContratado { get; set; }
        public DbSet<Contratado> Contratados { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<Associado> Associados { get; set; }
        public DbSet<DocumentoGeral> DocumentosGeral { get; set; }
        public DbSet<Ponto> Pontos { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<Perfil> Acessos { get; set; }
        //Entity N:N
        public DbSet<MappedLocal> MappedLocal { get; set; }
        public DbSet<MappedServico> MappedServico { get; set; }
        public DbSet<MappedEmpresaLocal> MappedEmpresaLocal { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new EmpresaConfiguration());
            modelBuilder.Configurations.Add(new LocalConfiguration());
            modelBuilder.Configurations.Add(new ServicoConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new ArquivoEmpresaConfiguration());
            modelBuilder.Configurations.Add(new ArquivoContratadoConfiguration());
            modelBuilder.Configurations.Add(new ContratadoConfiguration());
            modelBuilder.Configurations.Add(new FuncaoConfiguration());
            modelBuilder.Configurations.Add(new TipoDocumentoConfiguration());
            modelBuilder.Configurations.Add(new AssociadoConfiguration());
            modelBuilder.Configurations.Add(new DocumentoGeralConfiguration());
            modelBuilder.Configurations.Add(new PontoConfiguration());
            modelBuilder.Configurations.Add(new HistoricoConfiguration());
            modelBuilder.Configurations.Add(new PerfilConfiguration());
            //Entity N:N
            modelBuilder.Configurations.Add(new MappedLocalConfiguration());
            modelBuilder.Configurations.Add(new MappedServicoConfiguration());
            modelBuilder.Configurations.Add(new MappedEmpresaLocalConfiguration());


            modelBuilder.Properties()
                .Where(p => p.Name == p.ReflectedType.Name + "Id")
                .Configure(p => p.IsKey());

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));


            //Mapeamento entre Empresa e Servicos mapeados
            modelBuilder.Entity<Empresa>()
                .HasMany(x => x.MappedServico)
                .WithMany(x => x.Empresas)
                .Map(x =>
                {
                    x.MapLeftKey("EmpresaId");
                    x.MapRightKey("MappedServicoId");
                    x.ToTable("Empresa_MappedServico");
                });

            //Mapeamento entre Empresa e ArquivoEmpresa
            modelBuilder.Entity<Empresa>()
                .HasMany(x => x.ArquivosEmpresa)
                .WithMany(x => x.Empresas)
                .Map(x =>
                {
                    x.MapLeftKey("EmpresaId");
                    x.MapRightKey("ArquivoEmpresaId");
                    x.ToTable("Empresa_ArquivoEmpresa");
                });

            //Mapeamento entre Contratado e ArquivoContratado
            modelBuilder.Entity<Contratado>()
                .HasMany(x => x.ArquivosContratado)
                .WithMany(x => x.Contratados)
                .Map(x =>
                {
                    x.MapLeftKey("ContratadoId");
                    x.MapRightKey("ArquivoContratadoId");
                    x.ToTable("Contratado_ArquivoContratado");
                });

            //Mapeamento entre Usuário e Locais mapeados
            modelBuilder.Entity<Usuario>()
                .HasMany(x => x.MappedLocal)
                .WithMany(x => x.Usuarios)
                .Map(x =>
                {
                    x.MapLeftKey("UsuarioId");
                    x.MapRightKey("MappedLocalId");
                    x.ToTable("Usuario_MappedLocal");
                });

            //emp
            //Mapeamento entre Usuário e Locais mapeados
            modelBuilder.Entity<Empresa>()
                .HasMany(x => x.MappedEmpresaLocal)
                .WithMany(x => x.Empresas)
                .Map(x =>
                {
                    x.MapLeftKey("EmpresaId");
                    x.MapRightKey("MappedEmpresaLocalId");
                    x.ToTable("Empresa_MappedLocal");
                });
        }


        public override int SaveChanges()
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property("DataCadastro").IsModified = false;
                    }
                }
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
