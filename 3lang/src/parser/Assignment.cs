using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{
	public class Assignment
	{
		private string variable;
		private Expression value;

		public string Variable {
			get {
				return this.variable;
			}
		}

		public Expression Value {
			get {
				return this.value;
			}
		}

		public Assignment (string variable)
		{
			this.variable = variable;
		}

		public Assignment (string variable, Expression value) : this(variable)
		{
			this.value = value;
		}
	}
}