using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] tests = {
				"../../test/recursion/recursion.3l",
				"../../test/sum/sum.3l",
				"../../test/print/print.3l"
			};

			for ( int i = 0; i < tests.Length; i++) {
				int ind = tests [i].LastIndexOf ('/');
				Console.WriteLine ("Executing test: " + tests [i].Substring (ind + 1));
				Interpreter interp = Interpreter.FromFile (tests[i]);
				interp.Run ();
			}
		}
	}
}
