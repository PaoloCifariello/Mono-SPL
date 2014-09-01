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

		private void ExecuteStatements (Statements statements)
		{
			this.ExecuteStatements (statements, this.env);
		}

		private void ExecuteStatements (Statements statements, Environment env)
		{
			for (int i = 0; i < statements.Length; i++)
				this.ExecuteStatement (statements.GetStatement (i), env);
		}

		private void ExecuteStatement(Statement statement)
		{
			this.ExecuteStatement (statement, this.env);
		}

		private void ExecuteStatement (Statement statement, Environment env)
		{
			switch (statement.Type) {
			case StatementType.ASSIGN:
				{
					this.Assign (statement.Assignment);
					break;
				}
			case StatementType.IF_THEN:
				{
					ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						this.ExecuteStatements (statement.Statement1);
					break;		
				}
			case StatementType.IF_THEN_ELSE:
				{
				ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						this.ExecuteStatements (statement.Statement1);
					else
						this.ExecuteStatements (statement.Statement2);
					break;
				}
			case StatementType.WHILE:
				{
				ExpressionValue value = this.evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true) {
						Statements st = new Statements ();
						st.AddStatement (statement.Statement1);
						st.AddStatement (statement);
						this.ExecuteStatements (st);
					}
					break;
				}
			case StatementType.FUNCTION_DECLARATION:
				{
					this.DeclareFunction (statement.FunctionDeclaration);
					break;
				}
			case StatementType.FUNCTION:
				{
					this.ExecuteFunction (statement.Function);
					break;
				}
			}
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

			if (fun.AccessKey == null)
				f = this.env.Get (fun.FunctionName) as Function;
			else {
				List<string> accessor = fun.AccessKey;

				ExpressionValue obj = this.env.Get (accessor [0]) as ExpressionValue;

				for (int i = 1; i < accessor.Count; i++)
					obj = obj.GetProperty (accessor [i]);

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

			this.ExecuteStatements (f.InnerStatements);
			ExpressionValue ev = this.evaluator.Evaluate (f.ReturnValue, this.env);

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
				if (val.IsInt)
					Console.WriteLine (val.Number);
				else if (val.IsBool)
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
				ExpressionValue v =  i.RunAsModule ();
				return v;
			} else
				return null;
		}
	}
}