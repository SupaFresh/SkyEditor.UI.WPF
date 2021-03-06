﻿Imports System.Reflection
Imports System.Windows.Controls
Imports SkyEditor.Core.UI

<Obsolete("Will be deleted in the future.  Use DataBoundObjectControl instead.")> Public MustInherit Class ObjectControl
    Inherits UserControl
    Implements IViewControl

    ''' <summary>
    ''' Updates UI elements to display certain properties.
    ''' </summary>
    Public Overridable Sub RefreshDisplay()

    End Sub

    ''' <summary>
    ''' Updates the EditingObject using data in UI elements.
    ''' </summary>
    Public Overridable Sub UpdateObject()

    End Sub

    ''' <summary>
    ''' Returns an IEnumeriable of Types that this control can display or edit.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function GetSupportedTypes() As IEnumerable(Of TypeInfo) Implements IViewControl.GetSupportedTypes
        Dim context = DataContext
        If context IsNot Nothing Then
            Return context.GetType
        Else
            Return {}
        End If
    End Function

    ''' <summary>
    ''' Determines whether or not the control supports the given object.
    ''' The given object will inherit or implement one of the types in GetSupportedTypes.
    ''' </summary>
    ''' <param name="Obj"></param>
    ''' <returns></returns>
    Public Overridable Function SupportsObject(Obj As Object) As Boolean Implements IViewControl.SupportsObject
        Return True
    End Function

    ''' <summary>
    ''' If True, this control will not be used if another one exists.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function IsBackupControl() As Boolean Implements IViewControl.GetIsBackupControl
        Return False
    End Function

    Public Overridable Function GetSortOrder(currentType As TypeInfo, isTab As Boolean) As Integer Implements IViewControl.GetSortOrder
        Return 0
    End Function

    ''' <summary>
    ''' Called when Header is changed.
    ''' </summary>
    Public Event HeaderUpdated(sender As Object, e As HeaderUpdatedEventArgs) Implements IViewControl.HeaderUpdated

    ''' <summary>
    ''' Called when IsModified is changed.
    ''' </summary>
    Public Event IsModifiedChanged As EventHandler Implements IViewControl.IsModifiedChanged

    ''' <summary>
    ''' Returns the value of the Header.  Only used when the iObjectControl is behaving as a tab.
    ''' </summary>
    ''' <returns></returns>
    Public Property Header As String Implements IViewControl.Header
        Get
            Return _header
        End Get
        Set(value As String)
            _header = value
            RaiseEvent HeaderUpdated(Me, New HeaderUpdatedEventArgs With {.NewValue = value})
        End Set
    End Property

    Dim _header As String

    ''' <summary>
    ''' Returns the current EditingObject, after casting it to type T.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <returns></returns>
    Protected Function GetEditingObject(Of T)() As T
        If TypeOf _editingObject Is T Then
            Return DirectCast(_editingObject, T)
        Else
            'I should probably throw my own exception here, since I'm casting EditingObject to T even though I just found that EditingObject is NOT T, but there will be an exception anyway
            Return DirectCast(_editingObject, T)
        End If
    End Function

    ''' <summary>
    ''' Returns the current EditingObject.
    ''' It is recommended to use GetEditingObject(Of T), since it returns iContainter(Of T).Item if the EditingObject implements that interface.
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetEditingObject() As Object
        Return _editingObject
    End Function

    Protected Sub SetEditingObject(Of T)(Value As T)
        If TypeOf _editingObject Is T Then
            _editingObject = Value
        Else
            _editingObject = Value
        End If
    End Sub

    ''' <summary>
    ''' The way to get the EditingObject from outside this class.  Refreshes the display on set, and updates the object on get.
    ''' Calling this from inside this class could result in a stack overflow, especially if called from UpdateObject, so use GetEditingObject or GetEditingObject(Of T) instead.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Property EditingObject As Object Implements IViewControl.ViewModel
        Get
            UpdateObject()
            Return _editingObject
        End Get
        Set(value As Object)
            _editingObject = value
            RefreshDisplay()
        End Set
    End Property

    Dim _editingObject As Object

    ''' <summary>
    ''' Whether or not the EditingObject has been modified without saving.
    ''' Set to true when the user changes anything in the GUI.
    ''' Set to false when the object is saved, or if the user undoes every change.
    ''' </summary>
    ''' <returns></returns>
    Public Property IsModified As Boolean Implements IViewControl.IsModified
        Get
            Return _isModified
        End Get
        Set(value As Boolean)
            Dim oldValue As Boolean = _isModified
            _isModified = value
            If Not oldValue = _isModified Then
                RaiseEvent IsModifiedChanged(Me, New EventArgs)
            End If
        End Set
    End Property

    Dim _isModified As Boolean
End Class