using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{
	public class Program
	{
		private Statements statements;

		public int Length {
			get {
				return this.statements.Length;
			}
		}

		public Statements Statements {
			get {
				return this.statements;
			}
		}

		public Program ()
		{
			this.statements = new Statements ();
		}

		public Program (Statements statements)
		{
			this.statements = statements;
		}

		public void AddStatement (Statements stat)
		{
			this.statements.AddStatement (stat);

		}

		public void AddStatement (Statement stat)
		{
			this.statements.AddStatement (stat);
		}

		public Statement GetStatement (int index)
		{
			return this.statements.GetStatement (index);
		}

		public void Print ()
		{
			this.statements.Print ();
		}
	}
}