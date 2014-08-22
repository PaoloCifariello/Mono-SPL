using System;
using System.Collections.Generic;
using lang.parser;

namespace lang.virtualmachine
{
	public class Environment
	{
		private List<Dictionary<string, Object>> env;

		public Dictionary<string, Object> EnvironmentAssociation {
			get {
				return this.env[this.env.Count - 1];
			}
		}

		public Environment ()
		{
			this.env = new List<Dictionary<string, Object>> ();
			this.PushEnvironment ();
		}

		public void PushEnvironment ()
		{
			this.env.Add (new Dictionary<string, object> ());
		}

		public void PopEnvironment()
		{
			if (this.env.Count > 1)
				this.env.RemoveAt (this.env.Count - 1);
		}

		public void Add(string variable, Object value)
		{
			Object old;
			bool found = false;

			for (int i = this.env.Count - 1; i > 0; i--)
				if (this.env[i].TryGetValue (variable, out old)) {
					this.env [i].Remove (variable);
					this.env [i].Add (variable, value);
					found = true;
					break;
				}
			if (!found)
				this.env[this.env.Count - 1].Add (variable, value);
		}

		public Object Get(string variable)
		{
			Object value;

			for (int i = this.env.Count - 1; i >= 0; i--)
				if (this.env[i].TryGetValue (variable, out value)) {
					return value;
			}

			return null;
		}
	}
}