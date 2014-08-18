using System;
using lang.parser;

namespace lang.virtualmachine
{
	public static class Evaluator
	{
		public static ExpressionValue Evaluate (Expression exp, Environment env)
		{
			switch (exp.Type) {
			case (ExpressionType.IDENTIFIER):
				{
					string id = exp.Value;
					return env.Get (id);
				}
			case (ExpressionType.BOOL):
				{
					return new ExpressionValue (ExpressionValueType.BOOLEAN, Evaluator.ToBool (exp.Value));
				}
			case (ExpressionType.STRING):
				{
					return new ExpressionValue (ExpressionValueType.STRING, exp.Value);
				}
			case (ExpressionType.INTEGER):
				{
					return new ExpressionValue (ExpressionValueType.INTEGER, Evaluator.ToInt (exp.Value));
				}
			case (ExpressionType.PLUS):
				{
					ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int + v2.Int);
				}

			case (ExpressionType.MINUS):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int - v2.Int);
				}

			case (ExpressionType.TIMES):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int * v2.Int);
				}
			case (ExpressionType.AND):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool && v2.Bool);
				}
			case (ExpressionType.OR):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool || v2.Bool);
				}
			case (ExpressionType.EQUAL):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                           (v1.Bool == v2.Bool) && (v1.Int == v2.Int) && (v1.String == v2.String));
				}
			case (ExpressionType.DISEQUAL):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                           (v1.Bool != v2.Bool) && (v1.Int != v2.Int) && (v1.String != v2.String));
			}
				
			case (ExpressionType.LESS):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Int < v2.Int);
				}
				
			case (ExpressionType.GREATER):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int > v2.Int);
				}
				
			case (ExpressionType.LESS_OR_EQUAL):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int <= v2.Int);
				}
				
			case (ExpressionType.GREATER_OR_EQUAL):
				{
				ExpressionValue v1 = Evaluator.Evaluate (exp.Expression1, env);
				ExpressionValue v2 = Evaluator.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.INTEGER, v1.Int >= v2.Int);
				}
			default:
				return null;
			}
		}

		public static int ToInt (string value)
		{
			return int.Parse (value);
		}

		public static bool ToBool (string value)
		{
			return bool.Parse (value);
		}
	}

	public class ExpressionValue
	{
		private ExpressionValueType type;
		private int iValue = 0;
		private bool bValue = false;
		private string sValue = "";

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

		public void Substitute (ExpressionValue value)
		{
			this.bValue = value.Bool;
			this.iValue = value.Int;
			this.sValue = value.String;
		}
	}

	public enum ExpressionValueType
	{
		STRING,
		INTEGER,
		BOOLEAN
	}
}