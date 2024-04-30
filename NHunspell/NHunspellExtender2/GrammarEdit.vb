Imports ApiServices.Services
Imports ApiServices.Library

Public Class GrammarEdit

    Public ReadOnly Property StartGrammar As Integer
    Public ReadOnly Property EndGrammar As Integer
    Public ReadOnly Property Sentence As String
    Public ReadOnly Property Result As GrammarBotResult
    Public Property Handled As Boolean
    Public Property Ingored As Boolean

    Public Sub New(startGrammar As Integer, endGrammar As Integer, sentence As String, result As GrammarBotResult)
        Me.StartGrammar = startGrammar
        Me.EndGrammar = endGrammar
        Me.Sentence = sentence
        Me.Result = result
    End Sub

    Public ReadOnly Property HandledOrIgnored As Boolean

        Get
            Return Me.Handled Or Me.Ingored
        End Get

    End Property

End Class
