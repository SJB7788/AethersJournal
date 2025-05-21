using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

class FreeAITherapist
{
    private string _apiKey;
    private HttpClient _httpClient;
    private ChatLog[] _chatLogs;
    private string _basePrompt = """
    You are an Therapist that will engage in supportive, reflective conversations based on journal entries given to you. " + 
        "Your primary goal is to help me (the user) process my thoughts, emotions, and experiences by simulating aspects of a therapeutic conversation.
    """;
    private StringBuilder _stringBuilder;

    public FreeAITherapist(IConfiguration config, HttpClient httpClient)
    {
        _apiKey = config["GeminiAPIKey"]!;
        _httpClient = httpClient;
        _chatLogs = [];

        _stringBuilder = new StringBuilder(_basePrompt);
        _stringBuilder.AppendLine("Chat History:");
    }

    public async Task<string> Converse(string input)
    {
        // if (_chatLogs.Length != 0)
        // {
        //     foreach (ChatLog log in _chatLogs)
        //     {
        //         _stringBuilder.AppendLine(log.ToString());
        //     }
        // }

        _stringBuilder.AppendLine(new ChatLog(ChatProfile.User, input).ToString());

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = input }
                    }
                }
            }
        };

        string json = JsonSerializer.Serialize(requestBody);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        string endpoint = $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key={_apiKey}";
        var response = await _httpClient.PostAsync(endpoint, content);
        
         if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error: {response.StatusCode} - {err}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        // var result = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse, new JsonSerializerOptions
        // {
        //     PropertyNameCaseInsensitive = true
        // });

        // return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No content generated.";
    
    }
}