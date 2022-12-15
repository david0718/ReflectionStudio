using System.Reflection;

namespace ReflectionStudio.Core
{
	static public class PathHelper
	{
		static public string ApplicationPath
		{
			get { return System.IO.Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
			}
		}
	}
}
