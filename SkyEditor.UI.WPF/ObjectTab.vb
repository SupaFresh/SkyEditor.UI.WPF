Imports System.Windows
Imports System.Windows.Controls
Imports SkyEditor.Core.UI
Imports SkyEditor.Core.Utilities

Public Class ObjectTab
    Inherits TabItem

    Public Sub New()
        Margin = New Thickness(0, 0, 0, 0)
    End Sub

    Public Sub New(containedViewControl As IViewControl)
        Me.New
        Me.ContainedViewControl = containedViewControl
    End Sub

    Public Property ContainedViewControl As IViewControl
        Get
            If TypeOf Content Is IViewControl Then
                Return Content
            Else
                Return Nothing
            End If
        End Get
        Set(value As IViewControl)
            If Content IsNot Nothing AndAlso TypeOf Content Is IViewControl Then
                RemoveHandler DirectCast(Content, IViewControl).HeaderUpdated, AddressOf OnContentHeaderChanged
            End If

            If TypeOf value Is UserControl Then
                Content = value
            End If

            If value.Header IsNot Nothing Then
                Header = value.Header
            Else
                Header = ReflectionHelpers.GetTypeFriendlyName(value.GetType)
            End If

            If Content IsNot Nothing AndAlso TypeOf Content Is IViewControl Then
                AddHandler DirectCast(Content, IViewControl).HeaderUpdated, AddressOf OnContentHeaderChanged
            End If
        End Set
    End Property

    Private Sub OnContentHeaderChanged(sender As Object, e As HeaderUpdatedEventArgs)
        Header = e.NewValue
    End Sub

End Class