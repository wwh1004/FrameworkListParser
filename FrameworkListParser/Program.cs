using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace FrameworkListParser {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine(".NET Framework v3.0");
			Console.WriteLine(string.Join(Environment.NewLine, GetAssemblyFullNames(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\RedistList\FrameworkList.xml").Select(t => $"\"{t}\",")));
			Console.WriteLine();

			Console.WriteLine(".NET Framework v3.5");
			Console.WriteLine(string.Join(Environment.NewLine, GetAssemblyFullNames(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.5\RedistList\FrameworkList.xml").Select(t => $"\"{t}\",")));
			Console.WriteLine();

			Console.ReadKey(true);
		}

		private static IEnumerable<string> GetAssemblyFullNames(string frameworkListPath) {
			var xmlSerializer = new XmlSerializer(typeof(FileList));
			FileList fileList;
			using (var stream = new FileStream(frameworkListPath, FileMode.Open, FileAccess.Read))
				fileList = (FileList)xmlSerializer.Deserialize(stream);
			var assemblyFullNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (var file in fileList.File)
				assemblyFullNames.Add(file.FullName);
			return assemblyFullNames;
		}
	}

	[XmlRoot(ElementName = "File")]
	public class File {
		[XmlAttribute(AttributeName = "AssemblyName")]
		public string AssemblyName { get; set; }
		[XmlAttribute(AttributeName = "Version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "PublicKeyToken")]
		public string PublicKeyToken { get; set; }
		[XmlAttribute(AttributeName = "Culture")]
		public string Culture { get; set; }
		[XmlAttribute(AttributeName = "ProcessorArchitecture")]
		public string ProcessorArchitecture { get; set; }
		[XmlAttribute(AttributeName = "InGAC")]
		public string InGAC { get; set; }

		public string FullName => $"{AssemblyName}, Version={Version}, Culture={Culture}, PublicKeyToken={PublicKeyToken}";
	}

	[XmlRoot(ElementName = "FileList")]
	public class FileList {
		[XmlElement(ElementName = "File")]
		public List<File> File { get; set; }
		[XmlAttribute(AttributeName = "Redist")]
		public string Redist { get; set; }
	}
}
