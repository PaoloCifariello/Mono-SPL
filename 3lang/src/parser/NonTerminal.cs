using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;

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

	public class Statements
	{
		private List<Statement> statemets;

		public int Length {
			get {
				return this.statemets.Count;
			}
		}

		public Statements ()
		{
			this.statemets = new List<Statement> ();
		}

		public void AddStatement (Statement st)
		{
			this.statemets.Add (st);
		}

		public void AddStatement (Statements st)
		{
			this.statemets.AddRange (st.statemets);
		}

		public Statement GetStatement (int index)
		{
			if (index < this.statemets.Count)
				return this.statemets [index];
			else
				return null;
		}

		public void Print ()
		{
			for (int i = 0; i < this.statemets.Count; i++) {
				Console.Write ("Statement " + i + ": ");
				this.statemets [i].Print ();
		
			}
		}
	}

	public class Statement
	{
		private StatementType type;
		private Condition condition;
		private Statements statements1;
		private Statements statements2;
		private Assignment assignment;

		public StatementType Type {
			get {
				return this.type;
			}
		}

		public Expression ConditionExpression {
			get {
				return this.condition.Expression;
			}
		}

		public Assignment Assignment {
			get {
				return this.assignment;
			}
		}

		public Statements Statement1
		{
			get {
				return this.statements1;
			}
		}

		
		public Statements Statement2
		{
			get {
				return this.statements2;
			}
		}

		private Statement (StatementType type)
		{
			this.type = type;
		}
		// Assignment
		public Statement (StatementType type, Assignment assign) : this(type)
		{
			this.assignment = assign;
		}
		// If then, while
		public Statement (StatementType type, Condition cond, Statements stat1) : this(type)
		{
			this.condition = cond;
			this.statements1 = stat1;
		}
		// If then else
		public Statement (StatementType type, Condition cond, Statements stat1, Statements stat2) : this(type, cond, stat1)
		{
			this.statements2 = stat2;
		}

		public void Print ()
		{
			Console.WriteLine (this.type);
		}
	}

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

	public class Expression
	{
		private ExpressionType type;
		private Token[] tokens;
		private Expression exp1;
		private Expression exp2;

		public ExpressionType Type
		{
			get {
				return this.type;
			}
		}

		public string Value
		{
			get {
				return this.tokens [0].Value;
			}
		}

		public Expression Expression1 {
			get {
				return this.exp1;
			}
		}

		
		public Expression Expression2 {
			get {
				return this.exp2;
			}
		}

		public Expression (ExpressionType type)
		{
			this.type = type;
		}

		public Expression (ExpressionType type, Token token) : this(type)
		{
			this.tokens = new Token[1] {
				token
			};
		}

		public Expression (ExpressionType type, Token[] tokens) : this(type)
		{
			this.tokens = tokens;
		}

		public Expression (ExpressionType type, Expression e1, Expression e2) :this(type)
		{
			this.exp1 = e1;
			this.exp2 = e2;
		}
	}

	public enum StatementType
	{
		IF_THEN,
		IF_THEN_ELSE,
		WHILE,
		ASSIGN}

	;

	public enum ExpressionType
	{
		// Integer
		IDENTIFIER,
		BOOL,
		STRING,
		INTEGER,
		// Operations
		PLUS,
		MINUS,
		TIMES,
		AND,
		OR,
		DISEQUAL,
		EQUAL,
		LESS,
		GREATER,
		LESS_OR_EQUAL,
		GREATER_OR_EQUAL
	}
}

