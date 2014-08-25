using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{
	public class Function 
	{
		private string Name;
		private ArrayList<string> Parameters;
		private Statements innerStatements;
		private Expression returnValue;
	
		public string Identifier {
			get {
				return this.Name;
			}
		}

		public ArrayList<string> ParametersNames {
			get {
				return this.Parameters;
			}
		}

		public Statements InnerStatements {
			get {
				return this.innerStatements;
			}
		}

		public Expression ReturnValue {
			get {
				return this.returnValue;
			}
		}

		public Function(string name, Statements inner, ArrayList<string> parameters, Expression returnValue)
		{
			this.Name = name;
			this.Parameters = parameters;
			this.innerStatements = inner;
			this.returnValue = returnValue;
		}
	}	
}