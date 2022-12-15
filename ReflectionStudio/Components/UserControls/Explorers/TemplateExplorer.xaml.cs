using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AvalonDock;
using ReflectionStudio.Classes;
using ReflectionStudio.Classes.Workspace;
using ReflectionStudio.Components.Dialogs.Database;
using ReflectionStudio.Controls;
using ReflectionStudio.Core.Database;
using ReflectionStudio.Core.Database.Schema;
using ReflectionStudio.Core.Events;
using System.IO;
using ReflectionStudio.Core.FileManagement;
using System.ComponentModel;

namespace ReflectionStudio.Components.UserControls
{
	/// <summary>
	/// Interaction logic for TemplateExplorer.xaml
	/// </summary>
	public partial class TemplateExplorer : DockableContent
	{
		#region ----------------------EVENTS----------------------
		
		public TemplateExplorer()
		{
			InitializeComponent();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, DeleteCommandHandler, CanExecuteDeleteCommand));

				this.treeViewTemplate.OnItemNeedPopulate += new TreeViewExtended.ItemNeedPopulateEventHandler(treeViewTemplate_OnItemNeedPopulate);

				FillTreeView(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"));
			}
		}

		#endregion

		#region ----------------------COMMANDS----------------------

		public void CanExecuteDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = this.Visibility == System.Windows.Visibility.Visible & e.Parameter != null;
		}

		public void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			DiskContent dc = e.Parameter as DiskContent;
			if (dc.Delete())
				this.treeViewTemplate.Items.Remove(this.treeViewTemplate.SelectedItem);
		}

		#endregion

		#region ----------------TREEVIEW----------------

		private void FillTreeView(string source)
		{
			Tracer.Verbose("TemplateExplorer:FillTreeView", source);

			if (this.treeViewTemplate == null) return;
			try
			{
				this.treeViewTemplate.Items.Clear();
				AddFolder(null, source);
			}
			catch (Exception err)
			{
				Tracer.Error("TemplateExplorer.FillTreeView", err);
			}
			finally
			{
				Tracer.Verbose("TemplateExplorer:FillTreeView", "END");
			}
		}

		void treeViewTemplate_OnItemNeedPopulate(object sender, RoutedEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.Wait;

				TreeViewItem openedItem = (TreeViewItem)e.OriginalSource;

				DiskContent dc = openedItem.Tag as DiskContent;
				if (dc.IsFolder)
				{
					ParseFolder(openedItem, dc.FullName);
				}
			}
			catch (Exception all)
			{
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		private void AddFolder(TreeViewItem parent, string path)
		{
			DiskContent dc = new DiskContent(path, true);
			this.treeViewTemplate.AddItem(parent, dc.Name, dc );
		}

		private void AddFile(TreeViewItem parent, string path)
		{
			DiskContent dc = new DiskContent(path);
			this.treeViewTemplate.AddItem(parent, dc.Name, dc, false);
		}

		private void ParseFolder(TreeViewItem parent, string path)
		{
			DirectoryInfo DirRef = new DirectoryInfo(path);

			//get all files in the current dir
			FileInfo[] filesRef = DirRef.GetFiles("*.rst");

			//loop through each file
			foreach (FileInfo fRef in filesRef)
				AddFile(parent, fRef.FullName);

			DirectoryInfo[] dirsRef = DirRef.GetDirectories();

			foreach (DirectoryInfo subdirRef in dirsRef)
				AddFolder(parent, subdirRef.FullName);

			//cleanup
			filesRef = null;
			dirsRef = null;
		}

		private void ParseDirectoryRecursively(TreeViewItem parent, string path)
		{
			DirectoryInfo DirRef = new DirectoryInfo(path);

			TreeViewItem DirNode = new TreeViewItem() { Header = DirRef.Name,  Tag = new DiskContent(DirRef.FullName, true) };

			if (parent == null) //root
				this.treeViewTemplate.Items.Add(DirNode);
			else
				parent.Items.Add(DirNode);

			FileInfo[] filesRef;

			//get all files in the current dir
			filesRef = DirRef.GetFiles("*.rst");

			//loop through each file
			foreach (FileInfo fRef in filesRef)
			{
				TreeViewItem Node = new TreeViewItem() { Header = fRef.Name, Tag = new DiskContent( fRef.FullName ) };
				DirNode.Items.Add(Node);
			}

			//cleanup
			filesRef = null;

			DirectoryInfo[] dirsRef;
			dirsRef = DirRef.GetDirectories();

			foreach (DirectoryInfo subdirRef in dirsRef)
			{
				ParseDirectoryRecursively(DirNode, subdirRef.FullName);
			}

			dirsRef = null;
		}

		#endregion

		private void treeViewTemplate_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			BindingView.Instance.PropertyContext = ((TreeViewItem)e.NewValue).Tag;
		}
	}
}
