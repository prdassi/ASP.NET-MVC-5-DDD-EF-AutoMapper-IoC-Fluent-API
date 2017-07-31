[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HHT.UI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(HHT.UI.App_Start.NinjectWebCommon), "Stop")]

namespace HHT.UI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Application;
    using Domain.Interfaces.Repositories;
    using Domain.Services;
    using Application.Interface;
    using Domain.Interfaces.Services;
    using Infra.Data.Repositories;
    //using static WebApiApplication; erro no ninject pq mudou o global
    using Ninject.Web.Mvc;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using WebApiContrib.IoC.Ninject;
    using Autenticacao;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        //private static IKernel CreateKernel()
        //{
        //    var kernel = new StandardKernel();
        //    try
        //    {
        //        kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
        //        kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

        //        RegisterServices(kernel);
        //        return kernel;
        //    }
        //    catch
        //    {
        //        kernel.Dispose();
        //        throw;
        //    }
        //}


        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                //Note: Add the line below:
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>();
            kernel.Bind<UserManager<ApplicationUser>>().ToSelf();

            kernel.Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();

            kernel.Bind(typeof(IAppServiceBase<>)).To(typeof(AppServiceBase<>));
            kernel.Bind<IEmpresaAppService>().To<EmpresaAppService>();
            kernel.Bind<ILocalAppService>().To<LocalAppService>();
            kernel.Bind<IServicoAppService>().To<ServicoAppService>();
            kernel.Bind<IArquivoEmpresaAppService>().To<ArquivoEmpresaAppService>();
            kernel.Bind<IArquivoContratadoAppService>().To<ArquivoContratadoAppService>();
            kernel.Bind<IUsuarioAppService>().To<UsuarioAppService>();
            kernel.Bind<IContratadoAppService>().To<ContratadoAppService>();
            kernel.Bind<IFuncaoAppService>().To<FuncaoAppService>();
            kernel.Bind<ITipoDocumentoAppService>().To<TipoDocumentoAppService>();
            kernel.Bind<IAssociadoAppService>().To<AssociadoAppService>();
            kernel.Bind<IDocumentoGeralAppService>().To<DocumentoGeralAppService>();
            kernel.Bind<IPontoAppService>().To<PontoAppService>();
            kernel.Bind<IAjustePontoAppService>().To<AjustePontoAppService>();
            kernel.Bind<IHistoricoAppService>().To<HistoricoAppService>();
            kernel.Bind<IConsolidacaoAppService>().To<ConsolidacaoAppService>();
            kernel.Bind<IPerfilAppService>().To<PerfilAppService>();

            kernel.Bind<IMappedEmpresaLocalAppService>().To<MappedEmpresaLocalAppService>();
            kernel.Bind<IMappedLocalAppService>().To<MappedLocalAppService>();
            kernel.Bind<IMappedServicoAppService>().To<MappedServicoAppService>();

            kernel.Bind(typeof(IServiceBase<>)).To(typeof(ServiceBase<>));
            kernel.Bind<IEmpresaService>().To<EmpresaService>();
            kernel.Bind<ILocalService>().To<LocalService>();
            kernel.Bind<IServicoService>().To<ServicoService>();
            kernel.Bind<IArquivoEmpresaService>().To<ArquivoEmpresaService>();
            kernel.Bind<IArquivoContratadoService>().To<ArquivoContratadoService>();
            kernel.Bind<IUsuarioService>().To<UsuarioService>();
            kernel.Bind<IContratadoService>().To<ContratadoService>();
            kernel.Bind<IFuncaoService>().To<FuncaoService>();
            kernel.Bind<ITipoDocumentoService>().To<TipoDocumentoService>();
            kernel.Bind<IAssociadoService>().To<AssociadoService>();
            kernel.Bind<IDocumentoGeralService>().To<DocumentoGeralService>();
            kernel.Bind<IPontoService>().To<PontoService>();
            kernel.Bind<IAjustePontoService>().To<AjustePontoService>();
            kernel.Bind<IHistoricoService>().To<HistoricoService>();
            kernel.Bind<IConsolidacaoService>().To<ConsolidacaoService>();
            kernel.Bind<IPerfilService>().To<PerfilService>();

            kernel.Bind<IMappedEmpresaLocalService>().To<MappedEmpresaLocalService>();
            kernel.Bind<IMappedLocalService>().To<MappedLocalService>();
            kernel.Bind<IMappedServicoService>().To<MappedServicoService>();

            kernel.Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            kernel.Bind<IEmpresaRepository>().To<EmpresaRepository>();
            kernel.Bind<ILocalRepository>().To<LocalRepository>();
            kernel.Bind<IServicoRepository>().To<ServicoRepository>();
            kernel.Bind<IArquivoEmpresaRepository>().To<ArquivoEmpresaRepository>();
            kernel.Bind<IArquivoContratadoRepository>().To<ArquivoContratadoRepository>();
            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>();
            kernel.Bind<IContratadoRepository>().To<ContratadoRepository>();
            kernel.Bind<IFuncaoRepository>().To<FuncaoRepository>();
            kernel.Bind<ITipoDocumentoRepository>().To<TipoDocumentoRepository>();
            kernel.Bind<IAssociadoRepository>().To<AssociadoRepository>();
            kernel.Bind<IDocumentoGeralRepository>().To<DocumentoGeralRepository>();
            kernel.Bind<IPontoRepository>().To<PontoRepository>();
            kernel.Bind<IAjustePontoRepository>().To<AjustePontoRepository>();
            kernel.Bind<IHistoricoRepository>().To<HistoricoRepository>();
            kernel.Bind<IConsolidacaoRepository>().To<ConsolidacaoRepository>();
            kernel.Bind<IPerfilRepository>().To<PerfilRepository>();


            kernel.Bind<IMappedEmpresaLocalRepository>().To<MappedEmpresaLocalRepository>();
            kernel.Bind<IMappedLocalRepository>().To<MappedLocalRepository>();
            kernel.Bind<IMappedServicoRepository>().To<MappedServicoRepository>();
        }
    }
}
