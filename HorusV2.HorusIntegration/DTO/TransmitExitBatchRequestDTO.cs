namespace HorusV2.HorusIntegration.DTO;

public class TransmitExitBatchRequestDTO
{
    public SaidaEstabelecimento Estabelecimento { get; set; }
    public SaidaCaracterizacao Caracterizacao { get; set; }
    public IEnumerable<SaidaItem> Itens { get; set; }
}

public class SaidaCaracterizacao
{
    public string CodigoOrigem { get; set; }
    public string DataSaida { get; set; }
    public string EstabelecimentoDestino { get; set; }
    public string TipoSaida { get; set; }
}

public class SaidaEstabelecimento
{
    public string Cnes { get; set; }
    public string Tipo { get; set; }
}

public class SaidaItem
{
    public string CnpjFabricante { get; set; }
    public string DataValidade { get; set; }
    public List<SaidaIum> Iums { get; set; }
    public string Lote { get; set; }
    public string NomeFabricanteInternacional { get; set; }
    public string Numero { get; set; }
    public string TipoProduto { get; set; }
    public string CodigoOrigem { get; set; }
    public string SiglaProgramaSaude { get; set; }
    public int Quantidade { get; set; }
}

public class SaidaIum
{
    public string Ium { get; set; }
}
