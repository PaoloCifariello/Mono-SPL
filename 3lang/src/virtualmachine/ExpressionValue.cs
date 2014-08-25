using System;
using lang.parser;

namespace lang.virtualmachine
{
	public class ExpressionValue
	{
		private ExpressionValueType type;
		private int iValue = 0;
		private bool bValue = false;
		private string sValue = "";
		private Function fValue = new Function("", new Statements(), new C5.ArrayList<string>());

		public bool IsBool {
			get {
				return this.type == ExpressionValueType.BOOLEAN;
			}
		}

		public bool IsInt {
			get {
				return this.type == ExpressionValueType.INTEGER;
			}
		}

		public bool IsString {
			get {
				return this.type == ExpressionValueType.STRING;
			}
		}

		public bool IsFunction {
			get {
				return this.type == ExpressionValueType.FUNCTION;
			}
		}

		public bool Bool {
			get {
				return this.bValue;
			}
		}

		public int Int {
			get {
				return this.iValue;
			}
		}

		public string String {
			get {
				return this.sValue;
			}
		}

		public Function Function {
			get {
				return this.fValue;
			}
		}

		private ExpressionValue (ExpressionValueType type)
		{
			this.type = type;
		}

		public ExpressionValue (ExpressionValueType type, string value) : this(type)
		{
			this.sValue = value;
			this.bValue = (value != "");
			this.iValue = (value != "") ? 1 : 0;
		}

		public ExpressionValue (ExpressionValueType type, int value) : this(type)
		{
			this.sValue = (value != 0) ? "_" : "";
			this.bValue = (value != 0) ? true : false;
			this.iValue = value;
		}

		public ExpressionValue (ExpressionValueType type, bool value) : this(type)
		{
			this.sValue = (value) ? "_" : "";
			this.bValue = value;
			this.iValue = (value) ? 1 : 0;
		}

		public ExpressionValue (ExpressionValueType type, Function value) : this(type)
		{
			this.sValue = "_";
			this.bValue = true;
			this.iValue = 1;
			this.fValue = value;
		}

		public void Substitute (ExpressionValue value)
		{
			this.bValue = value.Bool;
			this.iValue = value.Int;
			this.sValue = value.String;
		}
	}
	
}