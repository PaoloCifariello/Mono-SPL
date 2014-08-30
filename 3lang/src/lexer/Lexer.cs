using System;
using System.Collections;
using C5;

namespace lang.lexer
{
	public class Lexer
	{
		private Source source;
		private ArrayList<Token> tokens;
		private Matcher matcher;

		private string CurrentLine;

		public ArrayList<Token> Tokens
		{
			get {
				return this.tokens;
			}
		}

		private Lexer()
		{
			this.tokens = new ArrayList<Token> ();
			this.matcher = new Matcher ();
		}

		private Lexer(Source src) : this()
		{
			this.source = src;
		}

		public Lexer(string scode) : this()
		{
			this.source = new Source (scode);
		}

		public static Lexer FromFile (string path)
		{
			Source src = Source.FromFile (path);
			return new Lexer (src);
		}

		private bool AddNextToken()
		{
			this.SkipEmptyChar ();

			if (this.CurrentLine.Length == 0)
				return false;

			Token CurrentToken = this.matcher.Match (CurrentLine);

			if (CurrentToken == null)
				return false;

			tokens.Push (CurrentToken);
			int length = CurrentToken.Length;
			this.CurrentLine = this.CurrentLine.Substring (length);

			// string case
			if (CurrentToken.Type == TokenType.QUOTE) {
				string value = this.ParseString ();
				if (value == null)
					return false;

				this.tokens.Push (new Token (TokenType.ALPHANUMERIC, value));
				this.tokens.Push (new Token (TokenType.QUOTE));
			}

			return true;
		}

		private string ParseString ()
		{
			int ind = this.CurrentLine.IndexOf ('"');
			if (ind == -1)
				return null;

			string val = this.CurrentLine.Substring (0, ind);
			this.CurrentLine = this.CurrentLine.Substring (ind);

			// Looking for second quote
			if (this.CurrentLine [0] != '"')
				return null;

			this.CurrentLine = this.CurrentLine.Substring (1);
			return val;
		}

		private void SkipEmptyChar ()
		{
			int position = 0;

			while (position < this.CurrentLine.Length) {
				char c = this.CurrentLine [position];

				if (c == ' ' || c == '\t' || c == '\r')
					position++;
				else {
					this.CurrentLine = this.CurrentLine.Substring (position);
					break;
				}
			}
		}

		public void Tokenize() 
		{
			// Gets tokens for each line.
			for (int i = 0; i < this.source.LineCount; i++) {
				this.CurrentLine = this.source.getLine ();

				while (this.AddNextToken());

				tokens.Push (
					new Token (TokenType.LINE_END)
					);
			}
		}

		public void PrintToken(int index)
		{
			if (index < this.tokens.Count)
				Console.Write (this.tokens [index].ToString());
		}

		public void PrintToken()
		{
			for (int i = 0; i < this.tokens.Count; i++)
				this.PrintToken(i);
		}
	}
}