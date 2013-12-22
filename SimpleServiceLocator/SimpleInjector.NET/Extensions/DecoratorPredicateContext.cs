﻿#region Copyright (c) 2013 Simple Injector Contributors
/* The Simple Injector is an easy-to-use Inversion of Control library for .NET
 * 
 * Copyright (c) 2013 Simple Injector Contributors
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ or mail to steven at 
 * cuttingedge.it.
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

namespace SimpleInjector.Extensions
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using SimpleInjector.Extensions.Decorators;

    /// <summary>
    /// An instance of this type will be supplied to the <see cref="Predicate{T}"/>
    /// delegate that is that is supplied to the 
    /// <see cref="DecoratorExtensions.RegisterDecorator(Container, Type, Type, Predicate{DecoratorPredicateContext})">RegisterDecorator</see>
    /// overload that takes this delegate. This type contains information about the decoration that is about
    /// to be applied and it allows users to examine the given instance to see whether the decorator should
    /// be applied or not.
    /// </summary>
    /// <remarks>
    /// Please see the 
    /// <see cref="DecoratorExtensions.RegisterDecorator(Container, Type, Type, Predicate{DecoratorPredicateContext})">RegisterDecorator</see>
    /// method for more information.
    /// </remarks>
    [DebuggerDisplay("DecoratorPredicateContext ({DebuggerDisplay,nq})")]
    public sealed class DecoratorPredicateContext
    {
        internal DecoratorPredicateContext(Type serviceType, Type implementationType,
            ReadOnlyCollection<Type> appliedDecorators, Expression expression)
        {
            this.ServiceType = serviceType;
            this.ImplementationType = implementationType;
            this.AppliedDecorators = appliedDecorators;
            this.Expression = expression;
        }

        /// <summary>
        /// Gets the closed generic service type for which the decorator is about to be applied. The original
        /// service type will be returned, even if other decorators have already been applied to this type.
        /// </summary>
        /// <value>The closed generic service type.</value>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the type of the implementation that is created by the container and for which the decorator
        /// is about to be applied. The original implementation type will be returned, even if other decorators
        /// have already been applied to this type. Please not that the implementation type can not always be
        /// determined. In that case the closed generic service type will be returned.
        /// </summary>
        /// <value>The implementation type.</value>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Gets the list of the types of decorators that have already been applied to this instance.
        /// </summary>
        /// <value>The applied decorators.</value>
        public ReadOnlyCollection<Type> AppliedDecorators { get; private set; }

        /// <summary>
        /// Gets the current <see cref="Expression"/> object that describes the intention to create a new
        /// instance with its currently applied decorators.
        /// </summary>
        /// <value>The current expression that is about to be decorated.</value>
        public Expression Expression { get; private set; }
        
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called by the debugger.")]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "ServiceType = {0}, ImplementationType = {1}",
                    this.ServiceType.ToFriendlyName(),
                    this.ImplementationType.ToFriendlyName());
            }
        }

        internal static DecoratorPredicateContext CreateFromInfo(Type serviceType, Expression expression,
            ServiceTypeDecoratorInfo info)
        {
            var appliedDecorators = new ReadOnlyCollection<Type>(
                info.AppliedDecorators.Select(d => d.DecoratorType).ToList());

            return new DecoratorPredicateContext(serviceType, info.ImplementationType, appliedDecorators,
                expression);
        }
    }
}