using System;
using System.Collections.Generic;

namespace lang.lexer
{
	public class Matcher
	{
		private MatchKey[] KeywordMatcher;
		private MatchKey[] SpecialMatcher;

		public Matcher ()
		{
			this.InitializeMatchList ();
		}

		public void InitializeMatchList()
		{
			this.KeywordMatcher = new MatchKey[] {
				new MatchKey(TokenType.VOID, "void"),
				new MatchKey(TokenType.INT, "int"),
				new MatchKey(TokenType.FUNCTION, "function"),
				new MatchKey(TokenType.IF, "if"),
				new MatchKey(TokenType.DECLARE, "var"),
				new MatchKey(TokenType.ELSE, "else"),
				new MatchKey(TokenType.WHILE, "while"),
				new MatchKey(TokenType.FOR, "for"),
				new MatchKey(TokenType.RETURN, "return"),
				new MatchKey(TokenType.TRUE, "true"),
				new MatchKey(TokenType.FALSE, "false"),
				new MatchKey(TokenType.BOOLEAN, "bool"),
				new MatchKey(TokenType.STRING, "string"),
				new MatchKey(TokenType.NEW, "new"),
				new MatchKey(TokenType.NULL, "null")
			};

			this.SpecialMatcher = new MatchKey[] {
				new MatchKey(TokenType.PLUS, "+"),
				new MatchKey(TokenType.MINUS, "-"),
				new MatchKey(TokenType.TIMES, "*"),
				new MatchKey(TokenType.DOT, "."),
				new MatchKey(TokenType.COLON, ":"),
				new MatchKey(TokenType.PERCENT, "%"),
				new MatchKey(TokenType.PIPE, "|"),
				new MatchKey(TokenType.EXCLAMATION, "!"),
				new MatchKey(TokenType.QUESTION, "?"),
				new MatchKey(TokenType.POUND, "#"),
				new MatchKey(TokenType.AMPERSAND, "&"),
				new MatchKey(TokenType.SEMI, ";"),
				new MatchKey(TokenType.COMMA, ","),
				new MatchKey(TokenType.L_PAREN, "("),
				new MatchKey(TokenType.R_PAREN, ")"),
				new MatchKey(TokenType.L_ANG, "<"),
				new MatchKey(TokenType.R_ANG, ">"),
				new MatchKey(TokenType.L_BRACE, "{"),
				new MatchKey(TokenType.R_BRACE, "}"),
				new MatchKey(TokenType.L_BRACKET, "["),
				new MatchKey(TokenType.R_BRACKET, "]"),
				new MatchKey(TokenType.EQUALS, "="),
				new MatchKey(TokenType.LINE_END, "\0")
			};

		}

		public Token Match(string line)
		{
			// Try to match Integer
			if (line [0] >= '0' && line [0] <= '9') {
				int idx = this.ParseInteger (line);

				if (idx == -1)
					return new Token (
						TokenType.INTEGER,
						line.Substring (0)
					);
				else
					return new Token (
						TokenType.INTEGER,
						line.Substring (0, idx)
					);
			}

			// Try to match Keywords
			for (int i = 0; i < this.KeywordMatcher.Length; i++) {
				if (this.KeywordMatcher [i].Match (line)) {

					return new Token (
						this.KeywordMatcher [i].Type,
						null
					);
				}
			}


			// Try to match Special Keywords
			for (int i = 0; i < this.SpecialMatcher.Length; i++) {
				if (this.SpecialMatcher [i].Match (line)) {

					return new Token (
						this.SpecialMatcher [i].Type,
						null
					);
				}
			}

			// Try to match alphanumeric
			if ((line [0] >= 'a' && line [0] <= 'z') || (line [0] >= 'A' && line [0] <= 'Z')) {
				int idx = this.ParseAlphanumeric (line);

				if (idx == -1)
					return new Token (
						TokenType.ALPHANUMERIC,
						line.Substring (0)
					);
				else
					return new Token (
						TokenType.ALPHANUMERIC,
						line.Substring (0, idx)
					);
			}

			return null;
		}

		private int ParseInteger(string line)
		{
			for (int position = 1; position < line.Length; position++) 
				if (!this.isNumber (line [position]))
					return position;

			return -1;
		}

		private int ParseAlphanumeric(string line) 
		{
			for (int position = 1; position < line.Length; position++)
				if (!this.isAlphanumeric (line [position]))
					return position;

			return -1;
		}

		private bool isNumber(char c)
		{
			return (c >= '0' && c <= '9');
		}

		private bool isAlphanumeric(char c)
		{
			if (isNumber (c))
				return true;

			return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '$') || (c == '_'));
		}

	}

	public class MatchKey
	{
		private TokenType type;
		private string word;

		public TokenType Type
		{
			get {
				return this.type;
			}
		}

		public string Word
		{
			get {
				return this.word;
			}
		}

		public int Length
		{
			get {
				if (this.word != null)
					return this.word.Length;
				else
					return 0;
			}
		}

		public MatchKey(TokenType type, string word)
		{
			this.type = type;
			this.word = word;
		}

		public bool Match (string line)
		{
			string substr;

			try {
				substr = line.Substring (0, this.word.Length);
			} catch (ArgumentOutOfRangeException) {
				return false;
			}

			return this.word == substr;
		}
	}
}