﻿Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Input
Imports SkyEditor.Core.Projects
Imports SkyEditor.UI.WPF.MenuActions.Context
Imports SkyEditor.UI.WPF.ViewModels.Projects

Namespace ObjectControls
    Public Class SolutionExplorer

        Private Sub SolutionExplorer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            menuContext.CurrentPluginManager = Me.CurrentPluginManager
        End Sub

        Private Sub SolutionExplorer_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Me.PropertyChanged
            If e.PropertyName = NameOf(CurrentPluginManager) Then
                menuContext.CurrentPluginManager = Me.CurrentPluginManager
            End If
        End Sub

        Private Async Sub tvSolutions_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles tvSolutions.MouseDoubleClick
            If tvSolutions.SelectedItem IsNot Nothing AndAlso TypeOf tvSolutions.SelectedItem Is ProjectHeiarchyItemViewModel Then
                Await ProjectNodeOpenFile.OpenFile(tvSolutions.SelectedItem, CurrentPluginManager)
            End If
        End Sub

        Private Sub tvSolutions_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object)) Handles tvSolutions.SelectedItemChanged
            menuContext.Target = tvSolutions.SelectedItem
        End Sub
    End Class

End Namespace
