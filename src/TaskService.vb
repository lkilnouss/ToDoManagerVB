Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Text.Json

Public Enum Commands
    _add = 0
    _list = 1
    _list_open = 2
    _list_done = 3
    _complete_id = 4
    _delete_id = 5
    _help = 6
    _exit = 7
    _invalid = 8
End Enum

' Encapsulate ParseCommand in a Module
Public Module CommandParser
    Public Function ParseCommand(c As String) As Commands
        Select Case c
            Case "add", "add "
                Return Commands._add
            Case "list", "list "
                Return Commands._list
            Case "list open", "list open "
                Return Commands._list_open
            Case "list done", "list done "
                Return Commands._list_done
            Case "complete", "complete "
                Return Commands._complete_id
            Case "delete", "delete "
                Return Commands._delete_id
            Case "help", "help "
                Return Commands._help
            Case "exit", "exit "
                Return Commands._exit
            Case Else
                Return Commands._invalid
        End Select
    End Function
End Module

Public Class TaskService

    Private _taskitems As List(Of TaskItem)
    Private _repository As Repository

    Public Sub New(filepath As String)
        _repository = New Repository(filepath)

        Try
            _taskitems = _repository.LoadTasks
        Catch ex As InvalidFileException
            Console.WriteLine("Exception encountered: " + ex.Message)
        Catch ex As JsonException
            Console.WriteLine("Exception encountered while loading the .json task file: " + ex.Message)
        Catch ex As Exception
            Console.WriteLine("Unknown Exception while loading file:" + ex.Message)
        End Try
    End Sub

    Public Function PollCommands() As Commands
        Dim cmd As String = Console.ReadLine()
        Return CommandParser.ParseCommand(cmd)
    End Function

    Public Function HandleCommand() As Boolean
        Try
            Dim cmd As Commands = PollCommands()
            Select Case cmd
                Case Commands._add
                    Console.WriteLine(vbLf + "Title: ")
                    Dim title As String = Console.ReadLine()
                    Console.WriteLine(vbLf + "Description: ")
                    Dim description As String = Console.ReadLine()
                    Dim task As New TaskItem(title, description)
                    _taskitems.Add(task)
                    _repository.SaveTasks(_taskitems)
                    Console.WriteLine(vbLf + "Task created succesfully!" + vbLf)
                    Return True
                Case Commands._list
                    For Each TaskItem In _taskitems
                        Console.WriteLine(vbLf + "ID: " + TaskItem.Id.ToString +
                                          vbLf + "Task: " + TaskItem.Title +
                                          vbLf + "Description: " + TaskItem.Description +
                                          vbLf + "Created at: " + TaskItem.CreatedAt +
                                          vbLf + "Completed: " + TaskItem.IsCompleted.ToString)
                        If TaskItem.IsCompleted Then
                            Console.WriteLine("Completed at: " + TaskItem.CompletedAt + vbLf)
                        End If
                    Next
                    Return True
                Case Commands._list_open
                    For Each TaskItem In _taskitems
                        If Not TaskItem.IsCompleted Then
                            Console.WriteLine(vbLf + "ID: " + TaskItem.Id.ToString +
                                              vbLf + "Task: " + TaskItem.Title +
                                              vbLf + "Description: " + TaskItem.Description +
                                              vbLf + "Created at: " + TaskItem.CreatedAt +
                                              vbLf)
                        End If
                    Next
                    Return True
                Case Commands._list_done
                    For Each TaskItem In _taskitems
                        If TaskItem.IsCompleted Then
                            Console.WriteLine(vbLf + "ID: " + TaskItem.Id.ToString +
                                              vbLf + "Task: " + TaskItem.Title +
                                              vbLf + "Description: " + TaskItem.Description +
                                              vbLf + "Created at: " + TaskItem.CreatedAt +
                                              vbLf + "Completed: " + TaskItem.IsCompleted.ToString +
                                              vbLf + "Completed at: " + TaskItem.CompletedAt +
                                              vbLf)
                        End If
                    Next
                    Return True
                Case Commands._complete_id
                    Console.WriteLine("ID: ")
                    Dim id As String = Console.ReadLine()
                    Dim completed_id As Guid = Guid.Parse(id)
                    For Each TaskItem In _taskitems
                        If TaskItem.Id = completed_id Then
                            TaskItem.Complete()
                        End If
                    Next
                    _repository.SaveTasks(_taskitems)
                    Return True
                Case Commands._delete_id 'DOESNT WORK MAAAAAAAAN'
                    Console.WriteLine("ID: ")
                    Dim id As String = Console.ReadLine()
                    Dim deleted_id As Guid = Guid.Parse(id)
                    For Each TaskItem In _taskitems
                        If TaskItem.Id = deleted_id Then
                            _taskitems.Remove(TaskItem)
                        End If
                    Next
                    _repository.SaveTasks(_taskitems)
                    Return True
                Case Commands._help
                    Console.WriteLine(vbLf + "Following commands can be used: " + vbLf + vbLf +
                                      "<add>:       adds a new task" + vbLf +
                                      "<list>:      lists all tasks" + vbLf +
                                      "<list open>: lists all open tasks" + vbLf +
                                      "<list done>: lists all done tasks" + vbLf +
                                      "<complete>:  marks a task as complete" + vbLf +
                                      "<delete>:    deletes a task" + vbLf +
                                      "<exit>:      terminates the program" + vbLf)
                    Return True
                Case Commands._exit
                    Return False
                Case Commands._invalid
                    Throw New InvalidCommandException()
                    Return True
            End Select

        Catch ex As InvalidCommandException
            Console.WriteLine(ex.Message)
            Return True
        Catch ex As Exception
            Console.WriteLine("Unknown Exception: " + ex.Message + " Terminating Program...")
            Return False
        End Try

    End Function
End Class

