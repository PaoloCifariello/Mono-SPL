using System;
using lang.parser;

namespace lang.virtualmachine
{
	public class VirtualMachine
	{
		private Environment env;

		public VirtualMachine ()
		{
			this.env = new Environment ();
			Evaluator.Init (this);
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
					ExpressionValue value = Evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						this.ExecuteStatements (statement.Statement1);
					break;		
				}
			case StatementType.IF_THEN_ELSE:
				{
				ExpressionValue value = Evaluator.Evaluate (statement.ConditionExpression, this.env);
					if (value.Bool == true)
						this.ExecuteStatements (statement.Statement1);
					else
						this.ExecuteStatements (statement.Statement2);
					break;
				}
			case StatementType.WHILE:
				{
				ExpressionValue value = Evaluator.Evaluate (statement.ConditionExpression, this.env);
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
			Function f = this.env.Get (fun.FunctionName) as Function;

			if (f == null)
				return null;

			this.env.PushEnvironment ();
			for (int i = 0; i < fun.Parameters.Count; i++) {
				this.env.Declcare (
					f.ParametersNames [i], 
					Evaluator.Evaluate (fun.Parameters [i], this.env)
				);
			}

			this.ExecuteStatements (f.InnerStatements);
			ExpressionValue ev = Evaluator.Evaluate (f.ReturnValue, this.env);

			this.env.PopEnvironment ();
			return ev;
		}

		private void Assign (Assignment assignment)
		{
			if (assignment.IsGlobal)
				this.env.Modify (
					assignment.Variable, 
					Evaluator.Evaluate(assignment.Value, this.env)
					);
			else
				this.env.Declcare (
					assignment.Variable, 
					Evaluator.Evaluate(assignment.Value, this.env)
					);
		}
	}
}