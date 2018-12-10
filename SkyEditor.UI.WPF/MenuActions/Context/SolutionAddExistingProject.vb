﻿Imports System.Reflection
Imports System.Windows.Forms
Imports SkyEditor.Core
Imports SkyEditor.Core.Projects
Imports SkyEditor.Core.UI
Imports SkyEditor.UI.WPF.ViewModels.Projects

Namespace MenuActions.Context

    Public Class SolutionAddExistingProject
        Inherits MenuAction

        Public Sub New(pluginManager As PluginManager, applicationViewModel As ApplicationViewModel)
            MyBase.New({My.Resources.Language.MenuAddProject})
            IsContextBased = True

            CurrentApplicationViewModel = applicationViewModel
            CurrentPluginManager = pluginManager
        End Sub

        Public Property CurrentApplicationViewModel As ApplicationViewModel

        Public Property CurrentPluginManager As PluginManager

        Public Overrides Sub DoAction(targets As IEnumerable(Of Object))
            For Each item In targets
                Dim parentSolution As Solution
                Dim parentPath As String

                If TypeOf item Is SolutionHeiarchyItemViewModel Then
                    parentSolution = DirectCast(item, SolutionHeiarchyItemViewModel).Project
                    parentPath = DirectCast(item, SolutionHeiarchyItemViewModel).CurrentPath
                Else
                    Throw New ArgumentException(String.Format(My.Resources.Language.ErrorUnsupportedType, item.GetType.Name))
                End If

                Dim w As New OpenFileDialog
                w.Filter = CurrentApplicationViewModel.GetIOFilter({Project.ProjectFileExt})

                If w.ShowDialog = DialogResult.OK Then
                    parentSolution.AddExistingProject(parentPath, w.FileName, CurrentPluginManager)
                End If
            Next
        End Sub

        Public Overrides Function GetSupportedTypes() As IEnumerable(Of TypeInfo)
            Return {GetType(SolutionHeiarchyItemViewModel).GetTypeInfo}
        End Function

        Public Overrides Function SupportsObject(obj As Object) As Task(Of Boolean)
            If TypeOf obj Is SolutionHeiarchyItemViewModel Then
                Dim node As SolutionHeiarchyItemViewModel = obj
                Return Task.FromResult(node.IsDirectory AndAlso node.Project.CanCreateProject(node.CurrentPath))
            Else
                Return Task.FromResult(False)
            End If
        End Function

    End Class

End Namespace