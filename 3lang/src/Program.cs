using System;
using lang.interpreter;
using System.Diagnostics;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] tests = {
				//"../../test/sum/sum.3l",
				"../../test/object/object.3l",
				//"../../test/print/print.3l",
				//"../../test/recursion/factorial.3l",
				//"../../test/recursion/fibonacci.3l",
			};

			for ( int i = 0; i < tests.Length; i++) {
				int ind = tests [i].LastIndexOf ('/');
				string name = tests [i].Substring (ind + 1);

				Console.WriteLine ("Executing test: " + name);

				Interpreter interp = Interpreter.FromFile (tests[i]);
				Stopwatch sw = Stopwatch.StartNew();
				interp.Run ();
				sw.Stop ();

				Console.WriteLine ("Test " + name + " executed in " + sw.ElapsedMilliseconds + " ms\n");
			}
		}
	}
}
