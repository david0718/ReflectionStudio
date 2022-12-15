using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ReflectionStudio.Classes;

namespace ReflectionStudio.Components.UserControls
{
	/// <summary>
	/// Interaction logic for TemplateDocument.xaml
	/// </summary>
	public partial class TemplateDocument : ZoomDocument
	{
		public TemplateDocument()
		{
			InitializeComponent();
		}

		private void Document_Loaded(object sender, RoutedEventArgs e)
		{
			this.SyntaxEditor.TextArea.LayoutTransform = ScaleTransformer;

			if (File.Exists(((DocumentDataContext)DataContext).FullName))
				Open(((DocumentDataContext)DataContext).FullName);
		}

		public void Open( string fileName)
		{
			using (StreamReader sr = File.OpenText(fileName))
			{
				this.SyntaxEditor.Text = sr.ReadToEnd();
			}
		}

		public void Save()
		{

		}

		#region ---------------------APPLICATION.SAVE---------------------

		public void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = DocumentFactory.Instance.ActiveDocument == this;
		}

		public void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			Save();
		}


		#endregion
	}
}
