Imports System.IO
Imports System.Text.Json

Public Class Repository
    Implements IRepository

    Private ReadOnly _filepath As String

    Public Sub New(path As String)
        _filepath = path
    End Sub

    Public Function LoadTasks() As List(Of TaskItem) Implements IRepository.Load

        If Not File.Exists(_filepath) Then
            Throw New InvalidFileException()
        End If

        Dim data As String = File.ReadAllText(_filepath)

        If String.IsNullOrWhiteSpace(data) Then
            Return New List(Of TaskItem)
        End If

        Return JsonSerializer.Deserialize(Of List(Of TaskItem))(data)
    End Function

    Public Sub SaveTasks(tasks As List(Of TaskItem)) Implements IRepository.Save

        If Not File.Exists(_filepath) Then
            Throw New InvalidFileException()
        End If

        Dim data As String = JsonSerializer.Serialize(tasks)
        File.WriteAllText(_filepath, data)
    End Sub
End Class
