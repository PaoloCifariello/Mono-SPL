using System;

namespace lang.lexer
{
	public class Token
	{
		private TokenType type;
		private string value;

		public int Length
		{
			get {
				if (this.value != null)
					return this.value.Length;

				switch (this.type) {
				case TokenType.PLUS:
				case TokenType.MINUS:
				case TokenType.TIMES:
				case TokenType.DOT:
				case TokenType.COLON:
				case TokenType.PERCENT:
				case TokenType.QUESTION:
				case TokenType.POUND:
				case TokenType.SEMI:
				case TokenType.COMMA:
				case TokenType.L_PAREN:
				case TokenType.R_PAREN:
				case TokenType.LESS:
				case TokenType.GREATER:
				case TokenType.L_BRACE:
				case TokenType.R_BRACE:
				case TokenType.L_BRACKET:
				case TokenType.R_BRACKET:
				case TokenType.ASSIGN:
				case TokenType.QUOTE:
					return 1;
				case TokenType.EQUAL:
				case TokenType.LESS_OR_EQUAL:
				case TokenType.GREATER_OR_EQUAL:
				case TokenType.IF:
				case TokenType.OR:
				case TokenType.DISEQUAL:
				case TokenType.AND:
					return 2;
				//case TokenType.INT:
				case TokenType.DECLARE:
				case TokenType.FOR:
				case TokenType.NEW:
					return 3;
				//case TokenType.VOID:
				case TokenType.ELSE:
				case TokenType.TRUE:
				case TokenType.NULL:
					return 4;
				case TokenType.WHILE:
				case TokenType.FALSE:
					return 5;
				case TokenType.RETURN:
				case TokenType.STRING:
					return 6;
				//case TokenType.BOOLEAN:
					//return 7;
				case TokenType.FUNCTION:
					return 8;
				}

				return 0;
			
			}
		}

		public TokenType Type 
		{
			get {
				return this.type;
			}
		}

		public string Value
		{
			get {
				return this.value;
			}
		}

		public Token (TokenType type, string value)
		{
			this.type = type;
			this.value = value;
		}

		public override string ToString()
		{
			string str = this.type.ToString();

			if (this.value != null)
				str += " : " + this.value;

			return str + "\n";
		}
	}


	public enum TokenType {
		INTEGER,
		DOUBLE,
		ALPHANUMERIC,
		STRING,

		// Keywords
		FUNCTION,		// function
		IF,				// if
		DECLARE,		// var
		ELSE,			// else
		WHILE,			// while
		FOR,			// for
		RETURN,			// return
		TRUE,			// true
		FALSE,			// false
		NEW,			// new
		NULL,			// null

		// Special characters
		PLUS,			// +
		MINUS,			// -
		TIMES,			// *
		QUOTE,			// "
		DOT,			// .
		COLON,			// :
		PERCENT,		// %
		DISEQUAL,		// !=
		QUESTION,		// ?
		POUND,			// #
		AND,			// &&
		OR,				// ||
		LESS_OR_EQUAL,	// <=
		GREATER_OR_EQUAL,// >=
		LESS,			// <
		GREATER,		// >
		SEMI,			// ;
		COMMA,			// ,
		L_PAREN,		// (
		R_PAREN,		// )
		L_BRACE,		// {
		R_BRACE,		// }
		L_BRACKET,		// [
		R_BRACKET,		// ]
		ASSIGN,			// =
		EQUAL,			// ==
		LINE_END,		// \n

		END				// End of source code
	};
}