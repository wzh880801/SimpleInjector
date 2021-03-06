﻿#region Copyright Simple Injector Contributors
/* The Simple Injector is an easy-to-use Inversion of Control library for .NET
 * 
 * Copyright (c) 2013-2016 Simple Injector Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace SimpleInjector.Extensions.LifetimeScoping
{
    using System;
    using SimpleInjector.Advanced;

    /// <summary>
    /// Thread and container specific cache for services that are registered with one of the 
    /// <see cref="SimpleInjectorLifetimeScopeExtensions">RegisterLifetimeScope</see> extension method overloads.
    /// </summary>
    internal sealed class LifetimeScope : Scope
    {
        private readonly LifetimeScopeManager manager;

        internal LifetimeScope(LifetimeScopeManager manager, LifetimeScope parentScope) : base(manager.Container)
        {
            this.manager = manager;
            this.ParentScope = parentScope;
        }

        internal LifetimeScope ParentScope { get; }

        internal bool Disposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged 
        /// resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release 
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            finally
            {
                this.Disposed = true;
                this.manager.RemoveLifetimeScope(this);
            }
        }
    }
}