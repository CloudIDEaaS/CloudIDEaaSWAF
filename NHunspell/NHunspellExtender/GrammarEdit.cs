#if USE_API_SERVICES
using ApiServices.Services;
using ApiServices.Library;

public class GrammarEdit
{
    public int StartGrammar { get; }
    public int EndGrammar { get; }
    public string Sentence { get; }
    public GrammarBotResult Result { get; }
    public bool Handled { get; set; }
    public bool Ingored { get; set; }

    public GrammarEdit(int startGrammar, int endGrammar, string sentence, GrammarBotResult result)
    {
        StartGrammar = startGrammar;
        EndGrammar = endGrammar;
        Sentence = sentence;
        Result = result;
    }

    public bool HandledOrIgnored
    {
        get
        {
            return Handled || Ingored;
        }
    }
}
#endif
