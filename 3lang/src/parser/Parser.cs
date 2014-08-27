using System;
using lang.lexer;
using C5;
using lang.structure;
using lang.virtualmachine;

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

		private TokenType NextType {
			get {
				if (this.tokens.Count > 1)
					return this.tokens[1].Type;
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

			if (type == TokenType.FUNCTION) {
				this.Pop ();
				Function fun = this.ParseFunction ();
				return new Statement (StatementType.FUNCTION_DECLARATION, fun);

				// If then, if then else
			} else if (type == TokenType.IF) {
				this.Pop ();
				Condition cnd = this.ParseCondition ();

				if (this.CurrentType != TokenType.L_BRACE) 
					return null;

				this.Pop ();
				Statements st1 = this.ParseStatements ();

				if (this.CurrentType != TokenType.R_BRACE)
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

			// var, alphanumeric or alphanumeric ()
			} else {
				if (this.NextType != TokenType.L_PAREN) {
					Assignment asg = this.ParseAssignment ();
					if (asg == null)
						return null;
					else
						return new Statement (StatementType.ASSIGN, asg);;
				} else {
					Expression fun = this.ParseExpression ();
					if (fun == null)
						return null;
					else {
						if (this.CurrentType != TokenType.SEMI)
							return null;
						this.Pop ();
						return new Statement (StatementType.FUNCTION, fun);
					}

				}
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
					return new Assignment (word, false);
				} else if (this.CurrentType == TokenType.ASSIGN) {
					this.Pop ();
					Expression exp = this.ParseExpression ();

					if (exp == null || this.CurrentType != TokenType.SEMI)
						return null;

					this.Pop ();
					return new Assignment (word, exp, false);
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
				return new Assignment (word, exp, true);
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
				if (this.CurrentType == TokenType.L_PAREN) {
					this.Pop ();

					ArrayList<Expression> parameters = this.ParseFunctionParameters ();

					exp1 = new Expression (ExpressionType.FUNCTION, id.Value, parameters);
				} else 

				//char initial = id.Value [0];

				//if (!Matcher.isValidInitialIdentifier (initial))
				//	return null;
				//else
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

		private Function ParseFunction ()
		{
			if (this.CurrentType != TokenType.ALPHANUMERIC)
				return null;

			string name = this.Pop ().Value;

			if (this.CurrentType != TokenType.L_PAREN)
				return null;

			this.Pop ();
			ArrayList<string> parameters = new ArrayList<string> ();

			while (this.CurrentType == TokenType.ALPHANUMERIC) {
				parameters.Push (this.Pop ().Value);
				if (this.CurrentType == TokenType.COMMA)
					this.Pop ();
			}

			if (this.CurrentType != TokenType.R_PAREN)
				return null;

			this.Pop ();

			if (this.CurrentType != TokenType.L_BRACE)
				return null;

			this.Pop ();

			Statements st = this.ParseStatements ();
			Expression returnValue = null;

			if (this.CurrentType == TokenType.RETURN) {
				this.Pop ();
				returnValue = this.ParseExpression ();
				if (this.CurrentType != TokenType.SEMI)
					return null;

				this.Pop ();
			}

			if (this.CurrentType != TokenType.R_BRACE)
				return null;

			this.Pop ();
			return new Function (name, st, parameters, returnValue);
		}

		ArrayList<Expression> ParseFunctionParameters ()
		{
			ArrayList<Expression> par = new ArrayList<Expression> ();

			while (this.CurrentType != TokenType.R_PAREN) {
				Expression exp = this.ParseExpression ();
				if (exp != null)
					par.Push (exp);

				if (this.CurrentType == TokenType.COMMA)
					this.Pop ();
			}

			this.Pop ();

			return par;
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