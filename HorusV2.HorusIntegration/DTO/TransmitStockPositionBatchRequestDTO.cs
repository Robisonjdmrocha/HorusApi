namespace HorusV2.HorusIntegration.DTO;

public class PosicaoEstoqueCaracterizacao
{
    public string CodigoOrigem { get; set; }
    public string DataPosicaoEstoque { get; set; }
}

public class PosicaoEstoqueEstabelecimento
{
    public string Cnes { get; set; }
    public string Tipo { get; set; }
}

public class PosicaoEstoqueItem
{
    public string CnpjFabricante { get; set; }
    public string DataValidade { get; set; }
    public List<PosicaoEstoqueIum> Iums { get; set; }
    public string Lote { get; set; }
    public string NomeFabricanteInternacional { get; set; }
    public string Numero { get; set; }
    public string TipoProduto { get; set; }
    public string CodigoOrigem { get; set; }
    public string SiglaProgramaSaude { get; set; }
    public decimal ValorUnitario { get; set; }
    public int Quantidade { get; set; }
}

public class PosicaoEstoqueIum
{
    public string Ium { get; set; }
}

public class TransmitStockPositionBatchRequestDTO
{
    public PosicaoEstoqueEstabelecimento Estabelecimento { get; set; }
    public PosicaoEstoqueCaracterizacao Caracterizacao { get; set; }
    public IEnumerable<PosicaoEstoqueItem> Itens { get; set; }
}
