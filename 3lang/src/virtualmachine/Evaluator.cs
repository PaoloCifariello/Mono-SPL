using System;
using lang.parser;

namespace lang.virtualmachine
{
	public static class Evaluator
	{
		private static VirtualMachine vm;

		public static void Init(VirtualMachine vm)
		{
			Evaluator.vm = vm;
		}

		public static ExpressionValue Evaluate (Expression exp, Environment env)
		{
			if (exp == null)
				return new ExpressionValue (ExpressionValueType.BOOLEAN, false);

			switch (exp.Type) {
			case (ExpressionType.FUNCTION):
				{
					return Evaluator.vm.ExecuteFunction (exp);
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
}