using System;
using lang.lexer;
using lang.parser;
using lang.structure;

namespace lang.interpreter
{
	public class Interpreter
	{
		private Lexer lexer;
		private Parser parser;

		private Interpreter()
		{
			this.parser = new Parser ();
		}

		private Interpreter(Lexer l)
		{
			this.lexer = l;
		}

		public Interpreter (string scode) : this()
		{
			this.lexer = new Lexer (scode);
		}

		public static Interpreter FromFile(string path)
		{
			return new Interpreter (Lexer.FromFile (path));
		}

		public void Run()
		{
			this.lexer.Tokenize ();
			Tree ParseTree = this.parser.Parse (this.lexer.Tokens);

			this.lexer.PrintToken ();
		}
	}
}