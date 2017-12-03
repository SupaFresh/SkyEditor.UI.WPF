﻿Imports System.Reflection
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports SkyEditor.Core.IO
Imports SkyEditor.Core.UI
Imports SkyEditor.Core.Utilities

Namespace MenuActions
    Public Class FileOpenManual
        Inherits MenuAction

        Public Sub New()
            MyBase.New({My.Resources.Language.MenuFile, My.Resources.Language.MenuFileOpen, My.Resources.Language.MenuFileOpenManual})
            AlwaysVisible = True
            SortOrder = 1.22
        End Sub
        Public Overrides Async Sub DoAction(Targets As IEnumerable(Of Object))
            Dim o = CurrentApplicationViewModel.GetOpenFileDialog(False)
            If o.ShowDialog = DialogResult.OK Then
                Dim w As New FileTypeSelector()
                Dim games As New Dictionary(Of String, TypeInfo)
                For Each item In IOHelper.GetOpenableFileTypes(CurrentApplicationViewModel.CurrentPluginManager)
                    games.Add(ReflectionHelpers.GetTypeFriendlyName(item), item)
                Next
                w.SetFileTypeSource(games)
                If w.ShowDialog Then
                    Await CurrentApplicationViewModel.OpenFile(o.FileName, w.SelectedFileType)
                End If
            End If
        End Sub

        Private Sub FileNewSolution_CurrentPluginManagerChanged(sender As Object, e As EventArgs) Handles Me.CurrentApplicationViewModelChanged
            Me.AlwaysVisible = CurrentApplicationViewModel IsNot Nothing AndAlso (CurrentApplicationViewModel.CurrentPluginManager.GetRegisteredObjects(Of IOpenableFile).Any() OrElse CurrentApplicationViewModel.CurrentPluginManager.GetRegisteredObjects(Of IFileOpener).Any(Function(x As IFileOpener) TypeOf x IsNot OpenableFileOpener))
        End Sub
    End Class
End Namespace

