using System.Windows.Input;
using ReflectionStudio.Controls;
using ReflectionStudio.Core.Project;

namespace ReflectionStudio
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainWindow : Fluent.RibbonWindow
	{
		#region ---------------------INIT---------------------

		public void SetupCommandBinding()
		{
			this.CommandBindings.Add(new CommandBinding(OpenSnapshotCommand, OnOpenSnapshotCommand, CanExecute));
		}

		#endregion

		#region ---------------------COMMAMNDS---------------------

		private RoutedCommand _OpenSnapshotCmd = new RoutedCommand("OpenSnapshot", typeof(MainWindow));
		public RoutedCommand OpenSnapshotCommand
		{
			get { return _OpenSnapshotCmd; }
		}

		#endregion

		public void CanExecuteAllways(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = ProjectService.Instance.Current != null;
		}
	}
} 
