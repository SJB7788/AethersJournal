using System.Text;
using System.Text.Json.Serialization;

// API Response / Request Classes
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
    public GeminiAPIRole? Role {get; set;}

    [JsonPropertyName("parts")]
    public List<GeminiAPIPart> Parts {get; set;}

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
}

public class GeminiAPIPart
{
    [JsonPropertyName("text")]
    public string Text {get; set;}

    public GeminiAPIPart(string text)
    {
        Text = text;
    }
}

// API Request Classes
public class GeminiAPIRequest
{
    [JsonPropertyName("system_instruction")]
    public GeminiAPIContent? SystemInstruction {get; set;}

    [JsonPropertyName("contents")]
    public List<GeminiAPIContent> Contents { get; set; }

    public GeminiAPIRequest(List<GeminiAPIContent> contents, GeminiAPIContent? systemInstruction = null)
    {
        Contents = contents;
        SystemInstruction = systemInstruction;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (SystemInstruction != null)
        {
            sb.AppendLine("System Instruction:");
            foreach (var part in SystemInstruction.Parts)
            {
                sb.AppendLine($"  - {part.Text}");
            }
        }

        sb.AppendLine("Contents:");
        foreach (var content in Contents)
        {
            var role = content.Role?.ToString() ?? "unknown";
            sb.AppendLine($"  Role: {role}");
            foreach (var part in content.Parts)
            {
                sb.AppendLine($"    - {part.Text}");
            }
        }

        return sb.ToString();
    }
}

// API Response Classes
public class GeminiAPICandidates
{
    [JsonPropertyName("contents")]
    public GeminiAPIContent Content {get; set;}

    public GeminiAPICandidates(GeminiAPIContent content)
    {
        Content = content;
    }
}

public class GeminiAPIResponse
{
    [JsonPropertyName("candidates")]
    public List<GeminiAPICandidates> Candidates {get; set;}

    public GeminiAPIResponse(List<GeminiAPICandidates> candidates)
    {
        Candidates = candidates;
    }
}