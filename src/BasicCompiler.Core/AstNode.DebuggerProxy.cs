﻿namespace BasicCompiler.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <content>
    /// Contains the debugger proxy for an <see cref="AstNode"/>.
    /// </content>
    public partial class AstNode
    {
        /// <summary>
        /// The debugger proxy for an <see cref="AstNode"/>.
        /// </summary>
        private class DebuggerProxy
        {
            private readonly AstNode _node;

            public DebuggerProxy(AstNode node)
            {
                _node = node ?? throw new ArgumentNullException(nameof(node));
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public AstNode[] Children => _node.Children.ToArray();
        }
    }
}
