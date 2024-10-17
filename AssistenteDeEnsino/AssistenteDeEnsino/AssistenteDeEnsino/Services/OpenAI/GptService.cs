using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace AssistenteDeEnsino.Services.OpenAI;

public class GPTService : IGptService
{
    private readonly OpenIAOptions _openIaOptions;

    public GPTService(IOptions<OpenIAOptions> openIaOptions)
    {
        _openIaOptions = openIaOptions.Value;
    }

    public async Task<string> ProcessarPergunta(string pergunta, string transcript)
    {
        try
        {
            // Cliente
            ChatClient client = new(
                    model: GptModelExtension.ToApiModel(GptModel.Gpt3_5Turbo),
                    apiKey: _openIaOptions.ApiKey);

            // Prompt
            var prompt = $@"
                    O seguinte é o transcript de um vídeo:
                    {transcript}

                    O usuário fez a seguinte pergunta:
                    {pergunta}  
                    
                    Siga as diretivas a seguir:

                    1 - Encontre a resposta para a pergunta do usuário no transcript acima e forneça a resposta junto com o minuto do vídeo onde essa informação pode ser encontrada. 
                    2 - Toda resposta deve ser fornecida em markwdown puro e sempre formatada o melhor possivel, destaque pontos relevantes, faça listas, monte tabelas sempre que possível e etc, não utilize titulos com textos usando fontes grandes (usando #, ##, etc), apenas negrito. 
                    3 - Não forneça detalhes na integra sobre o transcript. Formule a resposta por conta propria sem repassar o transcript na integra ou parcialmente. 
                    4 - Nunca forneça uma resposta que não tenha relação com o conteúdo fornecido, você pode ir além e usar sua base se o assunto for referente ao tema do transcript, caso no lugar, responda educadamente que está limitado ao tema do vídeo (nunca mencione que está se baseando em um transcript)";

            // Envia o prompt e aguarda uma resposta
            ChatCompletion completion = await client.CompleteChatAsync(prompt);

            return completion.Content[0].Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");

            return "Desculpe, não foi possível obter a resposta no momento, tente novamente mais tarde";
        }
    }
}
