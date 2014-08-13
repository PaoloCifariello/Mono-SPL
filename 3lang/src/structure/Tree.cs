using System;
using lang.lexer;

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
		private Node left = null;
		private Node right = null;
		private Token value;

		public Node LeftNode
		{
			get {
				return this.left;
			}
		}
		
		public Node RightNode
		{
			get {
				return this.right;
			}
		}

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

		public Node(Token value, Node left, Node right) : this(value)
		{
			this.left = left;
			this.right = right;
		}
	}
}