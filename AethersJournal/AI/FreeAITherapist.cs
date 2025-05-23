using System.Text;
using System.Text.Json;

class FreeAITherapist
{
    private string _endpoint;
    private HttpClient _httpClient;
    private GeminiAPIContent _systemInstruction;
    private List<GeminiAPIContent> _contentHistory;

    public FreeAITherapist(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={config["GeminiAPIKey"]}";

        _contentHistory = new();

        _systemInstruction = new(new(), GeminiAPIRole.system);
        string basePrompt = "You are an Therapist that will engage in supportive, reflective conversations based on journal entries given to you. Your primary goal is to help me (the user) process my thoughts, emotions, and experiences by simulating aspects of a therapeutic conversation.";
        _systemInstruction.AddPart(basePrompt);
    }

    public async Task<string> Converse(string input)
    {
        // create new content 
        GeminiAPIContent newContent = new(new(), GeminiAPIRole.user);
        newContent.AddPart(input);

        // add it to content history
        _contentHistory.Add(newContent);

        // create API Request with the content history
        GeminiAPIRequest requestBody = new GeminiAPIRequest(_contentHistory, _systemInstruction);
        // Console.WriteLine(requestBody.ToString());

        // serialize
        string json = JsonSerializer.Serialize(requestBody);
        // Console.WriteLine(json);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        // send request
        var response = await _httpClient.PostAsync(_endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error: {response.StatusCode} - {err}");
        }

        // read response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        var result = JsonSerializer.Deserialize<GeminiAPIResponse>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // no content generated 
        return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No content generated.";
    }
}