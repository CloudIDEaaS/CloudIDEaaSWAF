Imports System.Windows.Forms

Public Delegate Sub GetSentencesEventHandler(sender As Object, e As GetSentenceEventArgs)

Public Class GetSentenceEventArgs
    Public Property Sentences As String()
    Public ReadOnly Property Text As String

    Public Sub New(text As String)
        Me.Text = text
    End Sub

End Class
