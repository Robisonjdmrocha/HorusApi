using System.Data;
using System.Diagnostics;
using HorusV2.Domain.Data.Repositories;
using HorusV2.Domain.Entities;
using HorusV2.Infrastructure.Data.Relational;
using Serilog;

namespace HorusV2.Infrastructure.Data.Repositories;

public class StreamingMovementRepository : BaseRepository, IStreamingMovementRepository
{
    #region SqlScripts

    private const string _addScript = @"
                                        INSERT INTO Movimentacoes_Por_Transmissao
                                        (
                                            identificador_unico, 
                                            id_solicitacao_transmissao, 
                                            id_tipo_movimentacao, 
                                            data_transmissao,
                                            protocolo_horus,
                                            Sucesso_fl

                                        ) OUTPUT Inserted.id
                                        VALUES
                                        (
                                            @UniqueIdentifier, 
                                            @StreamingRequestId, 
                                            @TransmissionType, 
                                            @TransmissionDate,
                                            @HorusProtocol,
                                            @Sucesso_fl
                                        );";

    #endregion

    public StreamingMovementRepository(RelationalDatabaseContext relationalDatabaseContext,
        IDbTransaction databaseTransaction) : base(relationalDatabaseContext, databaseTransaction)
    {
    }

    public async Task Add(StreamingMovement streamingMovement)
    {

        SaveJsonLogs(streamingMovement);

        streamingMovement.Id = await ExecuteScalarAsync(_addScript, streamingMovement);
    }

    public Task<IEnumerable<StreamingMovement>> GetByStreamingRequest(int streamingRequestId)
    {
        throw new NotImplementedException();
    }

    private void SaveJsonLogs(StreamingMovement streamingMovement)
    {
        try
        {
            // Obtém o caminho raiz do projeto
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;

            // Define o caminho do diretório com base no Identificador Geral
            string directoryPath = Path.Combine(projectRoot, "LogsStraming\\" + streamingMovement.StreamingRequestId.ToString());

            // Verifica se o diretório já existe, se não, cria
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Define o caminho do arquivo com base no Identificador de Solicitação
            string envioFilePath = Path.Combine(directoryPath, $"{streamingMovement.JsonEnvio.Length.ToString()}_{streamingMovement.UniqueIdentifier.ToString()}_envio.json");
            string retornoFilePath = Path.Combine(directoryPath, $"{streamingMovement.JsonRetorno.Length.ToString()}_{streamingMovement.UniqueIdentifier.ToString()}_retorno.json");

            // Salva o JSON de envio
            File.WriteAllText(envioFilePath, streamingMovement.JsonEnvio);

            // Salva o JSON de retorno
            File.WriteAllText(retornoFilePath, streamingMovement.JsonRetorno);
        }
        catch (Exception ex)
        {
            Log.Error($"Erro ao criar arquivo log streaming. Identificador: {streamingMovement.UniqueIdentifier.ToString()}\nErro:{ex.Message}");
        }
       
    } 
}