using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{
	public enum StatementType
	{
		FUNCTION,

		FUNCTION_DECLARATION,
		IF_THEN,
		IF_THEN_ELSE,
		WHILE,
		ASSIGN,

		RETURN
	};

	public class Statement
	{
		private StatementType type;
		private Condition condition;
		private Statements statements1;
		private Statements statements2;
		private Assignment assignment;
		private Expression function;
		private Function functionDeclaration;
		private Expression returnValue;

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

		public Expression Function {
			get {
				return this.function;
			}
		}

		public Function FunctionDeclaration {
			get {
				return this.functionDeclaration;
			}
		}

		public Expression ReturnValue {
			get {
				return this.returnValue;
			}
		}

		private Statement (StatementType type)
		{
			this.type = type;
		}

		// Function
		public Statement(StatementType type, Expression fun) : this(type)
		{
			if (type == StatementType.RETURN)
				this.returnValue = fun;
			else
				this.function = fun;
		}

		// If then else
		public Statement (StatementType type, Function fun) : this(type)
		{
			this.functionDeclaration = fun;
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
}