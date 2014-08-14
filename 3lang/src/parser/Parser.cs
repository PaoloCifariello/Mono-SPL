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

			if (exp == null || this.CurrentType != TokenType.L_PAREN)
				return null;

			this.Pop ();

			return new Condition (exp);
		}

		private Expression ParseExpression ()
		{
			throw new NotImplementedException ();
		}

		private Assignment ParseAssignment() 
		{
			if (this.CurrentType == TokenType.DECLARE) {
				this.Pop ();

				if (this.CurrentType != TokenType.ALPHANUMERIC)
					return null;

				string word = this.Pop ().Value;

				if (this.CurrentType == TokenType.COLON) {
					return new Assignment (word);
				} else if (this.CurrentType == TokenType.EQUALS) {
					this.Pop ();
					Expression exp = this.ParseExpression ();

					if (exp == null || this.CurrentType != TokenType.COLON)
						return null;

					return new Assignment (word, exp);
				} else
					return null;
			} else if (this.CurrentType == TokenType.ALPHANUMERIC) {
				string word = this.Pop ().Value;

				if (this.CurrentType != TokenType.EQUALS)
					return null;

				this.Pop ();

				Expression exp = this.ParseExpression ();

				if (exp == null || this.CurrentType != TokenType.COLON)
					return null;

				return new Assignment (word, exp);
			} else
				return null;
		}
	}

	class ParsingError : System.Exception
	{
	}
}

