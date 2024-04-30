Public Class Form1

    

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim newFont As New FontDialog
        If newFont.ShowDialog = Windows.Forms.DialogResult.OK Then
            If RichTextBox1.SelectionLength = 0 Then
                RichTextBox1.Font = newFont.Font
            Else
                RichTextBox1.SelectionFont = newFont.Font
            End If
        End If
    End Sub

    
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'Using w As New System.IO.StreamWriter("D:\Messagelog2.txt", True)
        'w.WriteLine("Change Font")
        'w.Flush()
        'w.Close()
        'End Using

        Dim newFont As New FontDialog
        If newFont.ShowDialog = Windows.Forms.DialogResult.OK Then
            If RichTextBox2.SelectionLength = 0 Then
                RichTextBox2.Font = newFont.Font
            Else
                RichTextBox2.SelectionFont = newFont.Font
            End If
        End If
    End Sub

    Private Sub RichTextBox1_FontChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.FontChanged
        MessageBox.Show("Font Changed")
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            NHunspellTextBoxExtender1.EnableTextBoxBase(RichTextBox1)
        Else
            NHunspellTextBoxExtender1.DisableTextBoxBase(RichTextBox1)
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        NHunspellTextBoxExtender1.Dispose()
    End Sub
End Class
