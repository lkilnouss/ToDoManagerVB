Public Class InvalidFileException
    Inherits Exception
    Public Sub New()
        MyBase.New("Invalid file path")
    End Sub
End Class
