using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] tests = {
				//"../../test/sum/sum.3l",
				"../../test/recursion/recursion.3l"
			};

			for ( int i = 0; i < tests.Length; i++) {
				Interpreter interp = Interpreter.FromFile (tests[i]);
				interp.Run ();
			}
		}
	}
}
