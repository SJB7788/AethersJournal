using System.Text;
using System.Text.Json;

public class FreeAITherapist
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
        // create new user content 
        GeminiAPIContent newUserContent = new(new(), GeminiAPIRole.user);
        newUserContent.AddPart(input);

        // add it to content history
        _contentHistory.Add(newUserContent);

        // create API Request with the content history
        GeminiAPIRequest requestBody = new GeminiAPIRequest(_contentHistory, _systemInstruction);
        Console.WriteLine(requestBody.ToString());

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

        Console.WriteLine(result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No content generated.");
        
        // create new AI content 
        GeminiAPIContent newAIContent = new(new(), GeminiAPIRole.model);
        newAIContent.AddPart(result?.Candidates.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "");

        // add it to content history
        _contentHistory.Add(newAIContent);

        return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "";
    }

    public async Task<string> Summarize(string journal)
    {
        // create new content to add to the API request
        GeminiAPIContent newContent = new(new(), GeminiAPIRole.user);
        newContent.AddPart(journal);

        // add base prompt
        GeminiAPIContent systemInstruction = new(new(), GeminiAPIRole.system);
        systemInstruction.AddPart("This is a journal entry. This summary will be given to a therapist. Summarize this journal entry to be helpful to a therapist, emphasizing on the emotions, things the person wants to accomplish, wishes to do, etc.");

        // create request
        GeminiAPIRequest requestBody = new GeminiAPIRequest(new() { newContent }, systemInstruction);

        // serialize
        string json = JsonSerializer.Serialize(requestBody);    
        StringContent content = new(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error: {response.StatusCode} - {err}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        var result = JsonSerializer.Deserialize<GeminiAPIResponse>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Console.WriteLine(result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No content generated.");

        return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "";
    }
}