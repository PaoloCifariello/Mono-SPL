using System;
using lang.parser;
using System.Collections.Generic;

namespace lang.virtualmachine
{
	public class Evaluator
	{
		private VirtualMachine vm;

		public Evaluator(VirtualMachine vm)
		{
			this.vm = vm;
		}

		public ExpressionValue Evaluate (Expression exp, Environment env)
		{
			if (exp == null)
				return new ExpressionValue (ExpressionValueType.BOOLEAN, false);

			switch (exp.Type) {
			case (ExpressionType.FUNCTION):
				{
					return this.vm.ExecuteFunction (exp);
				}
			case (ExpressionType.FUNCTION_DECLARATION):
				{
					return new ExpressionValue (ExpressionValueType.FUNCTION, exp.Function);
				}
			case (ExpressionType.OBJECT):
				{
					return new ExpressionValue (ExpressionValueType.OBJECT);
				}
			case (ExpressionType.OBJECT_ACCESSOR):
				{
					List<string> accessor = exp.AccessKey;
					ExpressionValue v = env.Get (accessor [0]) as ExpressionValue;

					for (int i = 1; i < accessor.Count; i++)
						v = v.GetProperty (accessor [i]);

					return v;
				}
			case (ExpressionType.IDENTIFIER):
				{
					string id = exp.Value;
					return env.Get (id) as ExpressionValue;
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
					return new ExpressionValue (ExpressionValueType.NUMBER, Evaluator.ToInt (exp.Value));
				}
			case (ExpressionType.PLUS):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					if (v1.IsString)
						return new ExpressionValue (ExpressionValueType.STRING, v1.String + v2.String);
					else 
						return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number + v2.Number);
				}

			case (ExpressionType.MINUS):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number - v2.Number);
				}

			case (ExpressionType.TIMES):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number * v2.Number);
				}
			case (ExpressionType.AND):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool && v2.Bool);
				}
			case (ExpressionType.OR):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Bool || v2.Bool);
				}
			case (ExpressionType.EQUAL):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                            (v1.Bool == v2.Bool) && (v1.Number == v2.Number) && (v1.String == v2.String));
				}
			case (ExpressionType.DISEQUAL):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, 
					                            (v1.Bool != v2.Bool) && (v1.Number != v2.Number) && (v1.String != v2.String));
				}
				
			case (ExpressionType.LESS):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.BOOLEAN, v1.Number < v2.Number);
				}
				
			case (ExpressionType.GREATER):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number > v2.Number);
				}
				
			case (ExpressionType.LESS_OR_EQUAL):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number <= v2.Number);
				}
				
			case (ExpressionType.GREATER_OR_EQUAL):
				{
					ExpressionValue v1 = this.Evaluate (exp.Expression1, env);
					ExpressionValue v2 = this.Evaluate (exp.Expression2, env);

					return new ExpressionValue (ExpressionValueType.NUMBER, v1.Number >= v2.Number);
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
}