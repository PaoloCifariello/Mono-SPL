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
				"../../test/math/math.3l",
				"../../test/object/object.3l",
				"../../test/object/function.3l",
				"../../test/print/print.3l",
				"../../test/recursion/factorial.3l",
				"../../test/recursion/fibonacci.3l",
				"../../test/require/require.3l"
			};

//			for ( int i = 0; i < tests.Length; i++) {
//				int ind = tests [i].LastIndexOf ('/');
//				string name = tests [i].Substring (ind + 1);
//
//				Console.WriteLine ("Executing test: " + name);
//
//				Interpreter interp = Interpreter.FromFile (tests[i]);
//				interp.Init ();
//				Stopwatch sw = Stopwatch.StartNew();
//				interp.Run ();
//				sw.Stop ();
//
//				Console.WriteLine ("Test " + name + " executed in " + sw.ElapsedMilliseconds + " ms\n");
//			}

			Interpreter interp = new Interpreter ();
			string nextInput;

			while (true) {
				nextInput = Console.ReadLine ();
				interp.GetNextInput (nextInput);
				interp.Init ();
				interp.Run ();
			}
		}
	}
}
