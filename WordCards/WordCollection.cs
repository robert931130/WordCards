using System.Collections.ObjectModel;
using System.Text;

namespace WordCards;

public class WordCollection : Collection<WordItem>
{
    public void LoadFromStringArray(string[] lines)
    {
        Clear();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            Add(new WordItem(line));
        }
    }

    public void SaveToFile(string filePath)
    {
        using StreamWriter writer = new(filePath, false, Encoding.Unicode);

        foreach (WordItem item in this)
        {
            writer.WriteLine(item.ToLineString());
        }
    }
}
