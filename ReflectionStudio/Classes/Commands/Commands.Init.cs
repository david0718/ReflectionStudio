using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ReflectionStudio.Components.UserControls;
using System.Windows;
using System.Windows.Documents;

namespace ReflectionStudio.Classes
{
	static public partial class RSCommands
	{
		/// <summary>
		/// Init commands
		/// </summary>
		/// <param name="main"></param>
		/// <param name="target"></param>
		static public void ApplicationCommandBinding(MainWindow main)
		{
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, main.NewCommandHandler, main.CanExecuteNewCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, main.OpenCommandHandler, main.CanExecutOpenCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, main.SaveCommandHandler, main.CanExecuteSaveCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, main.CutCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, main.CutCommandHandler, main.CanExecuteClipboardCommand));

			//main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, main.DeleteFileCommandHandler, main.CanExecuteDeleteFileCommand));

			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, main.CutCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, main.CopyCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, main.PasteCommandHandler, main.CanExecuteClipboardCommand));

			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, main.PasteCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, main.PasteCommandHandler, main.CanExecuteClipboardCommand));

			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, main.HelpCommandHandler, null));

			main.CommandBindings.Add(new CommandBinding(ApplicationCommands.Properties, main.PropertiesCommandHandler, null));

			main.CommandBindings.Add(new CommandBinding(NavigationCommands.DecreaseZoom, main.PasteCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(NavigationCommands.IncreaseZoom, main.PasteCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(NavigationCommands.Zoom, main.PasteCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, main.PasteCommandHandler, main.CanExecuteClipboardCommand));

			main.CommandBindings.Add(new CommandBinding(EditingCommands.DecreaseIndentation, main.PasteCommandHandler, main.CanExecuteClipboardCommand));
			main.CommandBindings.Add(new CommandBinding(EditingCommands.IncreaseIndentation, main.PasteCommandHandler, main.CanExecuteClipboardCommand));

			//CUSTOM COMMANDS
			main.CommandBindings.Add(new CommandBinding(ProjectNew, main.ProjectNewCommandHandler, main.CanExecuteProjectNewCommand));
			main.CommandBindings.Add(new CommandBinding(ProjectOpen, main.OpenProjectCommandHandler, main.CanExecuteProjectOpenCommand));
			main.CommandBindings.Add(new CommandBinding(ProjectSave, main.ProjectSaveCommandHandler, main.CanExecuteProjectSaveCommand));
			main.CommandBindings.Add(new CommandBinding(ProjectSaveAs, main.ProjectSaveAsCommandHandler, main.CanExecuteProjectSaveAsCommand));
			main.CommandBindings.Add(new CommandBinding(ProjectClose, main.ProjectCloseCommandHandler, main.CanExecuteProjectCloseCommand));

			main.CommandBindings.Add(new CommandBinding(FileAddExisting, main.DeleteFileCommandHandler, main.CanExecuteDeleteFileCommand));

			main.CommandBindings.Add(new CommandBinding(AddAssemblyFromFile, main.AddAssemblyFromFileCommandHandler, main.CanExecuteAddAssemblyCommand));
			main.CommandBindings.Add(new CommandBinding(AddAssemblyFromFolder, main.AddAssemblyFromFolderCommandHandler, main.CanExecuteAddAssemblyCommand));
		}

		///// <summary>
		///// Init all database commands
		///// </summary>
		///// <param name="main"></param>
		///// <param name="target"></param>
		//static public void DatabaseCommandBinding(MainWindow main, DatabaseExplorer target)
		//{
		//    // for DatabaseExplorer
		//    target.CommandBindings.Add(new CommandBinding(DataSourceAdd, target.AddDataSourceCommandHandler, target.CanExecute));
		//    target.CommandBindings.Add(new CommandBinding(DataSourceRefresh, target.RefreshDataSourceCommandHandler, target.CanExecute));
		//    target.CommandBindings.Add(new CommandBinding(DataSourceRemove, target.RemoveDataSourceCommandHandler, target.CanExecute));

		//    //for MainWindow
		//    main.CommandBindings.Add(new CommandBinding(QueryNewEditor, main.QueryNewEditorCommandHandler, main.CanExecuteQueryNewEditor));

		//    main.CommandBindings.Add(new CommandBinding(QueryScriptCreate, main.QueryScriptCreateCommandHandler, main.CanExecuteQueryScript));
		//    main.CommandBindings.Add(new CommandBinding(QueryScriptAlter, main.QueryQueryScriptAlterCommandHandler, main.CanExecuteQueryScript));
		//    main.CommandBindings.Add(new CommandBinding(QueryScriptDrop, main.QueryScriptDropCommandHandler, main.CanExecuteQueryScript));

		//    main.CommandBindings.Add(new CommandBinding(QueryScriptSelect, main.QueryScriptSelectCommandHandler, main.CanExecuteQueryScript));
		//    main.CommandBindings.Add(new CommandBinding(QueryScriptInsert, main.QueryScriptInsertCommandHandler, main.CanExecuteQueryScript));
		//    main.CommandBindings.Add(new CommandBinding(QueryScriptUpdate, main.QueryScriptUpdateCommandHandler, main.CanExecuteQueryScript));
		//    main.CommandBindings.Add(new CommandBinding(QueryScriptDelete, main.QueryScriptDeleteCommandHandler, main.CanExecuteQueryScript));

		//    main.CommandBindings.Add(new CommandBinding(QueryScriptExecute, main.QueryScriptExecuteCommandHandler, main.CanExecuteQueryScriptExecute));

		//    main.CommandBindings.Add(new CommandBinding(QueryViewDependencies, main.QueryViewDependenciesCommandHandler, main.CanExecuteQueryViewDependencies));
		//    main.CommandBindings.Add(new CommandBinding(QueryControlQuality, main.QueryControlQualityCommandHandler, main.CanExecuteQueryControlQuality));
		//}
	}
}
