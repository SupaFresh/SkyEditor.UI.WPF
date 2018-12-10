Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

<Obsolete> Public Class SearchableDropDown
    Inherits ComboBox
    Private DefaultValue As Object
    Private DefaultIndex As Integer

    Public Function Contains(ByVal Text As String) As Boolean
        Dim found As Boolean = False
        For Each item In Items
            If item.ToString = Text Then
                found = True
                Exit For
            End If
        Next
        Return found
    End Function

    Public Function IndexOf(ByVal Text As String) As Object
        Dim output As Object = Nothing
        For Each item In Items
            If item.ToString = Text Then
                output = item
                Exit For
            End If
        Next
        Return output
    End Function

    Private Sub SearchableDropDown_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If Contains(Text) Then
            DefaultValue = SelectedValue
            DefaultIndex = SelectedIndex
            Background = New SolidColorBrush(Windows.Media.Color.FromRgb(229, 229, 229))
            FontStyle = FontStyles.Normal
        Else
            Background = New SolidColorBrush(Windows.Media.Color.FromRgb(255, 0, 0))
            FontStyle = FontStyles.Oblique
        End If
    End Sub

    Private Sub SearchableDropDown_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        IsEditable = True
    End Sub

    ''' <summary>
    ''' Gets the current value.  If the current text is not a valid item, returns the previous valid item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LastSafeValue As Object
        Get
            If Contains(Text) Then
                Return IndexOf(Text)
            Else
                Return DefaultValue
            End If
        End Get
        Set(value As Object)
            SelectedValue = value
        End Set
    End Property

    Public Property LastSafeIndex As Integer
        Get
            If Contains(Text) Then
                Return SelectedIndex
            Else
                Return DefaultIndex
            End If
        End Get
        Set(value As Integer)
            SelectedIndex = value
        End Set
    End Property

    Private Sub SearchableDropDown_TextInput(sender As Object, e As TextCompositionEventArgs) Handles Me.TextInput

    End Sub

End Class