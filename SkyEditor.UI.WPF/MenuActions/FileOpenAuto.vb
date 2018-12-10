﻿Imports System.Windows
Imports SkyEditor.Core
Imports SkyEditor.Core.IO
Imports SkyEditor.Core.IO.PluginInfrastructure
Imports SkyEditor.Core.Projects
Imports SkyEditor.Core.Settings
Imports SkyEditor.Core.UI

Namespace MenuActions

    Public Class FileOpenAuto
        Inherits MenuAction

        Public Sub New(pluginManager As PluginManager, applicationViewModel As ApplicationViewModel)
            MyBase.New({My.Resources.Language.MenuFile, My.Resources.Language.MenuFileOpen, My.Resources.Language.MenuFileOpenAuto})

            CurrentApplicationViewModel = applicationViewModel
            CurrentPluginManager = pluginManager
            AlwaysVisible = (CurrentPluginManager.GetRegisteredObjects(Of IOpenableFile).Any() OrElse
                CurrentPluginManager.GetRegisteredObjects(Of IFileOpener).Any(Function(x As IFileOpener) TypeOf x IsNot OpenableFileOpener) OrElse
                CurrentPluginManager.CurrentSettingsProvider.GetIsDevMode)
            SortOrder = 1.21
        End Sub

        Public Property CurrentApplicationViewModel As ApplicationViewModel

        Public Property CurrentPluginManager As PluginManager

        Public Overrides Async Sub DoAction(Targets As IEnumerable(Of Object))
            Dim o = CurrentApplicationViewModel.GetOpenFileDialog(True)
            If o.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                If o.FileName.ToLower.EndsWith(".skysln") Then
                    Dim openProjectTask = ProjectBase.OpenProjectFile(o.FileName, CurrentPluginManager)
                    CurrentApplicationViewModel.ShowLoading(openProjectTask)

                    Dim solution = Await openProjectTask
                    If TypeOf solution Is Solution Then
                        CurrentApplicationViewModel.CurrentSolution = solution
                    Else
                        MessageBox.Show(My.Resources.Language.Menu_FileOpenAuto_SolutionOpenFailed)
                    End If
                Else
                    Await CurrentApplicationViewModel.OpenFile(o.FileName)
                End If
            End If
        End Sub

    End Class

End Namespace