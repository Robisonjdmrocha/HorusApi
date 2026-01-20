/*
    Relatório de Dispensação de Medicamentos
 */
SELECT SP.CNES AS 'CNESEstabelecimento', M.ID AS 'CodigoOrigemDispensacao', M.dataMov AS 'DataDispensacao', MI.ID AS 'CodigoOrigemProduto', PF.catmat 'NumeroProdutoCATMAT', L.lote AS 'NumeroLote', MI.QTD AS 'Quantidade', L.validade AS 'DataValidade', COALESCE(Pessoa.CPF, DocPessoal.Numero) AS 'DocumentoUsuarioSUS', 'B' AS 'TipoProduto'
FROM Setores S
         INNER JOIN AS_SetoresPar SP
                    ON SP.CodSetor = S.CodSetor
         INNER JOIN ALM_Movimentacoes M
                    ON S.CodSetor = M.setor_ID
         INNER JOIN ALM_MovimentacaoItens MI
                    ON MI.movimentacao_ID = M.ID
         INNER JOIN ALM_MovimentacaoDispensacao MD
                    ON M.ID = MD.movimentacao_ID
         INNER JOIN ASSMED_PesFisica Pessoa
                    ON MD.paciente_ID = Pessoa.Codigo
         INNER JOIN ASSMED_CadastroDocPessoal DocPessoal
                    ON Pessoa.Codigo = DocPessoal.Codigo
                        AND DocPessoal.CodTpDocP = 6
         INNER JOIN ALM_Lotes L
                    ON L.ID = MI.lote_ID
         INNER JOIN ALM_Produtos P
                    ON L.produto_ID = P.ID
         INNER JOIN ALM_Produtos_FAR PF
                    ON PF.produto_ID = P.ID
         INNER JOIN Horus.Catmat CATMAT
                    ON PF.catmat = CATMAT.Catmat
                        AND PF.catmat IS NOT NULL
WHERE SP.CNES IS NOT NULL
    AND SP.CNES <> ''
    AND YEAR (
    M.dataMov) = 2023
  AND MONTH (M.dataMov) = 4
  AND M.tipoMov_ID = 19
  AND COALESCE (Pessoa.CPF
    , DocPessoal.Numero) IS NOT NULL
ORDER BY
    SP.CNES ASC,
    M.ID ASC,
    MI.ID ASC

/*
	Relatório de Entrada Materiais e Medicamentos.
*/
SELECT
    CASE WHEN SM.CNPJ IS not NULL THEN SM.CNPJ ELSE CNES.CNES END AS 'CNESEstabelecimento'
     ,case when SM.almoxarifado=1 then 'A' else 'F' end AS 'TipoEstabelecimento'
     ,M.ID AS 'CodigoOrigemEntrada'
     ,M.dataMov AS 'DataEntrada'
     ,CASE WHEN F.CNPJ IS NULL THEN SMO.CNPJ ELSE F.CNPJ END AS 'IdentificacaoDistribuidor'
     ,Mt.codigo +' - ' + MT.descricao AS 'TipoEntrada'
     ,MI.ID AS 'CodigoOrigemProduto'
     ,ISNULL(PF.catmat,P.catmat) as 'NumeroProdutoCATMAT'
     ,L.lote AS 'NumeroLote'
     ,L.validade AS 'DataValidade'
     ,sum(CASE WHEN MT.sigla = 'E' THEN MI.QTD ELSE 0 END) AS 'Quantidade'
     ,MAR.descricao AS 'NomeFabricante'
FROM ALM_Movimentacoes M
         INNER JOIN ALM_MovimentacaoItens MI ON MI.movimentacao_ID = M.ID AND MI.excluido IS NULL
         INNER JOIN ALM_movimentacaoTipos MT ON MT.ID = M.tipoMov_ID
         INNER JOIN Setores SM ON SM.CodSetor = M.setor_ID
         INNER JOIN AS_SetoresPar CNES ON CNES.CodSetor=M.setor_ID
         LEFT join ALM_Fornecedores F on F.ID=M.fornecedor_ID
         INNER JOIN ALM_Lotes L ON L.ID = MI.lote_ID
         INNER JOIN ALM_Produtos P on P.ID=L.produto_ID
         LEFT JOIN ALM_Produtos_FAR PF on PF.ID=L.produtoFAR_ID
         LEFT JOIN ALM_Marcas MAR on MAR.ID=P.marca_ID
         LEFT JOIN ALM_Movimentacoes MO on CONVERT(varchar,MO.ID)=M.numDoc
         LEFT join Setores SMO ON SMO.CodSetor = MO.setor_ID
         LEFT JOIN AS_SetoresPar CNESO ON CNESO.CodSetor=SMO.CodSetor
WHERE isNull(MI.finalizado, 0) = CASE WHEN MT.sigla = 'E' THEN 1 ELSE 0 END
  AND MONTH(M.dataMov) = 9
  AND YEAR(M.dataMov) = 2022
  AND MT.codigo IN ('E-EVENTUAL','E-O','E-AE','E-D','E-PER','E-SI','E-T')
GROUP BY SM.DesSetor,CNES.CNES,M.dataMov ,L.lote,L.validade,MT.descricao,SM.DesSetor,M.ID,SM.almoxarifado,SM.CNPJ,SMO.CNPJ,MT.sigla,MI.QTD,MI.ID,PF.catmat,P.catmat,Mt.codigo,MAR.descricao,F.CNPJ
ORDER BY SM.DesSetor