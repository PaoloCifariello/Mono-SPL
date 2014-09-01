using System;
using lang.lexer;
using lang.parser;
using lang.structure;
using lang.virtualmachine;
using System.IO;

namespace lang.interpreter
{
	public class Interpreter
	{
		private Lexer lexer;
		private Parser parser;
		private VirtualMachine vm;

		private Interpreter()
		{
			this.parser = new Parser ();
			this.vm = new VirtualMachine ();
		}

		private Interpreter(Lexer l) : this()
		{
			this.lexer = l;
		}

		public Interpreter (string scode) : this()
		{
			this.lexer = new Lexer (scode);
		}

		public static Interpreter FromFile(string path)
		{
			Directory.SetCurrentDirectory (
				Directory.GetCurrentDirectory () + 
				"/" + 
				path.Substring (0, path.LastIndexOf ('/'))
			);
			return new Interpreter (Lexer.FromFile (path));
		}

		public void Run()
		{
			this.lexer.Tokenize ();
			//this.lexer.PrintToken ();

			Program program = this.parser.Parse (this.lexer.Tokens);
			if (program == null)
				Console.WriteLine ("Parsing error");

			this.vm.Execute (program);
		}

		public ExpressionValue RunAsModule ()
		{
			this.lexer.Tokenize ();
			//this.lexer.PrintToken ();

			Program program = this.parser.Parse (this.lexer.Tokens);
			if (program == null)
				Console.WriteLine ("Parsing error");

			this.vm.Environment.Modify ("exports", new ExpressionValue (ExpressionValueType.OBJECT));
			this.vm.Execute (program);
			return this.vm.Environment.Get ("exports") as ExpressionValue;
		}
	}
}