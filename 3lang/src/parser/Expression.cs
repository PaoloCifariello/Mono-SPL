using System;
using System.Collections.Generic;
using lang.lexer;
using lang.virtualmachine;
using C5;

namespace lang.parser
{
	public enum ExpressionType
	{
		OBJECT_ACCESSOR,

		OBJECT,

		FUNCTION,

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

	public class Expression
	{
		private ExpressionType type;
		private Token[] tokens;
		private Expression exp1;
		private Expression exp2;
		private string functionName;
		private ArrayList<Expression> parameters;
		private List<string> accessorKey;

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

		public string FunctionName {
			get {
				return this.functionName;
			}
		}

		public ArrayList<Expression> Parameters {
			get {
				return this.parameters;
			}
		}

		public List<string> AccessKey {
			get {
				return this.accessorKey;
			}
		}

		public Expression (ExpressionType type)
		{
			this.type = type;
		}

		public Expression(ExpressionType type, List<string> accessor ) : this(type)
		{
			this.accessorKey = accessor;
		}

		public Expression (ExpressionType type, string id, ArrayList<Expression> parameters) : this(type)
		{
			this.functionName = id;
			this.parameters = parameters;
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
}