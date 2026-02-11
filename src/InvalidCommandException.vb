Public Class InvalidCommandException
    Inherits Exception

    Public Sub New()
        MyBase.New("Invalid Command. See <help> for all commands!")
    End Sub

End Class

