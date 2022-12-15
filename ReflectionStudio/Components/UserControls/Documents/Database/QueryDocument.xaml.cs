using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;
using ReflectionStudio.Classes;
using ReflectionStudio.Controls.Helpers;
using ReflectionStudio.Core.Database;
using ReflectionStudio.Core.Database.Schema;
using ReflectionStudio.Core.Events;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;

namespace ReflectionStudio.Components.UserControls
{
	/// <summary>
	/// QueryDocument allow to edit, modify and run SQL queries
	/// </summary>
	public partial class QueryDocument : ZoomDocument
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public QueryDocument()
		{
			InitializeComponent();
		}

		private enum QueryOutputMode { Grid, Text };
		private QueryOutputMode OutputMode { get; set; }

		static public RoutedCommand CheckQuery = new RoutedCommand("CheckQuery", typeof(QueryDocument));
		static public RoutedCommand ExecuteQuery = new RoutedCommand("ExecuteQuery", typeof(QueryDocument));
		static public RoutedCommand StopExecuteQuery = new RoutedCommand("StopExecuteQuery", typeof(QueryDocument));
		static public RoutedCommand OutputQueryMode = new RoutedCommand("OutputQueryMode", typeof(QueryDocument));

		public DocumentDataContext Context { get { return this.DataContext as DocumentDataContext; } }
		public DataSource DataSource { get { return this.Context.Entity as DataSource; } }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				SetOutputMode( QueryOutputMode.Grid );
				SyntaxEditor.TextArea.LayoutTransform = base.ScaleTransformer;
				this.textblockSource.Text = DataSource.Name;
				this.MainGrid.RowDefinitions[2].Height = new GridLength(0);

				CommandBindings.Add(new CommandBinding(CheckQuery, CheckQueryCommandHandler, CanExecuteQueryCommand));
				CommandBindings.Add(new CommandBinding(ExecuteQuery, ExecuteQueryCommandHandler, CanExecuteQueryCommand));
				CommandBindings.Add(new CommandBinding(StopExecuteQuery, StopQueryCommandHandler, CanExecuteStopQuery));
				CommandBindings.Add(new CommandBinding(OutputQueryMode, OutputQueryModeCommandHandler, CanExecuteOutputQueryMode));
			}
		}

		#region ---------------------COMMAMND HANDLERS---------------------

		public void CanExecuteQueryCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = !string.IsNullOrEmpty(SyntaxEditor.Text);
		}

		public void CheckQueryCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			string checkQuery = string.Format("SET PARSEONLY ON {0}; SET PARSEONLY OFF;", SyntaxEditor.Text);
			try
			{
				this.DataSource.Database.Provider.GetSQLQueryInterface().ExecuteData(this.DataSource.ConnectionString, checkQuery);
			}
			catch (Exception all)
			{
			}
		}

		private BackgroundWorker _Worker = null;

		public void ExecuteQueryCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			//execute the thread that run the query on the database as ExecuteReader is blocking
			try
			{
				//display the context options 
				ShowResultPane();
				this.dataGridResult.Columns.Clear();
				
				//display the progress, 
				EventDispatcher.Instance.RaiseStatus("Executing query !", StatusEventType.StartProgress);

				QueryContext context = new QueryContext()
					{
						UIDispatcher = this.Dispatcher,
						Command = SyntaxEditor.Text,
						Source = this.DataSource,
						StartTime = DateTime.Now
					};

				if (OutputMode == QueryOutputMode.Grid)
				{
					context.AddColumn = new QueryContext.AddColumnDelegate(GridAddColumnHandler);
					context.AddData = new QueryContext.AddRowDelegate(GridAddDataHandler);
				}
				else
				{
					context.AddColumn = new QueryContext.AddColumnDelegate(TextAddColumnHandler);
					context.AddData = new QueryContext.AddRowDelegate(TextAddDataHandler);
				}

				// Start the asynchronous operation.
				_Worker = new BackgroundWorker();
				_Worker.WorkerSupportsCancellation = true;
				_Worker.DoWork += new DoWorkEventHandler(bw_RequestWork);
				_Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RequestCompleted);
				_Worker.ProgressChanged += new ProgressChangedEventHandler(bw_RequestProgressChanged);

				_Worker.RunWorkerAsync(context);
			}
			catch (Exception all)
			{
			}
		}


		public void CanExecuteStopQuery(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = (_Worker != null);
		}

		public void StopQueryCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;

			// Cancel the asynchronous operation.
			_Worker.CancelAsync();
			_Worker = null;
		}

		public void CanExecuteOutputQueryMode(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		public void OutputQueryModeCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			e.Handled = true;
			SetOutputMode( e.Parameter .ToString()== "Grid" ? QueryOutputMode.Grid : QueryOutputMode.Text );
		}

		private void SetOutputMode(QueryOutputMode mode)
		{
			OutputMode = mode;

			if (OutputMode == QueryOutputMode.Grid)
			{
				(this.tabControlResult.Items[0] as System.Windows.Controls.TabItem).Visibility = System.Windows.Visibility.Visible;
				(this.tabControlResult.Items[1] as System.Windows.Controls.TabItem).Visibility = System.Windows.Visibility.Hidden;
			}
			else
			{
				(this.tabControlResult.Items[0] as System.Windows.Controls.TabItem).Visibility = System.Windows.Visibility.Hidden;
				(this.tabControlResult.Items[1] as System.Windows.Controls.TabItem).Visibility = System.Windows.Visibility.Visible;
			}
		}
		#endregion

		#region ---------------------REQUEST WORKER---------------------

		protected void GridAddColumnHandler(string headerText)
		{
			if (this.dataGridResult.ItemsSource == null)
				this.dataGridResult.ItemsSource = new ObservableCollection<dynamic>();

			DataGridTextColumn dt = new DataGridTextColumn();
			dt.Header = headerText;
			dt.Binding = new Binding(headerText);
			this.dataGridResult.Columns.Add(dt);
		}

		protected void GridAddDataHandler(dynamic data)
		{
			(this.dataGridResult.ItemsSource as ObservableCollection<dynamic>).Add(data);
		}

		protected void TextAddColumnHandler(string header)
		{
			this.textBlockResult.Text += header;
			this.textBlockResult.Text += ";";
		}

		protected void TextAddDataHandler(dynamic data)
		{
			foreach (var property in (IDictionary<String, Object>)data)
			{
				this.textBlockResult.Text += property.Value.ToString();
				this.textBlockResult.Text += ";";
			}
			this.textBlockResult.Text += "\n";
		}

		protected void bw_RequestWork(object sender, DoWorkEventArgs e)
		{
			QueryWorker qw = new QueryWorker((BackgroundWorker)sender, e);
			qw.ExecuteQuery();
			e.Result = qw.Context;
		}

		protected void bw_RequestProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		protected void bw_RequestCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			// First, handle the case where an exception was thrown.
			if (e.Error != null)
			{
				EventDispatcher.Instance.RaiseStatus("Query error !", StatusEventType.StopProgress);
			}
			else if (e.Cancelled)
			{
				// Next, handle the case where the user canceled the operation.
				// Note that due to a race condition in the DoWork event handler, the Cancelled
				// flag may not have been set, even though CancelAsync was called.
				EventDispatcher.Instance.RaiseStatus("Query canceled !", StatusEventType.StopProgress);
			}
			else if (e.Result != null)
			{
				// Finally, handle the case where the operation succeeded.
				EventDispatcher.Instance.RaiseStatus("Query executed !", StatusEventType.StopProgress);

				this.Dispatcher.Invoke((ThreadStart)delegate
				{
					QueryContext qc = e.Result as QueryContext;
					this.textblockMessage.Text = qc.Message;
					this.textblockTime.Text = (qc.StartTime - DateTime.Now).ToString(@"hh\:mm\:ss\:ff");
					this.textblockLines.Text = qc.LineCount.ToString();
				});
			}

			_Worker = null;
		}

		#endregion

		#region ---------------------SCRIPTING---------------------

		public void ScriptCreate(SchemaObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Create(dbo);
		}

		public void ScriptAlter(SchemaObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Alter(dbo);
		}

		public void ScriptDrop(SchemaObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Drop(dbo);
		}

		public void ScriptSelect(ITabularObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Select(dbo);
		}
		public void ScriptInsert(ITabularObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Insert(dbo);
		}
		public void ScriptUpdate(ITabularObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Update(dbo);
		}
		public void ScriptDelete(ITabularObjectBase dbo)
		{
			IDbSQLWriter writer = this.DataSource.Database.Provider.GetSQLWriterInterface();
			SyntaxEditor.Text = writer.Delete(dbo);
		}
		#endregion

		#region ---------------------INTERNALS---------------------

		private void ShowResultPane()
		{
			this.MainGrid.RowDefinitions[2].Height = new GridLength( 1, GridUnitType.Star);
		}

		//void bw_DoWork(object sender, DoWorkEventArgs e)
		//{
		//    IDataReader reader = this.DataSource.Database.Provider.GetSQLQueryInterface().ExecuteReader(this.DataSource.ConnectionString, SyntaxEditor.Text);

		//    while (reader.Read())
		//    {
		//        builder.Append(reader.GetString(0));
		//        _Worker.ReportProgress(0, message);
		//    }
		//    reader.Close();
		//}

		//void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		//{
		//}

		//void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		//{
		//    // First, handle the case where an exception was thrown.
		//    if (e.Error != null)
		//    {
		//        EventDispatcher.Instance.RaiseStatus("Project build error !", StatusEventType.StopProgress);
		//    }
		//    else if (e.Cancelled)
		//    {
		//        // Next, handle the case where the user canceled the operation.
		//        // Note that due to a race condition in the DoWork event handler, the Cancelled
		//        // flag may not have been set, even though CancelAsync was called.
		//        EventDispatcher.Instance.RaiseStatus("Project build canceled !", StatusEventType.StopProgress);
		//    }
		//    else if ((bool)e.Result == true)
		//    {
		//        // Finally, handle the case where the operation succeeded.
		//        EventDispatcher.Instance.RaiseStatus("Project builded !", StatusEventType.StopProgress);
		//    }

		//    System.Media.SystemSounds.Exclamation.Play();

		//    _Worker = null;
		//}

		#endregion
	}
}
