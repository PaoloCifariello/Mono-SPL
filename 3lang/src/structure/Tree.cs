using System;
using lang.lexer;
using System.Collections.Generic;

namespace lang.structure
{
	public class Tree
	{
		private Node root;
		private bool empty = true;

		public bool Empty
		{
			get {
				return this.empty;
			}
		}

		public Tree (Node root)
		{
			this.root = root;
			this.empty = false;
		}

		public Tree ()
		{
		}
	}

	public class Node
	{
		private List<Node> children;
		private Token value;

		public Token Value
		{
			get {
				return this.value;
			}
		}

		public Node(Token value)
		{
			this.value = value;
		}

		public Node(Token value, Node[] children) : this(value)
		{
			this.children.AddRange (children);
		}

		public Node GetChild(int index)
		{
			if (index < this.children.Count)
				return this.children [index];
			else
				return null;
		}
	}
}