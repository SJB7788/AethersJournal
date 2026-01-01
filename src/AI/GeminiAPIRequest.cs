using System.Text;
using System.Text.Json.Serialization;

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