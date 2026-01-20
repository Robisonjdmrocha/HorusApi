using System.Collections.Concurrent;

namespace HorusV2.HorusIntegration.DTO;

public class EntradaCaracterizacao
{
    public string CnesCnpjDistribuidor { get; set; }
    public string CodigoOrigem { get; set; }
    public string DataEntrada { get; set; }
    public string NumeroDocumento { get; set; }
    public string TipoEntrada { get; set; }
}

public class EntradaEstabelecimento
{
    public string Cnes { get; set; }
    public string Tipo { get; set; }
}

public class EntradaItem
{
    public string CnpjFabricante { get; set; }
    public string DataValidade { get; set; }
    public List<EntradaIUM> Iums { get; set; }
    public string Lote { get; set; }
    public string NomeFabricanteInternacional { get; set; }
    public string Numero { get; set; }
    public string TipoProduto { get; set; }
    public string CodigoOrigem { get; set; }
    public string SiglaProgramaSaude { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}

public class EntradaIUM
{
    public string Ium { get; set; }
}

public class TransmitEntriesBatchRequestDTO
{
    public EntradaEstabelecimento Estabelecimento { get; set; }
    public EntradaCaracterizacao Caracterizacao { get; set; }
    public IEnumerable<EntradaItem> Itens { get; set; }
}