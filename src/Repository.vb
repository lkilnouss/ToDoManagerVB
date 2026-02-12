Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text.Json

Public Class Repository
    Implements IRepository
    Implements ICalendar


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

    Public Sub SaveCal(appoint As CalendarItem) Implements ICalendar.Save
        Dim output_path As String = "../../../res/output/calendar/" + appoint.Title + ".ics"

        Dim start_time As String = Format(appoint.Start_, "yyyyMMdd\THHmmss\Z")
        Dim end_time As String = Format(appoint.End_, "yyyyMMdd\THHmmss\Z")
        Dim create_time As String = Format(appoint.Stamp_, "yyyyMMdd\THHmmss\Z")

        Dim data As String = "BEGIN:VCALENDAR" + vbLf +
                             "VERSION:2.0" + vbLf +
                             "METHOD:PUBLISH" + vbLf +
                             "BEGIN:VEVENT" + vbLf +
                             "UID:" + appoint.Guid.ToString() + vbLf +
                             "LOCATION:" + appoint.Location + vbLf +
                             "SUMMARY:" + appoint.Title.ToString() + vbLf +
                             "DESCRIPTION:" + appoint.Description + vbLf +
                             "DTSTART:" + start_time + vbLf +
                             "DTEND:" + end_time + vbLf +
                             "DTSTAMP:" + create_time + vbLf +
                             "END:VEVENT" + vbLf +
                             "END:VCALENDAR"

        Using writer = New StreamWriter(output_path)
            writer.Write(data)
        End Using

    End Sub
End Class
