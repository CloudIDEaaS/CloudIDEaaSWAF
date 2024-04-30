using System;

public delegate void GetSentencesEventHandler(object sender, GetSentenceEventArgs e);

public class GetSentenceEventArgs : EventArgs
{
    public string[] Sentences { get; set; }
    public string Text { get; }

    public GetSentenceEventArgs(string text)
    {
        this.Text = text;
    }
}


