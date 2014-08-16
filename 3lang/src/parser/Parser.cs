using System;
using lang.lexer;
using C5;
using lang.structure;

namespace lang.parser
{
	public class Parser
	{
		private ArrayList<Token> tokens;

		private TokenType CurrentType
		{
			get {
				if (this.tokens.Count > 0)
					return this.tokens [0].Type;
				else
					throw new ParsingError ();
			}
		}

		public Parser ()
		{
		}

		private Token Pop ()
		{
			if (this.tokens.Count > 0)
				return this.tokens.RemoveAt (0);
			else
				return null;
		}

		public Program Parse (ArrayList<Token> tokens)
		{
			this.tokens = tokens;

			Program prog = new Program ();
			Statements st = this.ParseStatements ();

			if (st != null)
				prog.AddStatement (st);

			return prog;
		}

		private Statements ParseStatements ()
		{
			Statements statements = new Statements ();
			Statement st;

			while ((st = this.ParseStatement()) != null) {
				statements.AddStatement (st);
			}

			return statements;
		}

		private Statement ParseStatement()
		{
			TokenType type = this.CurrentType;
			if (type == TokenType.LINE_END) {
				this.Pop ();

				try {
					type = this.CurrentType;
				} catch (ParsingError) {
					return null;
				}
			}

			// If then, if then else
			if (type == TokenType.IF) {
				this.Pop ();
				Condition cnd = this.ParseCondition ();

				if (this.CurrentType != TokenType.L_BRACE) 
					return null;

				this.Pop ();
				Statements st1 = this.ParseStatements ();

				if (st1 == null || this.CurrentType != TokenType.R_BRACE)
					return null;

				this.Pop ();

				if (this.CurrentType == TokenType.ELSE) {
					this.Pop ();

					if (this.CurrentType != TokenType.L_BRACE)
						return null;

					this.Pop ();

					Statements st2 = this.ParseStatements ();

					if (this.CurrentType != TokenType.R_BRACE)
						return null;

					this.Pop ();

					return new Statement (StatementType.IF_THEN_ELSE, cnd, st1, st2);
				} else 
					return new Statement (StatementType.IF_THEN, cnd, st1);

			// while
			} else if (type == TokenType.WHILE) {
				this.Pop ();
				Condition cnd = this.ParseCondition ();

				if (this.CurrentType != TokenType.L_BRACE)
					return null;

				this.Pop ();
				Statements st = this.ParseStatements ();

				if (this.CurrentType != TokenType.R_BRACE)
					return null;
				this.Pop ();

				return new Statement (StatementType.WHILE, cnd, st);

			} else {
				Assignment asg = this.ParseAssignment ();
				if (asg == null)
					return null;

				return new Statement (StatementType.ASSIGN, asg);
			}
		}
		
		private Condition ParseCondition()
		{
			if (this.CurrentType != TokenType.L_PAREN)
				return null;

			this.Pop ();
			Expression exp = this.ParseExpression ();

			if (exp == null || this.CurrentType != TokenType.R_PAREN)
				return null;

			this.Pop ();

			return new Condition (exp);
		}

		private Assignment ParseAssignment() 
		{
			if (this.CurrentType == TokenType.DECLARE) {
				this.Pop ();

				if (this.CurrentType != TokenType.ALPHANUMERIC)
					return null;

				string word = this.Pop ().Value;

				if (this.CurrentType == TokenType.SEMI) {
					this.Pop ();
					return new Assignment (word);
				} else if (this.CurrentType == TokenType.ASSIGN) {
					this.Pop ();
					Expression exp = this.ParseExpression ();

					if (exp == null || this.CurrentType != TokenType.SEMI)
						return null;

					this.Pop ();
					return new Assignment (word, exp);
				} else
					return null;
			} else if (this.CurrentType == TokenType.ALPHANUMERIC) {
				string word = this.Pop ().Value;

				if (this.CurrentType != TokenType.ASSIGN)
					return null;

				this.Pop ();

				Expression exp = this.ParseExpression ();

				if (exp == null || this.CurrentType != TokenType.SEMI)
					return null;

				this.Pop ();
				return new Assignment (word, exp);
			} else
				return null;
		}
		
		private Expression ParseExpression ()
		{
			Expression exp1;
			// Case (EXPRESSION)
			if (this.CurrentType == TokenType.L_PAREN) {
				this.Pop ();
				exp1 = this.ParseExpression ();
				if (this.CurrentType != TokenType.R_PAREN)
					return null;

				this.Pop ();
				// Identifier case
			} else if (this.CurrentType == TokenType.ALPHANUMERIC) {
				Token id = this.Pop ();
				char initial = id.Value [0];

				if (!Matcher.isValidInitialIdentifier (initial))
					return null;
				else
					exp1 = new Expression (ExpressionType.IDENTIFIER, id);
				// Integer case
			} else if (this.CurrentType == TokenType.INTEGER) {
				exp1 = new Expression (ExpressionType.INTEGER, this.Pop ());
				// Boolean case
			} else if (this.CurrentType == TokenType.TRUE || this.CurrentType == TokenType.FALSE) {
				exp1 = new Expression (ExpressionType.BOOL, this.Pop ());
				// String case
			} else if (this.CurrentType == TokenType.QUOTE) {
				this.Pop ();

				if (this.CurrentType != TokenType.ALPHANUMERIC)
					return null;

				Token str = this.Pop ();
				if (this.CurrentType != TokenType.QUOTE)
					return null;

				this.Pop ();
				exp1 = new Expression (ExpressionType.STRING, str);
				// Expression combination case
			} else {
				return null;
			}

			if (Parser.IsExpressionOperator (this.CurrentType)) {
				ExpressionType type = Parser.GetExpressionOperator (this.CurrentType);
				this.Pop ();
				Expression exp2 = this.ParseExpression ();
				if (exp2 == null)
					return null;
				else
					return new Expression (type, exp1, exp2);
			} else 
				return exp1;
		}

		public static ExpressionType GetExpressionOperator(TokenType type)
		{
			switch (type)
			{
			case (TokenType.PLUS):
				return ExpressionType.PLUS;
			case (TokenType.MINUS):
				return ExpressionType.MINUS;
			case (TokenType.TIMES):
				return ExpressionType.TIMES;
			case (TokenType.AND):
				return ExpressionType.AND;
			case (TokenType.OR):
				return ExpressionType.OR;
			case (TokenType.DISEQUAL):
				return ExpressionType.DISEQUAL;
			case (TokenType.EQUAL):
				return ExpressionType.EQUAL;
			case (TokenType.LESS):
				return ExpressionType.LESS;
			case (TokenType.LESS_OR_EQUAL):
				return ExpressionType.LESS_OR_EQUAL;
			case (TokenType.GREATER):
				return ExpressionType.GREATER;
			case (TokenType.GREATER_OR_EQUAL):
				return ExpressionType.GREATER_OR_EQUAL;
			default:
				throw new ParsingError ();
			}
		}

		public static bool IsExpressionOperator (TokenType currentType)
		{
			try {
				Parser.GetExpressionOperator(currentType);
				return true;
			} catch (ParsingError) {
				return false;
			}
		}
	}


	class ParsingError : System.Exception
	{
	}
}