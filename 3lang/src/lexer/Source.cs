using System;
using System.IO;

namespace lang.lexer
{
	public class Source
	{
		private string source_code;
		private string[] lines;
		private int line_counter = 0;

		public bool Empty 
		{
			get {
				return this.source_code.Length == 0;
			}
		}

		public int Length
		{
			get {
				return this.source_code.Length;
			}

		}

		public int LineCount
		{
			get  {
				return this.lines.Length;
			}
		}

		public Source (string scode)
		{
			this.source_code = scode;
			this.lines = this.source_code.Replace("\r","").Split('\n');
		}

		public string getLine()
		{
			if (this.lines.Length >= this.line_counter)
				return this.lines [this.line_counter++];
			else 
				return null;
		}

		public void Print()
		{
			Console.Write (this.source_code);
		}

		public static Source FromFile(string path)
		{
			StreamReader f = new StreamReader (path);
			string scode = f.ReadToEnd ();
			f.Close ();
			return new Source (scode);
		}
	}
}