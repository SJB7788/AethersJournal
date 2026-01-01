using System.Text.Json.Serialization;

public enum GeminiAPIRole
{
    [JsonPropertyName("user")]
    user,

    [JsonPropertyName("model")]
    model,

    [JsonPropertyName("system")]
    system
}

public class GeminiAPIContent
{
    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GeminiAPIRole? Role { get; set; }

    [JsonPropertyName("parts")]
    public List<GeminiAPIPart> Parts { get; set; }

    public GeminiAPIContent(List<GeminiAPIPart> parts, GeminiAPIRole? role = null)
    {
        Role = role;
        Parts = parts;
    }

    public void AddPart(string text)
    {
        GeminiAPIPart newPart = new(text);
        Parts.Add(newPart);
    }

    public override string ToString()
    {
        string returnString = $"Role: {Role}\nPart: ";

        for (int i = 0; i < Parts.Count; i++)
        {
            returnString += $"{Parts[i].Text} ";
        }

        return returnString;
    }
}

public class GeminiAPISystemContent : GeminiAPIContent
{
    public string SystemPrompt = String.Empty;
    public string JournalSummary = String.Empty;

    public GeminiAPISystemContent(List<GeminiAPIPart> parts, GeminiAPIRole? role = null) : base(parts, role)
    {
        Role = role;
        Parts = parts;
    }

    public void AddOrEditSystemPrompt(string prompt)
    {
        SystemPrompt = prompt;

        if (Parts.Count > 0)
        {
            Parts[0].Text = SystemPrompt;
            return;
        }

        GeminiAPIPart newSystemPart = new(prompt);
        Parts.Add(newSystemPart);
    }

    public void AddOrEditJournalSummaryPart(string summary)
    {
        JournalSummary = summary;
        string partContent = $"Journal Summary: {JournalSummary}";

        if (Parts.Count > 1)
        {
            Parts[1].Text = partContent;
        }
        else if (Parts.Count == 1)
        {
            GeminiAPIPart newSystemPart = new(partContent);
            Parts.Add(newSystemPart);
        } else
        {
            Console.WriteLine("Error: You must add a system prompt first");
        }
    }
}

public class GeminiAPIPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    public GeminiAPIPart(string text)
    {
        Text = text;
    }
}