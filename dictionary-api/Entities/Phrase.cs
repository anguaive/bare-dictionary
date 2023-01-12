namespace dictionary_api.Entities;

public class Phrase
{
    [CsvHelper.Configuration.Attributes.Ignore]
    public int Id { get; set; }

    public string English { get; set; } = default!;
    public string Hungarian { get; set; } = default!;

    public override string ToString()
    {
        return English + " <-> " + Hungarian;
    }
}