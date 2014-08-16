using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Interpreter i = new Interpreter ("var a = 10;\nwhile(a >= 0) { a = a - 1;}\n");
			i.Run ();
		}
	}
}
