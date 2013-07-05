using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVNuPackage
{
	class Program
	{
		static void Main(string[] args)
		{
			var version                 = "";
			var version_check_file      = args[0];
			var nuspec_file             = "";
			var additional_parameters   = "";
			FileVersionInfo versionInfo = null;


			try
			{
				//! Check if given dll or exe file, which should be checked for version, exists
				if (!File.Exists(version_check_file))
					throw new Exception("File for version specificication not found!\r\n[" + version_check_file + "]");
				

				//! Get version and bring it into the correct format
				versionInfo = FileVersionInfo.GetVersionInfo(version_check_file);
				version = versionInfo.FileMajorPart.ToString() + "." + versionInfo.FileMinorPart.ToString() + "." + versionInfo.FileBuildPart.ToString();


				//! Check if .nuspec file is provided via command parameter or not and handle it
				if (args.Length == 1 || (args.Length > 1 && args[1] == ""))
				{
					var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					nuspec_file = Directory.GetFiles(exePath, "*.nuspec").FirstOrDefault();
					if (nuspec_file == null) throw new Exception("No .nuspec file found!\r\n");
				}
				else if (args.Length > 1 && args[1] != "")
				{
					nuspec_file = args[1];

					if (!args[1].Contains("\\"))
						nuspec_file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), args[1]);

					if (!nuspec_file.EndsWith(".nuspec"))
						nuspec_file += ".nuspec";

					if (!File.Exists(nuspec_file))
						throw new Exception(".nuspec file does not exist!\r\n[" + nuspec_file + "]");
				}


				//! Check if additional NuGet parameters are supplied
				if(args.Length > 2)
				{
					if (!args[2].StartsWith(" "))
						args[2] = " " + args[2];

					additional_parameters = args[2];
				}


				//! Execute nuget command line tool with given parameters
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow   = false;
				startInfo.UseShellExecute  = false;
				startInfo.FileName         = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "nuget.exe");
				startInfo.WindowStyle      = ProcessWindowStyle.Hidden;
				startInfo.Arguments        = "pack \"" + nuspec_file + "\" -Version " + version + additional_parameters;


				using (Process exeProcess = Process.Start(startInfo))
				{
					exeProcess.WaitForExit();
				}
			}
			catch (Exception ex)
			{
				File.AppendAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log.txt"), "\r\n\r\n" + DateTime.Now.ToString() + ":\r\n" + ex.Message + "");
			}
		}
	}
}