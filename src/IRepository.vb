Public Interface IRepository
    Function Load() As List(Of TaskItem)
    Sub Save(tasks As List(Of TaskItem))
End Interface

