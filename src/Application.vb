Imports System
Imports System.IO


Module Program

    Sub Main()

        Console.WriteLine("*******************************************************************************************************************")
        Console.WriteLine("                                                   ToDo-List                                                       ")
        Console.WriteLine("*******************************************************************************************************************")
        Console.WriteLine(vbLf)

        Dim taskservice As New TaskService("../../../res/Tasks.json")
        Dim Running As Boolean = True

        While Running

            Running = taskservice.HandleCommand()

        End While

    End Sub

End Module