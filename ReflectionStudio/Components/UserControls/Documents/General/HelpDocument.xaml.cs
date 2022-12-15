using System.Windows;
using System.Windows.Xps.Packaging;
using AvalonDock;
using ReflectionStudio.Classes;
using ReflectionStudio.Core;

namespace ReflectionStudio.Components.UserControls
{
	/// <summary>
	/// Interaction logic for HelpDocument.xaml
	/// </summary>
	public partial class HelpDocument : DocumentContent
	{
		public HelpDocument()
		{
			InitializeComponent();
		}

		private void HelpUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			XpsDocument xpsHelp = new XpsDocument(System.IO.Path.Combine(PathHelper.ApplicationPath, ((DocumentDataContext)DataContext).FullName),
					System.IO.FileAccess.Read);

			documentViewer1.Document = xpsHelp.GetFixedDocumentSequence();
		}
	}
}
