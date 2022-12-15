using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using AvalonDock;
using ReflectionStudio.Classes;
using ReflectionStudio.Components.Dialogs.Startup;
using ReflectionStudio.Core;
using ReflectionStudio.Core.Events;
using ReflectionStudio.Core.Helpers;

namespace ReflectionStudio.Components.UserControls
{
	/// <summary>
	/// HomeDocument show a xaml flow document that comes from the net (or resource if trouble) 
	/// It is supposed to give news and links
	/// </summary>
	public partial class HomeDocument : DocumentContent
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public HomeDocument()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the xaml if not in design mode in a backgound thread
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);

			Tracer.Verbose("HomeDocument:OnApplyTemplate", "START");

			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				try
				{
					string urlToRead = "http://i3.codeplex.com/Project/Download/FileDownload.aspx?ProjectName=ReflectionStudio&DownloadId=132959";
					string destFile = System.IO.Path.Combine(PathHelper.ApplicationPath, "Home.xaml");

					BackgroundWorker webWorker = new BackgroundWorker();
					webWorker.WorkerReportsProgress = false;
					webWorker.WorkerSupportsCancellation = false;
					webWorker.DoWork += new DoWorkEventHandler(bw_DoWork);
					webWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

					// Start the asynchronous operation.
					webWorker.RunWorkerAsync(new UrlSaveHelper(urlToRead, destFile));

				}
				catch (Exception all)
				{
					Tracer.Error("HomeDocument.OnApplyTemplate", all);
				}
			}

			Tracer.Verbose("HomeDocument:OnApplyTemplate", "END");
		}

		/// <summary>
		/// After the thread complete, load the xaml home document
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// First, handle the case where an exception was thrown.
			if (e.Error != null)
			{
				Tracer.Error("HomeDocument.bw_RunWorkerCompleted", e.Error);
			}
			else
			{
				Tracer.Verbose("HomeDocument:bw_RunWorkerCompleted", "START");

				UrlSaveHelper hlp = (UrlSaveHelper)e.Result;

				string destFile = System.IO.Path.Combine(PathHelper.ApplicationPath, "Home.xaml");

				// Finally, handle the case where the operation succeeded.
				if (File.Exists(destFile))
				{
					try
					{
						FileStream fs = File.OpenRead(destFile);
						FlowDocViewer.Document = (FlowDocument)XamlReader.Load(fs);
						fs.Close();

						Tracer.Verbose("HomeDocument:bw_RunWorkerCompleted", "Internet document loaded");
					}
					catch (Exception)
					{
						//load the config from resources
						using (Stream fs = Application.ResourceAssembly.GetManifestResourceStream("ReflectionStudio.Resources.Embedded.Home.xaml"))
						{
							if (fs == null)
								throw new InvalidOperationException("Could not find embedded resource");

							FlowDocViewer.Document = (FlowDocument)XamlReader.Load(fs);
							fs.Close();

							Tracer.Verbose("HomeDocument:bw_RunWorkerCompleted", "Local document loaded");
						}
					}

					ParseHyperlink(FlowDocViewer.Document);

					Tracer.Verbose("HomeDocument:bw_RunWorkerCompleted", "END");
				}
			}
		}

		/// <summary>
		/// Use UrlSaveHelper to get the home document from internet
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			UrlSaveHelper hlp = (UrlSaveHelper)e.Argument;
			hlp.SaveUrlContent();
		}

		#region --------------------FLOWDOC PROCESS--------------------

		/// <summary>
		/// Find elements in the flow document to associate events
		/// </summary>
		/// <param name="flowDocument"></param>
		private void ParseHyperlink(FlowDocument flowDocument)
		{
			Tracer.Verbose("HomeDocument:ParseHyperlink", "START");
			try
			{
				Hyperlink hp = flowDocument.FindName("HyperlinkNewProject") as Hyperlink;
				hp.Click += new RoutedEventHandler(HyperlinkNewProject_Click);

				hp = null;
				int counter = 1;
				do
				{
					hp = flowDocument.FindName(string.Format("HyperlinkNaviguate{0}", counter)) as Hyperlink;
					if (hp != null)
						hp.RequestNavigate += new RequestNavigateEventHandler(Hyperlink_RequestNavigate);
					counter++;
				}
				while (hp != null);
			}
			catch (Exception all)
			{
				Tracer.Error("HomeDocument.ParseHyperlink", all);
			}
			Tracer.Verbose("HomeDocument:ParseHyperlink", "END");
		}

		/// <summary>
		/// Launch special application link
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HyperlinkNewProject_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			StartupDlg dlg = new StartupDlg();
			dlg.Owner = Application.Current.MainWindow;
			dlg.DataContext = BindingView.Instance;
			dlg.ShowDialog();
		}

		/// <summary>
		/// launch any web request
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			ProcessHelper.LaunchWebUri(e.Uri);
			e.Handled = true;
		}

		#endregion
	}
}
