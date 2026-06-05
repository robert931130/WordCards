namespace WordCards;

public class WordItem
{
    public string Word { get; set; } = string.Empty;
    public string Phonogram { get; set; } = string.Empty;
    public string SoundPath { get; set; } = string.Empty;
    public string Explain { get; set; } = string.Empty;

    public WordItem()
    {
    }

    public WordItem(string line)
    {
        string[] fields = line.Split('\t');
        if (fields.Length == 0)
        {
            return;
        }

        Word = fields.ElementAtOrDefault(0) ?? string.Empty;
        Phonogram = fields.ElementAtOrDefault(1) ?? string.Empty;
        SoundPath = fields.ElementAtOrDefault(2) ?? string.Empty;
        Explain = fields.Length > 3
            ? string.Join(Environment.NewLine, fields.Skip(3))
            : string.Empty;
    }

    public string ToLineString()
    {
        string explain = Explain
            .Replace("\r\n", "\t")
            .Replace("\n", "\t")
            .Replace("\r", "\t");

        return string.Join("\t", Word, Phonogram, SoundPath, explain);
    }

    public override string ToString()
    {
        return Word;
    }
}
