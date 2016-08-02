﻿Imports System.IO
Imports System.Reflection
Imports System.Windows
Imports SkyEditor.Core.IO
Imports SkyEditor.Core.Projects
Imports SkyEditor.Core.UI

Namespace MenuActions.Context
    Public Class ProjectNodeOpenFile
        Inherits MenuAction

        Public Overrides Async Sub DoAction(Targets As IEnumerable(Of Object))
            For Each item As ProjectNode In Targets
                Dim obj = Await item.GetFile(CurrentPluginManager, AddressOf IOHelper.PickFirstDuplicateMatchSelector)
                If obj IsNot Nothing Then
                    CurrentPluginManager.CurrentIOUIManager.OpenFile(obj, item.ParentProject)
                Else
                    Dim f = Path.Combine(Path.GetDirectoryName(item.ParentProject.Filename), item.Filename)
                    If Not File.Exists(f) Then
                        MessageBox.Show(String.Format(My.Resources.Language.ErrorCantFindFileAt, f))
                    End If
                End If
            Next
        End Sub

        Public Overrides Function SupportedTypes() As IEnumerable(Of TypeInfo)
            Return {GetType(ProjectNode).GetTypeInfo}
        End Function

        Public Overrides Function SupportsObject(Obj As Object) As Boolean
            Return TypeOf Obj Is ProjectNode AndAlso Not DirectCast(Obj, ProjectNode).IsDirectory
        End Function

        Public Sub New()
            MyBase.New({My.Resources.Language.MenuFileOpen})
            IsContextBased = True
        End Sub
    End Class
End Namespace

