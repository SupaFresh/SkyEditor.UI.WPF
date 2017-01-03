﻿Imports System.Reflection
Imports System.Windows.Forms
Imports SkyEditor.Core.UI

Namespace MenuActions
    Public Class FileSaveAs
        Inherits MenuAction
        Public Overrides Function SupportedTypes() As IEnumerable(Of TypeInfo)
            Return {GetType(FileViewModel).GetTypeInfo}
        End Function

        Public Overrides Function SupportsObject(obj As Object) As Task(Of Boolean)
            Return Task.FromResult(TypeOf obj Is FileViewModel AndAlso DirectCast(Obj, FileViewModel).CanSaveAs(CurrentPluginManager))
        End Function

        Public Overrides Async Sub DoAction(targets As IEnumerable(Of Object))
            For Each item As FileViewModel In targets
                Dim s = CurrentPluginManager.CurrentIOUIManager.GetSaveFileDialog(item)
                If s.ShowDialog = DialogResult.OK Then
                    Await item.Save(s.FileName, CurrentPluginManager)
                End If
            Next
        End Sub

        Public Sub New()
            MyBase.New({My.Resources.Language.MenuFile, My.Resources.Language.MenuFileSave, My.Resources.Language.MenuFileSaveFileAs})
            SortOrder = 1.32
        End Sub
    End Class
End Namespace

