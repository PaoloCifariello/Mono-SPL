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
		}

		public void Execute (Program program)
		{
			this.ExecuteStatements (program.Statements);
		}

		private void ExecuteStatements (Statements statements)
		{
			for (int i = 0; i < statements.Length; i++)
				this.ExecuteStatement (statements.GetStatement (i));
		}

		private void ExecuteStatement (Statement statement)
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
			}
		}

		private void Assign (Assignment assignment)
		{
			this.env.Add (assignment.Variable, Evaluator.Evaluate(assignment.Value, this.env));
		}
	}
}