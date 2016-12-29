﻿namespace BasicCompiler.Core
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Ast : IEquatable<Ast>
    {
        public Ast(AstNode root)
        {
            // TODO: This is a temp workaround for the CLI apparently not supporting
            // C# 7 yet. Once it does, write this as an arrow expression.
            Root = root;
        }

        public AstNode Root { get; }

        internal string DebuggerDisplay => $"Root: {{ {Root.DebuggerDisplay} }}";

        public void Accept(IAstVisitor visitor) => Root.Accept(visitor);

        public override bool Equals(object obj) => obj is Ast ast && Equals(ast);

        // TODO: Add equality operators to both classes.
        public bool Equals(Ast other) => other?.Root != null && Root.Equals(other.Root);

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
