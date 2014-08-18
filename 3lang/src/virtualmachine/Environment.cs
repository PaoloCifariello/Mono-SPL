using System;
using System.Collections.Generic;

namespace lang.virtualmachine
{
	public class Environment
	{
		private Dictionary<string, ExpressionValue> env;

		public Environment ()
		{
			this.env = new Dictionary<string, ExpressionValue> ();
		}

		public void Add(string variable, ExpressionValue value)
		{
			ExpressionValue old;
			if (this.env.TryGetValue (variable, out old))
				old.Substitute (value);
			else 
				this.env.Add (variable, value);
		}

		public ExpressionValue Get(string variable)
		{
			ExpressionValue value;

			if (this.env.TryGetValue (variable, out value))
				return value;
			else
				return new ExpressionValue (ExpressionValueType.BOOLEAN, false);
		}
	}
}