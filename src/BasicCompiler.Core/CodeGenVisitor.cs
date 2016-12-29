﻿namespace BasicCompiler.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class CodeGenVisitor : IAstVisitor
    {
        private readonly StringBuilder _sb;
        private readonly Stack<bool> _visitingFirstArgs;

        internal CodeGenVisitor()
        {
            _sb = new StringBuilder();
            _visitingFirstArgs = new Stack<bool>();
        }

        public void LeaveCallExpression(AstNode node)
        {
            if (_sb.Length == 0)
            {
                throw new AstVisitorException("Leaving CallExpression, but nothing has been appended to the StringBuilder yet.");
            }

            if (_visitingFirstArgs.Pop() ^ _sb[_sb.Length - 1] == '(')
            {
                throw new AstVisitorException("The last character should be an open parenthesis iff there are no arguments.");
            }

            _sb.Append(')');

            // If this CallExpression is the first argument to another CallExpression, e.g.
            // (multiply (divide 9 111) 0), we want to make sure that we update the status of
            // `_visitingFirstArgs` appropriately for the next level up in the stack.
            // Note: If we're the top-level CallExpression, the stack will be empty, in which
            // case we don't have to do anything.
            if (_visitingFirstArgs.Count > 0)
            {
                _visitingFirstArgs.Pop();
                _visitingFirstArgs.Push(false);
            }
        }

        public void LeaveExpressionStatement(AstNode node)
        {
            if (_sb.Length == 0 || _sb[_sb.Length - 1] != ')')
            {
                throw new AstVisitorException("Did not find closing parenthesis after leaving CallExpression.");
            }

            _sb.Append(';');
        }

        public void VisitCallExpression(AstNode node)
        {
            _sb.Append(node.Value).Append('(');
            _visitingFirstArgs.Push(true);
        }

        public void VisitExpressionStatement(AstNode node)
        {
            // No-op. Everything will be taken care of by VisitCallExpression.
        }

        public void VisitNumberLiteral(AstNode node)
        {
            // TODO: Extend check to non-first arguments.
            if (_visitingFirstArgs.Peek() && (_sb.Length == 0 || _sb[_sb.Length - 1] != '('))
            {
                throw new AstVisitorException("Visiting a NumberLiteral node when not in a CallExpression?");
            }

            if (!_visitingFirstArgs.Peek())
            {
                _sb.Append(", ");
            }
            else
            {
                _visitingFirstArgs.Pop();
                _visitingFirstArgs.Push(false);
            }

            _sb.Append(node.Value);
        }

        public override string ToString() => _sb.ToString();
    }
}