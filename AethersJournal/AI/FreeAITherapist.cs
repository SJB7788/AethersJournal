using System.Text;
using System.Text.Json;

class FreeAITherapist : IAITherapist
{
    private HttpClient _httpClient;
    private ChatLog[] _chatLogs;
    private string _basePrompt = """
    You are an Therapist that will engage in supportive, reflective conversations based on journal entries given to you. " + 
        "Your primary goal is to help me (the user) process my thoughts, emotions, and experiences by simulating aspects of a therapeutic conversation.
    """;
    private StringBuilder _stringBuilder;

    public FreeAITherapist(HttpClient httpClient)
    {
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

        using StringContent jsonPrompt = new(
            JsonSerializer.Serialize(new
            {
                contents = ""

            })
        );

        await _httpClient.PostAsync("https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key=", )
    }
}