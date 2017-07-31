namespace HHT.Infra.Data.Migrations
{
    using Context;
    using Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HHTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HHTContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            #region Insert - Locais
            context.Locais.AddOrUpdate(
                l => l.LocalId,
                new Local
                {
                    LocalId = 1,
                    Sigla = "",
                    Nome = "",
                    DataCadastro = DateTime.Now
                }, new Local
                {
                    LocalId = 2,
                    Sigla = "AGV",
                    Nome = "Água Vermelha",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 3,
                    Sigla = "BAB",
                    Nome = "Barra Bonita",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 4,
                    Sigla = "BAR",
                    Nome = "Bariri",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 5,
                    Sigla = "BAU",
                    Nome = "Bauru",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 6,
                    Sigla = "CAC",
                    Nome = "Caconde",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 7,
                    Sigla = "EUC",
                    Nome = "Euclides da Cunha",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 8,
                    Sigla = "IBI",
                    Nome = "Ibitinga",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 9,
                    Sigla = "LMO",
                    Nome = "Limoeiro",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 10,
                    Sigla = "MOG",
                    Nome = "Mogi Guaçu",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 11,
                    Sigla = "NAV",
                    Nome = "Nova Avanhandava",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 12,
                    Sigla = "PRO",
                    Nome = "Promissão",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 13,
                    Sigla = "SJQ",
                    Nome = "São Joaquim",
                    DataCadastro = DateTime.Now
                },
                new Local
                {
                    LocalId = 14,
                    Sigla = "SJS",
                    Nome = "São José",
                    DataCadastro = DateTime.Now

                }
            );
            context.SaveChanges();
            #endregion

            #region Insert - Tipos de Serviços
            //context.Servicos.AddOrUpdate(
            //    s => s.ServicoId,
            //    new Servico
            //    {
            //        ServicoId = 1,
            //        Nome = "Munk, Guindaste e Guindauto",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Servico
            //    {
            //        ServicoId = 2,
            //        Nome = "Montador de Adaime",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Servico
            //    {
            //        ServicoId = 3,
            //        Nome = "NR 10 SEP",
            //        DataCadastro = DateTime.Now
            //    }
            //);
            #endregion

            #region Insert - Funções
            //context.Funcoes.AddOrUpdate(
            //    f => f.FuncaoId,
            //    new Funcao
            //    {
            //        FuncaoId = 1,
            //        Nome = "Engenheiro Elétrico",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Funcao
            //    {
            //        FuncaoId = 2,
            //        Nome = "Engenheiro Mecânico",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Funcao
            //    {
            //        FuncaoId = 3,
            //        Nome = "Engenheiro de Produção",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Funcao
            //    {
            //        FuncaoId = 4,
            //        Nome = "Técnico de Segunraça do Trabalho",
            //        DataCadastro = DateTime.Now
            //    },
            //    new Funcao
            //    {
            //        FuncaoId = 5,
            //        Nome = "Técnico Eletricista",
            //        DataCadastro = DateTime.Now
            //    }
            //);
            //context.SaveChanges();
            #endregion

            #region Insert - TipoDocumento
            context.TiposDocumento.AddOrUpdate(
                t => t.TipoDocumentoId,
                new TipoDocumento
                {
                    TipoDocumentoId = 1,
                    Nome = "Documento",
                    DataCadastro = DateTime.Now
                },
                new TipoDocumento
                {
                    TipoDocumentoId = 2,
                    Nome = "Integração",
                    DataCadastro = DateTime.Now
                },
                new TipoDocumento
                {
                    TipoDocumentoId = 3,
                    Nome = "Treinamento",
                    DataCadastro = DateTime.Now
                }
            );
            context.SaveChanges();
            #endregion

            #region Insert - Associado
            context.Associados.AddOrUpdate(
                a => a.AssociadoId,
                new Associado
                {
                    AssociadoId = 1,
                    Nome = "Empresa",
                    DataCadastro = DateTime.Now
                },
                new Associado
                {
                    AssociadoId = 2,
                    Nome = "Contratado",
                    DataCadastro = DateTime.Now
                }
            );
            context.SaveChanges();
            #endregion

            #region Insert - Documentos Gerais
            //context.DocumentosGeral.AddOrUpdate(
            //    d => d.DocumentoGeralId,
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 1,
            //        Nome = "RRF",
            //        Descricao = "Registro na Receita Federal",
            //        Vencimento = 12,
            //        TipoDocumentoId = 1,
            //        LocalId = 1,
            //        AssociadoId = 1,
            //        DataCadastro = DateTime.Now
            //    },
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 2,
            //        Nome = "CH",
            //        Descricao = "Certidão de Habite-se",
            //        Vencimento = 6,
            //        TipoDocumentoId = 2,
            //        LocalId = 2,
            //        AssociadoId = 2,
            //        DataCadastro = DateTime.Now
            //    },
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 3,
            //        Nome = "RJC",
            //        Descricao = "Registro na Junta Comercial",
            //        Vencimento = 24,
            //        TipoDocumentoId = 3,
            //        LocalId = 3,
            //        AssociadoId = 2,
            //        DataCadastro = DateTime.Now
            //    }
            //);
            //context.SaveChanges();
            #endregion

            #region Insert - Treinamentos
            //context.DocumentosGeral.AddOrUpdate(
            //    d => d.DocumentoGeralId,
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 1,
            //        Nome = "EPI/EPC",
            //        Descricao = "EPI/EPC",
            //        Vencimento = 12,
            //        TipoDocumentoId = 3,
            //        LocalId = 1,
            //        AssociadoId = 1,
            //        DataCadastro = DateTime.Now
            //    },
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 2,
            //        Nome = "CH",
            //        Descricao = "Certidão de Habite-se",
            //        Vencimento = 6,
            //        TipoDocumentoId = 3,
            //        LocalId = 2,
            //        AssociadoId = 2,
            //        DataCadastro = DateTime.Now
            //    },
            //    new DocumentoGeral
            //    {
            //        DocumentoGeralId = 3,
            //        Nome = "RJC",
            //        Descricao = "Registro na Junta Comercial",
            //        Vencimento = 24,
            //        TipoDocumentoId = 3,
            //        LocalId = 3,
            //        AssociadoId = 2,
            //        DataCadastro = DateTime.Now
            //    }
            //);
            //context.SaveChanges();
            #endregion

            #region Insert - Acesso
            context.Acessos.AddOrUpdate(
                a => a.PerfilId,
                new Perfil
                {
                    PerfilId = 1,
                    Nome = "Porteiro",
                    DataCadastro = DateTime.Now
                },
                new Perfil
                {
                    PerfilId = 2,
                    Nome = "Empresa",
                    DataCadastro = DateTime.Now
                },
                new Perfil
                {
                    PerfilId = 3,
                    Nome = "Seguranca",
                    DataCadastro = DateTime.Now
                },
                new Perfil
                {
                    PerfilId = 4,
                    Nome = "Funcional",
                    DataCadastro = DateTime.Now
                },
                new Perfil
                {
                    PerfilId = 5,
                    Nome = "TI",
                    DataCadastro = DateTime.Now
                },
                new Perfil
                {
                    PerfilId = 6,
                    Nome = "Gerente",
                    DataCadastro = DateTime.Now
                }
            );
            context.SaveChanges();
            #endregion
        }
    }
}