using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using HorusV2.Core.Models;
using Serilog;


namespace HorusV2.Core.Helpers;

public class HttpRequestHelper
{
    public static async Task<TResponse> MakeRequest<TResponse>(HttpRequestModel requestModel)
    {
        try
        {
            string route = string.Empty;

            if (!string.IsNullOrEmpty(requestModel.RequestUri)) route = requestModel.RequestUri;

            using HttpClient client = new();

            client.BaseAddress = new Uri(requestModel.BaseAddress + route);

            HttpRequestMessage request = new()
            {
                Method = new HttpMethod(requestModel.RequestMethod)
            };

            if (!string.IsNullOrWhiteSpace(requestModel.JsonContent))
                request.Content = new StringContent(requestModel.JsonContent, Encoding.UTF8, "application/json");

            foreach (KeyValuePair<string, string> header in requestModel.Headers)
                request.Headers.Add(header.Key, header.Value);

            HttpResponseMessage result = await client.SendAsync(request);

            string jsonResponse = await result.Content.ReadAsStringAsync();

            if(typeof(TResponse) == typeof(JsonObject))
            {
                var jsonObject = JsonObject.Parse(jsonResponse);

                if (jsonObject is null)
                    throw new JsonException(
                        $"Falha ao deserializar resposta da solicitação HTTP. Solicitação: {JsonHelper.ToJson(requestModel)}.\nResposta: {JsonHelper.ToJson(result)}.\n");

                return (TResponse)(object)JsonObject.Parse(jsonResponse);
                 
            } else
            {

                TResponse? deserializedObject = JsonHelper.FromJson<TResponse>(jsonResponse);

                if (deserializedObject is null)
                    throw new JsonException(
                        $"Falha ao deserializar resposta da solicitação HTTP. Solicitação: {JsonHelper.ToJson(requestModel)}.\nResposta: {JsonHelper.ToJson(result)}.\n");

                return deserializedObject;
            }


        }
        catch (Exception ex)
        {
            Log.Error(
                $"Falha ao efetuar requisição HTTP. Solicitação: {JsonHelper.ToJson(requestModel)}.\nError: {ex.Message}.\n");
            throw;
        }
    }
}