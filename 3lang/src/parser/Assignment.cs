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
		private bool simple;
		private List<string> accesKey;

		public bool IsGlobal {
			get {
				return this.global;
			}
		}

		public bool IsSimple {
			get {
				return this.simple;
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

		public List<string> AccesKey {
			get {
				return accesKey;
			}
		}

		public Assignment (string variable, bool global)
		{
			this.variable = variable;
			this.global = global;
			this.simple = true;
		}

		public Assignment (string variable, Expression value, bool global) : this(variable, global)
		{
			this.value = value;
		}

		public Assignment (List<string> accessor, Expression value, bool global)
		{
			this.accesKey = accessor;
			this.value = value;
			this.global = global;
			this.simple = false;
		}
	}
}