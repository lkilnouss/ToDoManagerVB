Public Class CalendarItem

    Private _uid As Guid
    Private _location As String
    Private _title As String
    Private _description As String
    Private _start As DateTime
    Private _end As DateTime
    Private _createdAt As DateTime

    Public Sub New(l As String, t As String, d As String, st As DateTime, en As DateTime)
        _uid = Guid.NewGuid()
        _location = l
        _title = t
        _description = d
        _start = st
        _end = en
        _createdAt = DateTime.Now()
    End Sub

    Public ReadOnly Property Title As String
        Get
            Return _title
        End Get
    End Property

    Public ReadOnly Property Guid As Guid
        Get
            Return _uid
        End Get
    End Property

    Public ReadOnly Property Location As String
        Get
            Return _location
        End Get
    End Property

    Public ReadOnly Property Description As String
        Get
            Return _description
        End Get
    End Property

    Public ReadOnly Property Start_ As DateTime
        Get
            Return _start
        End Get
    End Property

    Public ReadOnly Property End_ As DateTime
        Get
            Return _end
        End Get
    End Property

    Public ReadOnly Property Stamp_ As DateTime
        Get
            Return _createdAt
        End Get
    End Property
End Class

