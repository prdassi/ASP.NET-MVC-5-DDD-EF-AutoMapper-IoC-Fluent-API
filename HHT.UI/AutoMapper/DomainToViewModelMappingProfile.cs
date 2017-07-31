using AutoMapper;
using HHT.Domain.Entities;
using HHT.UI.ViewModels;
using System.Collections.Generic;

namespace HHT.UI.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<EmpresaViewModel, Empresa>();
            Mapper.CreateMap<LocalViewModel, Local>();
            Mapper.CreateMap<ServicoViewModel, Servico>();
            Mapper.CreateMap<ArquivoEmpresaViewModel, ArquivoEmpresa>();
            Mapper.CreateMap<ArquivoContratadoViewModel, ArquivoContratado>();
            Mapper.CreateMap<UsuarioViewModel, Usuario>();
            Mapper.CreateMap<ContratadoViewModel, Contratado>();
            Mapper.CreateMap<FuncaoViewModel, Funcao>();
            Mapper.CreateMap<TipoDocumentoViewModel, TipoDocumento>();
            Mapper.CreateMap<AssociadoViewModel, Associado>();
            Mapper.CreateMap<DocumentoGeralViewModel, DocumentoGeral>();
            Mapper.CreateMap<PontoViewModel, Ponto>();
            Mapper.CreateMap<AjustePontoViewModel, AjustePonto>();
            Mapper.CreateMap<IdentificacaoViewModel, Identificacao>();
                Mapper.CreateMap<IdentificacaoViewModel.DocumentoViewModel, Identificacao.Documento>();
                Mapper.CreateMap<IdentificacaoViewModel.LocalIntegracaoViewModel, Identificacao.LocalIntegracao>();
            Mapper.CreateMap<HistoricoViewModel, Historico>();
            Mapper.CreateMap<ConsolidacaoViewModel, Consolidacao>();
                Mapper.CreateMap<ConsolidacaoViewModel.MesViewModel, Consolidacao.Mes>();
            Mapper.CreateMap<PerfilViewModel, Perfil>();

            Mapper.CreateMap<MappedLocalViewModel, MappedLocal>();
            Mapper.CreateMap<MappedEmpresaLocalViewModel, MappedEmpresaLocal>();
            Mapper.CreateMap<MappedServicoViewModel, MappedServico>();
        }
    }
}