<Obsolete> Public Class GenericListItem(Of T)
    Implements IComparable

    Public Property Text As String
    Public Property Value As T

    Public Sub New(Text As String, Value As T)
        Me.Text = Text
        Me.Value = Value
    End Sub

    Public Sub New()
        Text = ""
        Value = Nothing
    End Sub

    Public Overrides Function ToString() As String
        Return Text
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return If(TypeOf obj Is GenericListItem(Of T), DirectCast(obj, GenericListItem(Of T)).Text = Text, False)
    End Function

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        If TypeOf obj Is GenericListItem(Of T) Then
            Return Text.CompareTo(DirectCast(obj, GenericListItem(Of T)).Text)
        Else
            Return 0
        End If
    End Function

End Class