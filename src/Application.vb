Imports System
Imports System.IO


Module Program

    Dim Running As Boolean = True

    Sub Main()

        Console.WriteLine("*******************************************************************************************************************")
        Console.WriteLine("                                                   ToDo-List                                                       ")
        Console.WriteLine("*******************************************************************************************************************")
        Console.WriteLine(vbLf)

        Dim taskservice As New TaskService("../../../res/Tasks.json")

        While Running = True

            Running = taskservice.HandleCommand()

        End While

    End Sub

End Module