using System.Text;
using System.Text.Json.Serialization;

public class GeminiAPICandidates
{
    [JsonPropertyName("content")]
    public GeminiAPIContent Content { get; set; }

    public GeminiAPICandidates(GeminiAPIContent content)
    {
        Content = content;
    }
}

public class GeminiAPIResponse
{
    [JsonPropertyName("candidates")]
    public List<GeminiAPICandidates> Candidates { get; set; }

    public GeminiAPIResponse(List<GeminiAPICandidates> candidates)
    {
        Candidates = candidates;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        if (Candidates == null || Candidates.Count == 0)
        {
            sb.AppendLine("No candidates returned.");
            return sb.ToString();
        }

        int index = 1;
        foreach (var candidate in Candidates)
        {
            sb.AppendLine($"Candidate #{index++}:");

            var content = candidate.Content;
            var role = content.Role?.ToString() ?? "model";

            sb.AppendLine($"  Role: {role}");

            if (content.Parts != null && content.Parts.Count > 0)
            {
                foreach (var part in content.Parts)
                {
                    sb.AppendLine($"    - {part.Text}");
                }
            }
            else
            {
                sb.AppendLine("    (No parts)");
            }
        }

        return sb.ToString();
    }

}