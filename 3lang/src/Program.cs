using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] tests = {
				"../../test/simple/simple1.3l",
				"../../test/simple/simple1.3l",
				"../../test/simple/simple1.3l",
				"../../test/simple/simple1.3l"
			};

			for ( int i = 0; i < tests.Length; i++) {
				Interpreter interp = Interpreter.FromFile (tests[i]);
				interp.Run ();
			}
		}
	}
}
