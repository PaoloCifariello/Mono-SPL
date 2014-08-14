using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//if (args.Length != 1)
			//	return;

			//lexer.Lexer l = lexer.Lexer.FromFile (args [0]);
			Interpreter i = new Interpreter ("if(3=4){var a}");
			i.Run ();
		}
	}
}
