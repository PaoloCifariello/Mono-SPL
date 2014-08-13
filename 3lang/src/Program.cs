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
			Interpreter i = new Interpreter ("var asd=0231234;function a () { return 3 ; };");
			i.Run ();
		}
	}
}
