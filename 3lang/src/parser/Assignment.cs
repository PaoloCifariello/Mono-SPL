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
		private bool global;

		public bool IsGlobal {
			get {
				return this.global;
			}
		}

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

		public Assignment (string variable, bool global)
		{
			this.variable = variable;
			this.global = global;
		}

		public Assignment (string variable, Expression value, bool global) : this(variable, global)
		{
			this.value = value;
		}
	}
}