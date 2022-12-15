using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AvalonDock;
using ReflectionStudio.Components.UserControls;

namespace ReflectionStudio.Classes
{
	static public partial class RSCommands
	{
		// for files, added to Application commands
		static public RoutedCommand FileDelete = new RoutedCommand("FileDelete", typeof(MainWindow));
		static public RoutedCommand FileAddExisting = new RoutedCommand("FileAddExisting", typeof(MainWindow));

		//project

		static public RoutedCommand ProjectNew = new RoutedCommand("ProjectNew", typeof(MainWindow));
		static public RoutedCommand ProjectOpen = new RoutedCommand("ProjectOpen", typeof(MainWindow));
		static public RoutedCommand ProjectSave = new RoutedCommand("ProjectSave", typeof(MainWindow));
		static public RoutedCommand ProjectSaveAs = new RoutedCommand("ProjectSaveAs", typeof(MainWindow));
		static public RoutedCommand ProjectClose = new RoutedCommand("ProjectClose", typeof(MainWindow));

		static public RoutedCommand AddAssemblyFromFile = new RoutedCommand("AddAssemblyFromFile", typeof(MainWindow));
		static public RoutedCommand AddAssemblyFromFolder = new RoutedCommand("AddAssemblyFromFolder", typeof(MainWindow));


	}
}
