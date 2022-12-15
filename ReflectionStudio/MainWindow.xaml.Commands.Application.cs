using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReflectionStudio.Classes.Workspace;
using ReflectionStudio.Components.Dialogs.About;
using ReflectionStudio.Components.UserControls;
using ReflectionStudio.Controls;
using ReflectionStudio.Controls.Helpers;
using System.IO;
using ReflectionStudio.Core.Project;
using ReflectionStudio.Core.FileManagement;
using ReflectionStudio.Core.Events;
using System;
using ReflectionStudio.Components.Dialogs;
using ReflectionStudio.Classes;
using System.Linq;
using Fluent;

namespace ReflectionStudio
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainWindow : Fluent.RibbonWindow
	{
		#region ---------------------APPLICATION.NEW---------------------

		public void CanExecuteNewCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		public void NewCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			if (string.IsNullOrEmpty((string)e.Parameter)) //default
			{
				//display the dialog
				NewDocumentDlg Dlg = new NewDocumentDlg();
				Dlg.Owner = Application.Current.MainWindow;
				Dlg.DataContext = DocumentFactory.Instance.SupportedDocuments.Where( p => p.CanCreate == true ).ToList();

				if (Dlg.ShowDialog() == true)
					DocumentFactory.Instance.CreateDocument( Dlg.DocumentTypeSelected );
			}
			else //file type as parameter   
			{
				DocumentFactory.Instance.CreateDocument(DocumentFactory.Instance.SupportedDocuments.Find(p => p.Extension == (string)e.Parameter));
			}
			e.Handled = true;
		}

		#endregion

		#region ---------------------APPLICATION.OPEN---------------------

		public void CanExecutOpenCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		/// <summary>
		/// Open compliant and supported file as document
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			if (e.Parameter == null) //default
			{
				OpenProject();
			}
			else  //file as parameter
			{
				DiskContent dc = e.Parameter as DiskContent;

				if (!dc.Exists)
					return;
				else
					DocumentFactory.Instance.OpenDocument(DocumentFactory.Instance.SupportedDocuments.Find(p => p.Extension == dc.Extension),
						new DocumentDataContext() { FullName = dc.FullName });

				WorkspaceService.Instance.AddRecentFile(dc.FullName);
			}
		}

		#endregion

		#region ---------------------APPLICATION.SAVE---------------------

		public void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = DocumentFactory.Instance.ActiveDocument != null;
		}

		public void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			//IFileService iFS = DocumentFactory.Instance.ActiveDocument.DataContext.Service;
		}
		

		#endregion

		#region ---------------------APPLICATION.SAVEAS---------------------

		#endregion

		#region ---------------------APPLICATION.CLOSE---------------------

		#endregion

		#region ---------------------APPLICATION.HELP---------------------

		/// <summary>
		/// Display the help
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void HelpCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			DisplayHelpDocument((string)e.Parameter);
		}

		#endregion

		#region ---------------------APPLICATION.PROPERTIES---------------------

		public void PropertiesCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			DisplayProperties(e.Parameter);
		}

		#endregion

		#region ---------------------APPLICATION.CLIPBOARD---------------------

		public void CanExecuteClipboardCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = DocumentFactory.Instance.ActiveDocument is QueryDocument;
		}

		public void CutCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			QueryDocument qd = DocumentFactory.Instance.ActiveDocument as QueryDocument;
			qd.SyntaxEditor.Cut();
		}

		public void CopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			QueryDocument qd = DocumentFactory.Instance.ActiveDocument as QueryDocument;
			qd.SyntaxEditor.Copy();
		}

		public void PasteCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			QueryDocument qd = DocumentFactory.Instance.ActiveDocument as QueryDocument;
			qd.SyntaxEditor.Paste();
		}
		#endregion

		#region ---------------------FILE.ADDEXISTING---------------------

		public void CanExecuteAddExistingFileCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		public void AddExistingFileCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			if (e.Parameter == null)
				return;

			try
			{
				DiskContent dc = e.Parameter as DiskContent;

				if (!dc.Exists)
					return;
				else
				{
					File.Delete(dc.FullName);
				}
			}
			catch (Exception Error)
			{
				Tracer.Error("MainWindow.DeleteFileCommandHandler", Error);
			}
		}

		#endregion

		#region ---------------------FILE.DELETE---------------------

		public void CanExecuteDeleteFileCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		public void DeleteFileCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			if (e.Parameter == null)
				return;

			try
			{
				DiskContent dc = e.Parameter as DiskContent;

				if (!dc.Exists)
					return;
				else
				{
					File.Delete(dc.FullName);
				}
			}
			catch (Exception Error)
			{
				Tracer.Error("MainWindow.DeleteFileCommandHandler", Error);
			}
		}

		#endregion

		#region ---------------------DIRECT COMMAMNDS---------------------

		/// <summary>
		/// Save the project if needed and close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			this.Close();
		}

		/// <summary>
		/// Display
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
			DisplayHomeDocument();
		}

		/// <summary>
		/// Display the about dialog
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AboutButton_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;

			AboutBoxDlg Dlg = new AboutBoxDlg();
			Dlg.Owner = this;
			Dlg.ShowDialog();
		}
		
		#endregion

		#region ---------------------OTHER COMMAMNDS---------------------

		///// <summary>
		///// Change the application theme
		///// </summary>
		///// <param name="sender"></param>
		///// <param name="e"></param>
		//private void ThemeMenuItem_Click(object sender, RoutedEventArgs e)
		//{
		//    WorkspaceService.Instance.Entity.CurrentTheme = (string)((System.Windows.Controls.MenuItem)sender).Header;
		//    ThemeHelper.LoadTheme(WorkspaceService.Instance.Entity.CurrentTheme);
		//}

		#endregion
	}
} 
