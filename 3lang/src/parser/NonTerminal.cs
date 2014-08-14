using System;
using System.Collections.Generic;
using lang.lexer;

namespace lang.parser
{
	public class Program
	{
		private Statements statements;

		public Program () {}

		public Program (Statements statements)
		{
			this.statements = statements;
		}

		
		public void AddStatement(Statements stat)
		{
			this.statements.AddStatement (stat);

		}

		public void AddStatement(Statement stat)
		{
			this.statements.AddStatement (stat);
		}
	}
	
	public class Statements
	{
		private List<Statement> statemets;

		public int Length 
		{
			get {
				return this.statemets.Count;
			}
		}

		public Statements ()
		{
			this.statemets = new List<Statement> ();
		}

		public void AddStatement(Statement st)
		{
			this.statemets.Add (st);
		}

		public void AddStatement(Statements st)
		{
			this.statemets.AddRange (st.statemets);
		}

		public Statement GetStatement(int index)
		{
			if (index < this.statemets.Count)
				return this.statemets [index];
			else
				return null;
		}
	}
	
	public class Statement
	{
		private StatementType type;
		private Condition condition;
		private Statements statements1;
		private Statements statements2;
		private Assignment assignment;

		private Statement (StatementType type)
		{
			this.type = type;
		}

		// Assignment
		public Statement(StatementType type, Assignment assign) : this(type)
		{
			this.assignment = assign;
		}

		// If then, while
		public Statement(StatementType type, Condition cond, Statements stat1) : this(type)
		{
			this.condition = cond;
			this.statements1 = stat1;
		}

		// If then else
		public Statement(StatementType type, Condition cond, Statements stat1, Statements stat2) : this(type, cond, stat1)
		{
			this.statements2 = stat2;
		}
	}
	
	public class Condition
	{
		private Expression expression;

		public Condition (Expression exp)
		{
			this.expression = exp;
		}
	}
	
	public class Assignment
	{
		private string variable;
		private Expression value;

		public Assignment (string variable)
		{
			this.variable = variable;
		}

		public Assignment(string variable, Expression value) : this(variable)
		{
			this.value = value;
		}
	}
	
	public class Expression
	{
		private ExpressionType type;
		private Token[] tokens;

		public Expression (ExpressionType type)
		{
			this.type = type;
		}

		public Expression(ExpressionType type, Token[] tokens) : this(type)
		{
			this.tokens = tokens;
		}
	}

	public enum StatementType {
		IF_THEN,
		IF_THEN_ELSE,
		WHILE,
		ASSIGN
	};

	public enum ExpressionType {
		// Integer
		INTEGER,
		PLUS,
		MINUS,
		TIMES,
		// Boolean
		BOOL,
		AND,
		OR,
		// String
		STRING,
		CONCAT
	}
}

