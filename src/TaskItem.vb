Public Class TaskItem

    Public Property _id As Guid
    Public Property _title As String
    Public Property _description As String
    Public Property _isCompleted As Boolean
    Public Property _createdAt As DateTime
    Public Property _completedAt As Nullable(Of DateTime)

    Public Property Id As Guid
        Get
            Return _id
        End Get
        Set(value As Guid)
            _id = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Public Property IsCompleted As Boolean
        Get
            Return _isCompleted
        End Get
        Set(value As Boolean)
            _isCompleted = value
        End Set
    End Property

    Public Property CreatedAt As DateTime
        Get
            Return _createdAt
        End Get
        Set(value As DateTime)
            _createdAt = value
        End Set
    End Property

    Public Property CompletedAt As Nullable(Of DateTime)
        Get
            Return _completedAt
        End Get
        Set(value As Nullable(Of DateTime))
            _completedAt = value
        End Set
    End Property

    Public Sub New(t As String, d As String)
        _id = Guid.NewGuid()
        _title = t
        _description = d
        _isCompleted = False
        _createdAt = DateTime.Now
        _completedAt = Nothing
    End Sub

    Public Sub New()
        'for json
    End Sub

    Public Sub Complete()
        _isCompleted = True
        _completedAt = DateTime.Now
    End Sub

End Class
