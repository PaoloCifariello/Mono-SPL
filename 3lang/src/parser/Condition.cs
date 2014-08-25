using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{

	public class Condition
	{
		private Expression expression;

		public Expression Expression {
			get {
				return this.expression;
			}
		}

		public Condition (Expression exp)
		{
			this.expression = exp;
		}
	}
	
}