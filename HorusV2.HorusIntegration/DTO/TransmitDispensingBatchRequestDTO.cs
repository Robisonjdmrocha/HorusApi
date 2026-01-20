using HorusV2.HorusIntegration.Entities;
using HorusV2.HorusIntegration.Entities.Dispensation;

namespace HorusV2.HorusIntegration.DTO;

public class TransmitDispensingBatchRequestDTO
{
    public TransmitDispensingBatchRequestDTO()
    {
    }

    public TransmitDispensingBatchRequestDTO(UsuarioSus usuarioSus,
        EstabelecimentoDispensador estabelecimentoDispensador, Caracterizacao caracterizacao, IEnumerable<Item> itens)
    {
        UsuarioSus = usuarioSus;
        EstabelecimentoDispensador = estabelecimentoDispensador;
        Caracterizacao = caracterizacao;
        Itens = itens;
    }

    public UsuarioSus UsuarioSus { get; set; }
    public EstabelecimentoDispensador EstabelecimentoDispensador { get; set; }
    public Caracterizacao Caracterizacao { get; set; }
    public IEnumerable<Item> Itens { get; set; }
}