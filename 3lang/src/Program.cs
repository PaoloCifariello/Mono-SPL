using System;
using lang.interpreter;

namespace lang
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Interpreter i = Interpreter.FromFile ("../../test/prova.3l");
			i.Run ();
		}
	}
}
