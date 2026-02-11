Public Class InvalidIdException
    Inherits Exception

    Public Sub New()
        MyBase.New("Invalid Task ID! Try again")
    End Sub
End Class
