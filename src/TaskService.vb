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
        Console.Write(">>")
        Dim cmd As String = Console.ReadLine()
        Return CommandParser.ParseCommand(cmd)
    End Function

    Public Function HandleCommand() As Boolean
        Try
            Dim cmd As Commands = PollCommands()
            Select Case cmd
                Case Commands._add
                    Console.Write(vbLf + "Title: ")
                    Dim title As String = Console.ReadLine()
                    Console.Write(vbLf + "Description: ")
                    Dim description As String = Console.ReadLine()
                    Dim task As New TaskItem(title, description)
                    _taskitems.Add(task)
                    _repository.SaveTasks(_taskitems)
                    Console.WriteLine(vbLf + "Task created succesfully!" + vbLf)
                    Return True
                Case Commands._list
                    Console.WriteLine("****************************************************************" + vbLf)
                    If _taskitems.Count = 0 Then
                        Console.WriteLine("No tasks to be done" + vbLf)
                    End If
                    For Each TaskItem In _taskitems
                        Console.Write("ID: " + TaskItem.Id.ToString +
                                          vbLf + "Task: " + TaskItem.Title +
                                          vbLf + "Description: " + TaskItem.Description +
                                          vbLf + "Created at: " + TaskItem.CreatedAt +
                                          vbLf + "Completed: " + TaskItem.IsCompleted.ToString +
                                          vbLf)
                        If TaskItem.IsCompleted Then
                            Console.WriteLine("Completed at: " + TaskItem.CompletedAt + vbLf)
                        End If
                    Next
                    Console.WriteLine(vbLf + "****************************************************************" + vbLf)
                    Return True
                Case Commands._list_open
                    Console.WriteLine("****************************************************************" + vbLf)
                    For Each TaskItem In _taskitems
                        If Not TaskItem.IsCompleted Then
                            Console.WriteLine(vbLf + "ID: " + TaskItem.Id.ToString +
                                              vbLf + "Task: " + TaskItem.Title +
                                              vbLf + "Description: " + TaskItem.Description +
                                              vbLf + "Created at: " + TaskItem.CreatedAt +
                                              vbLf)
                        End If
                    Next
                    Console.WriteLine(vbLf + "****************************************************************" + vbLf)
                    Return True
                Case Commands._list_done
                    Console.WriteLine("****************************************************************" + vbLf)
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
                    Console.WriteLine(vbLf + "****************************************************************" + vbLf)
                    Return True
                Case Commands._complete_id
                    Try
                        Console.Write(vbLf + "ID: ")
                        Dim id As String = Console.ReadLine()
                        Dim completed_id As Guid = Guid.Parse(id)
                        For Each TaskItem In _taskitems
                            Dim completed_sth As Boolean = False
                            If TaskItem.Id = completed_id Then
                                If TaskItem.IsCompleted() Then
                                    Console.WriteLine(vbLf + "Task was already completed!" + vbLf)
                                    Return True
                                End If
                                TaskItem.Complete()
                                completed_sth = True
                            End If
                            If Not completed_sth Then
                                Throw New InvalidIdException()
                            End If
                        Next
                    Catch ex As InvalidIdException
                        Console.WriteLine(vbLf + "Task Id exception: " + ex.Message + " Try again with valid task id!" + vbLf)
                        Return True
                    Catch ex As System.FormatException
                        Console.WriteLine(vbLf + "Format exception: " + ex.Message + " Try again with valid task id!" + vbLf)
                        Return True
                    Catch ex As Exception
                        Console.WriteLine(vbLf + "Unhandled Exception: " + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    End Try

                    Try
                        _repository.SaveTasks(_taskitems)
                    Catch ex As InvalidFileException
                        Console.WriteLine(vbLf + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    Catch ex As Exception
                        Console.WriteLine(vbLf + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    End Try

                    Console.WriteLine(vbLf + "Task completed successfully. Great job!" + vbLf)
                    Return True
                Case Commands._delete_id
                    Try
                        Console.Write(vbLf + "ID: ")
                        Dim id As String = Console.ReadLine()
                        Dim deleted_id As Guid = Guid.Parse(id)

                        Dim deleted_sth As Boolean = False

                        For Each TaskItem In _taskitems
                            If TaskItem.Id = deleted_id Then
                                _taskitems.Remove(TaskItem)
                                deleted_sth = True
                                Exit For
                            End If
                        Next

                        If Not deleted_sth Then
                            Throw New InvalidIdException
                        End If

                    Catch ex As InvalidIdException
                        Console.WriteLine(vbLf + ex.Message + " Try again with valid task id!" + vbLf)
                        Return True
                    Catch ex As System.FormatException
                        Console.WriteLine(vbLf + "Format Exception: " + ex.Message + " Try again with valid task id!" + vbLf)
                        Return True
                    Catch ex As Exception
                        Console.WriteLine(vbLf + "Unhandled Exception: " + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    End Try

                    Try
                        _repository.SaveTasks(_taskitems)
                    Catch ex As InvalidFileException
                        Console.WriteLine(vbLf + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    Catch ex As Exception
                        Console.WriteLine(vbLf + ex.Message + vbLf)
                        Console.WriteLine("Terminating program...")
                        Return False
                    End Try
                    Console.WriteLine("Task removed succesfully.")
                    Return True
                Case Commands._help
                    Console.WriteLine("****************************************************************" + vbLf)
                    Console.WriteLine("Following commands can be used: " + vbLf + vbLf +
                                      "<add>:       adds a new task" + vbLf +
                                      "<list>:      lists all tasks" + vbLf +
                                      "<list open>: lists all open tasks" + vbLf +
                                      "<list done>: lists all done tasks" + vbLf +
                                      "<complete>:  marks a task as complete" + vbLf +
                                      "<delete>:    deletes a task" + vbLf +
                                      "<exit>:      terminates the program" + vbLf)
                    Console.WriteLine(vbLf + "****************************************************************" + vbLf)
                    Return True
                Case Commands._exit
                    Console.WriteLine(vbLf + "Terminating Program..." + vbLf)
                    Return False
                Case Commands._invalid
                    Throw New InvalidCommandException()
                    Return True
            End Select

        Catch ex As InvalidCommandException
            Console.WriteLine(ex.Message + vbLf)
            Return True
        Catch ex As Exception
            Console.WriteLine("Unknown Exception: " + ex.Message + " Terminating Program..." + vbLf)
            Return False
        End Try

    End Function
End Class

