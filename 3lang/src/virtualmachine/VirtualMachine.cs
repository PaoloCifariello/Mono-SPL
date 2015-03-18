using System;
using lang.parser;
using System.Collections.Generic;
using lang.interpreter;

namespace lang.virtualmachine
{
	public class VirtualMachine
	{
		private Environment env;
		private Evaluator evaluator;

		public Environment Environment {
			get {
				return this.env;
			}
		}

		public VirtualMachine ()
		{
			this.env = new Environment ();
			this.evaluator = new Evaluator (this);
		}

		public void Execute (Program program)
		{
			this.ExecuteStatements (program.Statements);
		}

		private ExpressionValue ExecuteStatements (Statements statements)
		{
			return this.ExecuteStatements (statements, this.env);
		}

		private ExpressionValue ExecuteStatements (Statements statements, Environment env)
		{
			ExpressionValue ret;

			for (int i = 0; i < statements.Length; i++)
				if ((ret = this.ExecuteStatement (statements.GetStatement (i), env)) != null)
					return ret;

			return null;
		}

		private ExpressionValue ExecuteStatement(Statement statement)
		{
			return this.ExecuteStatement (statement, this.env);
		}

		private ExpressionValue ExecuteStatement (Statement statement, Environment env)
		{
			switch (statement.Type) {
			case StatementType.ASSIGN:
				{
					this.Assign (statement.Assignment);
					return null;
				}
			case StatementType.IF_THEN:
				{
					ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						return this.ExecuteStatements (statement.Statement1);
					return null;
				}
			case StatementType.IF_THEN_ELSE:
				{
				ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						return this.ExecuteStatements (statement.Statement1);
					else
						return this.ExecuteStatements (statement.Statement2);
				}
			case StatementType.WHILE:
				{
				ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true) {
						Statements st = new Statements ();
						st.AddStatement (statement.Statement1);
						st.AddStatement (statement);
						return this.ExecuteStatements (st);
					}
					
					return null;
				}
			case StatementType.FUNCTION_DECLARATION:
				{
					this.DeclareFunction (statement.FunctionDeclaration);
					return null;
				}
			case StatementType.FUNCTION:
				{
					return this.ExecuteFunction (statement.Function);
				}
			case StatementType.RETURN:
				{
					return this.evaluator.Evaluate (statement.ReturnValue, this.Environment);
				}
			}

			return null;
		}

		void DeclareFunction (Function function)
		{
			this.env.Declcare (
				function.Identifier, 
				function
				);
		}

		public ExpressionValue ExecuteFunction(Expression fun)
		{
			Function f = null;
			ExpressionValue obj = null;
			ExpressionValue last = null;

			if (fun.AccessKey == null)
				f = this.env.Get (fun.FunctionName) as Function;
			else {
				List<string> accessor = fun.AccessKey;

				obj = last = this.env.Get (accessor [0]) as ExpressionValue;

				for (int i = 1; i < accessor.Count; i++) {
					last = obj;
					obj = obj.GetProperty (accessor [i]);
				}

				f = obj.Function;
			}


			if (f == null) {
				if (this.IsSystemFunction (fun.FunctionName)) {
					return this.ExecuteSystemFunction (fun);
				} else
					return null;
			}


			this.env.PushEnvironment ();
			for (int i = 0; i < fun.Parameters.Count; i++) {
				this.env.Declcare (
					f.ParametersNames [i], 
					this.evaluator.Evaluate (fun.Parameters [i], this.env)
				);
			}


			if (obj != null)
				this.env.Declcare ("this", last);

			ExpressionValue ev = this.ExecuteStatements (f.InnerStatements);
			this.env.PopEnvironment ();
			return ev;
		}

		private void Assign (Assignment assignment)
		{
			if (assignment.IsSimple) {
				if (assignment.IsGlobal)
					this.env.Modify (
						assignment.Variable, 
						this.evaluator.Evaluate (assignment.Value, this.env)
					);
				else
					this.env.Declcare (
						assignment.Variable, 
						this.evaluator.Evaluate (assignment.Value, this.env)
					);
			} else {
				List<string> accessor = assignment.AccesKey;
				ExpressionValue MainObject = this.env.Get (accessor [0]) as ExpressionValue;

				for (int i = 1; i < accessor.Count - 1; i++) {
					 MainObject = MainObject.GetProperty (accessor [i]);
				}

				MainObject.SetProperty (
					accessor [accessor.Count - 1], 
					this.evaluator.Evaluate (assignment.Value, env)
				);
			}
		}

		bool IsSystemFunction (string functionName)
		{
			return (functionName == "print") || (functionName == "require");
		}

		private ExpressionValue ExecuteSystemFunction (Expression fun)
		{
			// Print function
			if (fun.FunctionName == "print") {
				ExpressionValue val = this.evaluator.Evaluate (fun.Parameters [0], this.env);
				if (val.IsNumber) {
					if (float.IsInfinity (val.Number)) {
						Console.WriteLine ("Infinity");
					} else if (float.IsNaN (val.Number)) {
						Console.WriteLine ("NaN");
					} else {
						Console.WriteLine (val.Number);
					}
				} else if (val.IsBool)
					Console.WriteLine (val.Bool);
				else if (val.IsString)
					Console.WriteLine (val.String);
				else if (val.IsFunction)
					Console.WriteLine ("Function " + val.Function.Identifier);
				else if (val.IsObject)
					Console.WriteLine ("Object");
				else 
					Console.WriteLine ("undefined");

				return null;
				// Require function
			} else if (fun.FunctionName == "require") {
				if (fun.Parameters.Count != 1)
					return null;

				ExpressionValue fileName = this.evaluator.Evaluate (fun.Parameters [0], this.env);

				if (!fileName.IsString)
					return null;

				Interpreter i = Interpreter.FromFile (fileName.String);
				i.Init ();
				ExpressionValue v =  i.RunAsModule ();
				return v;
			} else
				return null;
		}
	}
}