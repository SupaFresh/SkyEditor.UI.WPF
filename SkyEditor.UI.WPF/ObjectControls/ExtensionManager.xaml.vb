﻿Imports System.Reflection
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms
Imports SkyEditor.Core.Extensions
Imports SkyEditor.Core.Utilities
Imports SkyEditor.Core.Windows

Namespace ObjectControls
    Public Class ExtensionManager
        Inherits ObjectControl

        Private Function GetTVItem(Collection As IExtensionCollection) As TreeViewItem
            Dim item As New TreeViewItem
            'Todo: await
            item.Header = Collection.GetName().Result
            item.Tag = Collection
            For Each child In Collection.GetChildCollections(CurrentApplicationViewModel.CurrentPluginManager).Result
                item.Items.Add(GetTVItem(child))
            Next
            Return item
        End Function

        Private Sub ExtensionManager_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Me.Header = My.Resources.Language.ExtensionManager
            tvCategories.Items.Clear()
            tvCategories.Items.Add(GetTVItem(New InstalledExtensionCollection))
        End Sub

        Private Sub tvCategories_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object)) Handles tvCategories.SelectedItemChanged
            RefreshCurrentExtensionList()
        End Sub

        Async Sub RefreshCurrentExtensionList()
            If tvCategories.SelectedItem IsNot Nothing Then
                lvExtensions.ItemsSource = Await DirectCast(tvCategories.SelectedItem.Tag, IExtensionCollection).GetExtensions(0, Integer.MaxValue, CurrentApplicationViewModel.CurrentPluginManager)
            End If
        End Sub

        Public Overrides Function GetSupportedTypes() As IEnumerable(Of TypeInfo)
            Return {GetType(ExtensionHelper)}
        End Function

        Private Sub lvExtensions_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lvExtensions.SelectionChanged
            If lvExtensions.SelectedItem IsNot Nothing Then
                btnToggleInstall.Visibility = Visibility.Visible
                Dim info = DirectCast(lvExtensions.SelectedItem, ExtensionInfo)
                If info.IsInstalled Then
                    btnToggleInstall.Content = My.Resources.Language.Uninstall
                Else
                    btnToggleInstall.Content = My.Resources.Language.Install
                End If
            Else
                btnToggleInstall.Visibility = Visibility.Collapsed
            End If
        End Sub

        Private Async Sub btnBrowse_Click(sender As Object, e As RoutedEventArgs) Handles btnBrowse.Click
            Dim o As New OpenFileDialog
            o.Filter = $"{My.Resources.Language.ZipFiles} (*.zip)|*.zip|{My.Resources.Language.AllFiles} (*.*)|*.*"
            If o.ShowDialog = DialogResult.OK Then
                Dim result = Await ExtensionHelper.InstallExtensionZip(o.FileName, CurrentApplicationViewModel.CurrentPluginManager)
                DisplayInstallResultMessage(result)
                RefreshCurrentExtensionList()
            End If
        End Sub

        Private Async Sub btnToggleInstall_Click(sender As Object, e As RoutedEventArgs) Handles btnToggleInstall.Click
            If tvCategories.SelectedItem IsNot Nothing AndAlso lvExtensions.SelectedItem IsNot Nothing Then
                Dim repo = DirectCast(tvCategories.SelectedItem.Tag, IExtensionCollection)
                Dim info = DirectCast(lvExtensions.SelectedItem, ExtensionInfo)
                If info.IsInstalled Then
                    DisplayUninstallResultMessage(Await repo.UninstallExtension(info.ID, CurrentApplicationViewModel.CurrentPluginManager))
                Else
                    'Todo: add UI element to select version, instead of just using the default (info.Version)
                    DisplayInstallResultMessage(Await repo.InstallExtension(info.ID, info.Version, CurrentApplicationViewModel.CurrentPluginManager))
                End If
            End If
        End Sub

        Private Sub DisplayInstallResultMessage(Result As ExtensionInstallResult)
            Select Case Result
                Case ExtensionInstallResult.Success
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionInstallSuccess)
                Case ExtensionInstallResult.RestartRequired
                    If Forms.MessageBox.Show(My.Resources.Language.ExtensionInstallRestart, My.Resources.Language.MainTitle, MessageBoxButtons.YesNo) = DialogResult.Yes Then
                        RedistributionHelpers.RequestRestartProgram()
                    End If
                Case ExtensionInstallResult.InvalidFormat
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionInstallInvalid, My.Resources.Language.MainTitle)
                Case ExtensionInstallResult.UnsupportedFormat
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionInstallUnsupported, My.Resources.Language.MainTitle)
                Case Else
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionInstallUnknownFailure, My.Resources.Language.MainTitle)
            End Select
        End Sub

        Private Sub DisplayUninstallResultMessage(Result As ExtensionUninstallResult)
            Select Case Result
                Case ExtensionUninstallResult.Success
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionUninstallSuccess)
                Case ExtensionUninstallResult.RestartRequired
                    If Forms.MessageBox.Show(My.Resources.Language.ExtensionUninstallRestart, My.Resources.Language.MainTitle, MessageBoxButtons.YesNo) = DialogResult.Yes Then
                        RedistributionHelpers.RequestRestartProgram()
                    End If
                Case Else
                    Forms.MessageBox.Show(My.Resources.Language.ExtensionUninstallUnknownFailure, My.Resources.Language.MainTitle)
            End Select
        End Sub
    End Class
End Namespace

