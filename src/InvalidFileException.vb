Public Class InvalidFileException
    Inherits Exception
    Public Sub New()
        MyBase.New("Invalid Repository File.")
    End Sub
End Class
