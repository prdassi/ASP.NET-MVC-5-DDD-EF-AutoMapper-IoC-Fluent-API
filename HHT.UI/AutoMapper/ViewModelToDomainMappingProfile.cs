using AutoMapper;
using HHT.Domain.Entities;
using HHT.UI.ViewModels;
using System.Collections.Generic;

namespace HHT.UI.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Empresa, EmpresaViewModel>();
            Mapper.CreateMap<Local, LocalViewModel>();
            Mapper.CreateMap<Servico, ServicoViewModel>();
            Mapper.CreateMap<ArquivoEmpresa, ArquivoEmpresaViewModel>();
            Mapper.CreateMap<ArquivoContratado, ArquivoContratadoViewModel>();
            Mapper.CreateMap<Usuario, UsuarioViewModel>();
            Mapper.CreateMap<Contratado, ContratadoViewModel>();
            Mapper.CreateMap<Funcao, FuncaoViewModel>();
            Mapper.CreateMap<TipoDocumento, TipoDocumentoViewModel>();
            Mapper.CreateMap<Associado, AssociadoViewModel>();
            Mapper.CreateMap<DocumentoGeral, DocumentoGeralViewModel>();
            Mapper.CreateMap<Ponto, PontoViewModel>();
            Mapper.CreateMap<AjustePonto, AjustePontoViewModel>();
            Mapper.CreateMap<Identificacao, IdentificacaoViewModel>();
                Mapper.CreateMap<Identificacao.Documento, IdentificacaoViewModel.DocumentoViewModel>();
                Mapper.CreateMap<Identificacao.LocalIntegracao, IdentificacaoViewModel.LocalIntegracaoViewModel>();
            Mapper.CreateMap<Historico, HistoricoViewModel>();
            Mapper.CreateMap<Consolidacao, ConsolidacaoViewModel>();
                Mapper.CreateMap<Consolidacao.Mes, ConsolidacaoViewModel.MesViewModel>();
            Mapper.CreateMap<Perfil, PerfilViewModel>();

            Mapper.CreateMap<MappedLocal, MappedLocalViewModel>();
            Mapper.CreateMap<MappedEmpresaLocal, MappedEmpresaLocalViewModel>();
            Mapper.CreateMap<MappedServico, MappedServicoViewModel>();
        }
    }
}