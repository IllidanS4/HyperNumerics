﻿using IS4.HyperNumerics.Operations;
using System;
using System.Collections;
using System.Collections.Generic;

using static IS4.HyperNumerics.HyperMath;

namespace IS4.HyperNumerics.NumberTypes
{
    /// <summary>
    /// A utility number type modifying <typeparamref name="TInner"/> whose operations
    /// appear as if the number was transformed to another value and then back
    /// when the operation is performed.
    /// </summary>
    /// <typeparam name="TInner">The inner type.</typeparam>
    /// <typeparam name="TTransformation">A type implementing <see cref="ITransformation"/> which is constructed once for every number type and used for the transformations.</typeparam>
    [Serializable]
    public readonly partial struct TransformedNumber<TInner, TTransformation> : IWrapperNumber<TransformedNumber<TInner, TTransformation>, TInner>, INumber<TransformedNumber<TInner, TTransformation>, TInner>, INumber<TInner> where TInner : struct, INumber<TInner> where TTransformation : struct, TransformedNumber<TInner, TTransformation>.ITransformation
    {
        static TTransformation transform = default;

        readonly TInner value;

        public TInner Value => transform.TransformOutput(value);

        public bool IsInvertible => CanInv(value);

        public bool IsFinite => IsFin(value);

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;
        
        public TransformedNumber(in TInner value)
        {
            this.value = transform.TransformInput(value);
        }

        private TransformedNumber(in TInner value, object marker)
        {
            this.value = value;
        }

        public TransformedNumber<TInner, TTransformation> Clone()
        {
            return new TransformedNumber<TInner, TTransformation>(HyperMath.Clone(value), null);
        }

        TInner INumber<TInner>.Clone()
        {
            return transform.TransformOutput(HyperMath.Clone(value));
        }

        public TransformedNumber<TInner, TTransformation> Call(StandardBinaryOperation operation, in TransformedNumber<TInner, TTransformation> other)
        {
            return new TransformedNumber<TInner, TTransformation>(HyperMath.Call(operation, value, other.value), null);
        }

        public TransformedNumber<TInner, TTransformation> Call(StandardBinaryOperation operation, in TInner other)
        {
            return new TransformedNumber<TInner, TTransformation>(transform.TransformInput(other).CallReversed(operation, value), null);
        }

        public TransformedNumber<TInner, TTransformation> CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new TransformedNumber<TInner, TTransformation>(transform.TransformInput(other).Call(operation, value), null);
        }

        TInner INumber<TInner>.Call(StandardBinaryOperation operation, in TInner other)
        {
            return transform.TransformOutput(transform.TransformInput(other).CallReversed(operation, value));
        }

        TInner INumber<TInner>.CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return transform.TransformOutput(transform.TransformInput(other).Call(operation, value));
        }

        public TransformedNumber<TInner, TTransformation> Call(StandardUnaryOperation operation)
        {
            return new TransformedNumber<TInner, TTransformation>(HyperMath.Call(operation, value), null);
        }

        TInner INumber<TInner>.Call(StandardUnaryOperation operation)
        {
            return transform.TransformOutput(HyperMath.Call(operation, value));
        }

        public TInner CallComponent(StandardUnaryOperation operation)
        {
            return transform.TransformOutput(value).Call(operation);
        }

        public override bool Equals(object obj)
        {
            return obj is TransformedNumber<TInner, TTransformation> value && Equals(in value) || Value.Equals(obj);
        }

        public bool Equals(in TransformedNumber<TInner, TTransformation> other)
        {
            return HyperMath.Equals(value, other.value);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(value, transform.TransformInput(other));
        }

        public int CompareTo(in TransformedNumber<TInner, TTransformation> other)
        {
            return HyperMath.Compare(value, other.value);
        }

        public int CompareTo(in TInner other)
        {
            return HyperMath.Compare(value, transform.TransformInput(other));
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return transform.TransformOutput(value).ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return transform.TransformOutput(value).ToString(format, formatProvider);
        }

        partial class Operations : NumberOperations<TransformedNumber<TInner, TTransformation>>, IExtendedNumberOperations<TransformedNumber<TInner, TTransformation>, TInner>, INumberOperations<TransformedNumber<TInner, TTransformation>, TInner>
        {
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;

            public virtual TransformedNumber<TInner, TTransformation> Create(StandardNumber num)
            {
                return HyperMath.Create<TInner>(num);
            }

            public virtual TransformedNumber<TInner, TTransformation> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new TransformedNumber<TInner, TTransformation>(realUnit);
            }

            public virtual TransformedNumber<TInner, TTransformation> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual TransformedNumber<TInner, TTransformation> Create(IEnumerator<TInner> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new TransformedNumber<TInner, TTransformation>(value);
            }
        }

        /// <summary>
        /// An interface that a user of <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/> must provide
        /// an implementation of to define the transformations that are applied on the values of <typeparamref name="TInner"/>.
        /// </summary>
        public interface ITransformation
        {
            /// <summary>
            /// Transforms a value of <typeparamref name="TInner"/> for use in operations.
            /// </summary>
            /// <param name="num">The argument of the operations.</param>
            /// <returns>A value which is used internally in all operations.</returns>
            TInner TransformInput(in TInner num);

            /// <summary>
            /// Transforms back the result of an operation. Should be the inverse operation of <see cref="TransformInput(in TInner)"/>.
            /// </summary>
            /// <param name="num">The result of the operations.</param>
            /// <returns>A value which is presented as the result of the operation.</returns>
            TInner TransformOutput(in TInner num);
        }

        int ICollection<TInner>.Count => 1;

        int IReadOnlyCollection<TInner>.Count => 1;

        TInner IReadOnlyList<TInner>.this[int index] => index == 0 ? Value : throw new ArgumentOutOfRangeException(nameof(index));

        TInner IList<TInner>.this[int index]
        {
            get{
                return index == 0 ? Value : throw new ArgumentOutOfRangeException(nameof(index));
            }
            set{
                throw new NotSupportedException();
            }
        }

        int IList<TInner>.IndexOf(TInner item)
        {
            return Value.Equals(item) ? 0 : -1;
        }

        bool ICollection<TInner>.Contains(TInner item)
        {
            return Value.Equals(item);
        }

        void ICollection<TInner>.CopyTo(TInner[] array, int arrayIndex)
        {
            array[arrayIndex] = Value;
        }

        IEnumerator<TInner> IEnumerable<TInner>.GetEnumerator()
        {
            yield return Value;
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
            return Value.GetEnumerator();
        }
    }

    /// <summary>
    /// A utility number type modifying <typeparamref name="TInner"/> whose operations
    /// appear as if the number was transformed to another value and then back
    /// when the operation is performed.
    /// </summary>
    /// <typeparam name="TInner">The inner type.</typeparam>
    /// <typeparam name="TComponent">The component type the number uses.</typeparam>
    /// <typeparam name="TTransformation">A type implementing <see cref="TransformedNumber{TInner, TTransformation}.ITransformation"/> which is constructed once for every number type and used for the transformations.</typeparam>
    [Serializable]
    public readonly partial struct TransformedNumber<TInner, TComponent, TTransformation> : IWrapperNumber<TransformedNumber<TInner, TComponent, TTransformation>, TInner, TComponent>, INumber<TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent> where TTransformation : struct, TransformedNumber<TInner, TTransformation>.ITransformation
    {
        static TTransformation transform = default;

        readonly TInner value;

        public TInner Value => transform.TransformOutput(value);

        public bool IsInvertible => CanInv(value);

        public bool IsFinite => IsFin(value);

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;

        public TransformedNumber(in TInner value)
        {
            this.value = transform.TransformInput(value);
        }

        private TransformedNumber(in TInner value, object marker)
        {
            this.value = value;
        }

        public TransformedNumber<TInner, TComponent, TTransformation> Clone()
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Clone(value), null);
        }

        TInner INumber<TInner>.Clone()
        {
            return transform.TransformOutput(HyperMath.Clone(value));
        }

        public TransformedNumber<TInner, TComponent, TTransformation> Call(StandardBinaryOperation operation, in TransformedNumber<TInner, TComponent, TTransformation> other)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Call(operation, value, other.value), null);
        }

        public TransformedNumber<TInner, TComponent, TTransformation> Call(StandardBinaryOperation operation, in TInner other)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(transform.TransformInput(other).CallReversed(operation, value), null);
        }

        public TransformedNumber<TInner, TComponent, TTransformation> CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(transform.TransformInput(other).Call(operation, value), null);
        }

        TInner INumber<TInner>.Call(StandardBinaryOperation operation, in TInner other)
        {
            return transform.TransformOutput(transform.TransformInput(other).CallReversed(operation, value));
        }

        TInner INumber<TInner>.CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return transform.TransformOutput(transform.TransformInput(other).Call(operation, value));
        }

        public TransformedNumber<TInner, TComponent, TTransformation> Call(StandardBinaryOperation operation, in TComponent other)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(Operations.Instance.Create(other).CallReversed(operation, value), null);
        }

        public TransformedNumber<TInner, TComponent, TTransformation> CallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(Operations.Instance.Create(other).Call(operation, value), null);
        }

        TInner INumber<TInner, TComponent>.Call(StandardBinaryOperation operation, in TComponent other)
        {
            return transform.TransformOutput(Operations.Instance.Create(other).CallReversed(operation, value));
        }

        TInner INumber<TInner, TComponent>.CallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return transform.TransformOutput(Operations.Instance.Create(other).Call(operation, value));
        }

        public TransformedNumber<TInner, TComponent, TTransformation> Call(StandardUnaryOperation operation)
        {
            return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Call(operation, value), null);
        }

        TInner INumber<TInner>.Call(StandardUnaryOperation operation)
        {
            return transform.TransformOutput(HyperMath.Call(operation, value));
        }

        public TComponent CallComponent(StandardUnaryOperation operation)
        {
            return transform.TransformOutput(value).CallComponent(operation);
        }

        public override bool Equals(object obj)
        {
            return obj is TransformedNumber<TInner, TComponent, TTransformation> value && Equals(in value) || Value.Equals(obj);
        }

        public bool Equals(in TransformedNumber<TInner, TComponent, TTransformation> other)
        {
            return HyperMath.Equals(value, other.value);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(value, transform.TransformInput(other));
        }

        public int CompareTo(in TransformedNumber<TInner, TComponent, TTransformation> other)
        {
            return HyperMath.Compare(value, other.value);
        }

        public int CompareTo(in TInner other)
        {
            return HyperMath.Compare(value, transform.TransformInput(other));
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return transform.TransformOutput(value).ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return transform.TransformOutput(value).ToString(format, formatProvider);
        }

        partial class Operations : NumberOperations<TransformedNumber<TInner, TComponent, TTransformation>>, IExtendedNumberOperations<TransformedNumber<TInner, TComponent, TTransformation>, TInner, TComponent>
        {
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;

            public virtual TransformedNumber<TInner, TComponent, TTransformation> Create(StandardNumber num)
            {
                return HyperMath.Create<TInner>(num);
            }

            public virtual TransformedNumber<TInner, TComponent, TTransformation> Create(in TComponent num)
            {
                return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual TransformedNumber<TInner, TComponent, TTransformation> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, allUnitsCombined);
            }

            public virtual TransformedNumber<TInner, TComponent, TTransformation> Create(IEnumerable<TComponent> units)
            {
                return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units));
            }

            public virtual TransformedNumber<TInner, TComponent, TTransformation> Create(IEnumerator<TComponent> units)
            {
                return new TransformedNumber<TInner, TComponent, TTransformation>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units));
            }
        }
    }

    namespace Parameters
    {
        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms numbers via <see cref="StandardUnaryOperation.Negate"/>.
        /// </summary>
        /// <typeparam name="TNumber">The transformed number type.</typeparam>
        public struct Negate<TNumber> : TransformedNumber<TNumber, Negate<TNumber>>.ITransformation where TNumber : struct, INumber<TNumber>
        {
            TNumber TransformedNumber<TNumber, Negate<TNumber>>.ITransformation.TransformInput(in TNumber num)
            {
                return Neg(num);
            }

            TNumber TransformedNumber<TNumber, Negate<TNumber>>.ITransformation.TransformOutput(in TNumber num)
            {
                return Neg(num);
            }
        }

        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms numbers via <see cref="StandardUnaryOperation.Inverse"/>.
        /// </summary>
        /// <typeparam name="TNumber">The transformed number type.</typeparam>
        public struct Inverse<TNumber> : TransformedNumber<TNumber, Inverse<TNumber>>.ITransformation where TNumber : struct, INumber<TNumber>
        {
            TNumber TransformedNumber<TNumber, Inverse<TNumber>>.ITransformation.TransformInput(in TNumber num)
            {
                return Inv(num);
            }

            TNumber TransformedNumber<TNumber, Inverse<TNumber>>.ITransformation.TransformOutput(in TNumber num)
            {
                return Inv(num);
            }
        }

        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms numbers via <see cref="StandardUnaryOperation.Exponentiate"/>.
        /// </summary>
        /// <typeparam name="TNumber">The transformed number type.</typeparam>
        public struct Exponentiate<TNumber> : TransformedNumber<TNumber, Exponentiate<TNumber>>.ITransformation where TNumber : struct, INumber<TNumber>
        {
            TNumber TransformedNumber<TNumber, Exponentiate<TNumber>>.ITransformation.TransformInput(in TNumber num)
            {
                return Exp(num);
            }

            TNumber TransformedNumber<TNumber, Exponentiate<TNumber>>.ITransformation.TransformOutput(in TNumber num)
            {
                return Log(num);
            }
        }

        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms values of hyper-numbers to perpendicular vectors.
        /// </summary>
        /// <typeparam name="THyperNumber">The transformed number type, implementing <see cref="IHyperNumber{TNumber, TInner}"/>.</typeparam>
        /// <typeparam name="TInner">The inner number type.</typeparam>
        public struct Perpendicular<THyperNumber, TInner> : TransformedNumber<THyperNumber, Perpendicular<THyperNumber, TInner>>.ITransformation where THyperNumber : struct, IHyperNumber<THyperNumber, TInner> where TInner : struct, INumber<TInner>
        {
            static IHyperNumberOperations<THyperNumber, TInner> Operations => HyperMath.Operations.ForHyper<THyperNumber, TInner>.Instance;

            THyperNumber TransformedNumber<THyperNumber, Perpendicular<THyperNumber, TInner>>.ITransformation.TransformInput(in THyperNumber num)
            {
                return Operations.Create(Operations.GetSecondReference(num), Neg(Operations.GetFirstReference(num)));
            }

            THyperNumber TransformedNumber<THyperNumber, Perpendicular<THyperNumber, TInner>>.ITransformation.TransformOutput(in THyperNumber num)
            {
                return Operations.Create(Neg(Operations.GetSecondReference(num)), Operations.GetFirstReference(num));
            }
        }

        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms values of hyper-numbers by scaling them in the two primary axes.
        /// </summary>
        /// <typeparam name="THyperNumber">The transformed number type, implementing <see cref="IHyperNumber{TNumber, TInner}"/>.</typeparam>
        /// <typeparam name="TInner">The inner number type.</typeparam>
        /// <typeparam name="TComponent">The component type that <typeparamref name="TInner"/> uses.</typeparam>
        /// <typeparam name="TCoefficients">The type implementing <see cref="ICoefficients{TComponent}"/> whose default value will provide the coefficients.</typeparam>
        public struct Scale<THyperNumber, TInner, TComponent, TCoefficients> : TransformedNumber<THyperNumber, Scale<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation where THyperNumber : struct, IHyperNumber<THyperNumber, TInner> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent> where TCoefficients : struct, ICoefficients<TComponent>
        {
            static TCoefficients coefficients = default;
            static IHyperNumberOperations<THyperNumber, TInner> Operations => HyperMath.Operations.ForHyper<THyperNumber, TInner>.Instance;

            THyperNumber TransformedNumber<THyperNumber, Scale<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation.TransformInput(in THyperNumber num)
            {
                return Operations.Create(MulVal(Operations.GetFirstReference(num), coefficients.InputX), MulVal(Operations.GetSecondReference(num), coefficients.InputY));
            }

            THyperNumber TransformedNumber<THyperNumber, Scale<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation.TransformOutput(in THyperNumber num)
            {
                return Operations.Create(MulVal(Operations.GetFirstReference(num), coefficients.OutputX), MulVal(Operations.GetSecondReference(num), coefficients.OutputY));
            }
        }

        /// <summary>
        /// Parameter type for <see cref="TransformedNumber{TInner, TTransformation}"/> or <see cref="TransformedNumber{TInner, TComponent, TTransformation}"/>
        /// that transforms values of hyper-numbers by skewing them along the two primary axes.
        /// </summary>
        /// <typeparam name="THyperNumber">The transformed number type, implementing <see cref="IHyperNumber{TNumber, TInner}"/>.</typeparam>
        /// <typeparam name="TInner">The inner number type.</typeparam>
        /// <typeparam name="TComponent">The component type that <typeparamref name="TInner"/> uses.</typeparam>
        /// <typeparam name="TCoefficients">The type implementing <see cref="ICoefficients{TComponent}"/> whose default value will provide the coefficients.</typeparam>
        public struct Skew<THyperNumber, TInner, TComponent, TCoefficients> : TransformedNumber<THyperNumber, Skew<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation where THyperNumber : struct, IHyperNumber<THyperNumber, TInner> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent> where TCoefficients : struct, ICoefficients<TComponent>
        {
            static TCoefficients coefficients = default;
            static IHyperNumberOperations<THyperNumber, TInner> Operations => HyperMath.Operations.ForHyper<THyperNumber, TInner>.Instance;

            THyperNumber TransformedNumber<THyperNumber, Skew<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation.TransformInput(in THyperNumber num)
            {
                return Operations.Create(Add(Operations.GetFirstReference(num), MulVal(Operations.GetSecondReference(num), coefficients.InputX)), Add(Operations.GetSecondReference(num), MulVal(Operations.GetFirstReference(num), coefficients.InputY)));
            }

            THyperNumber TransformedNumber<THyperNumber, Skew<THyperNumber, TInner, TComponent, TCoefficients>>.ITransformation.TransformOutput(in THyperNumber num)
            {
                return Operations.Create(Add(Operations.GetFirstReference(num), MulVal(Operations.GetSecondReference(num), coefficients.OutputX)), Add(Operations.GetSecondReference(num), MulVal(Operations.GetFirstReference(num), coefficients.OutputY)));
            }
        }

        /// <summary>
        /// An interface that is used to provide custom coefficients to <see cref="Scale{THyperNumber, TInner, TComponent, TCoefficients}"/> or <see cref="Skew{THyperNumber, TInner, TComponent, TCoefficients}"/>.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        public interface ICoefficients<TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent>
        {
            /// <summary>
            /// The input coefficient for the X axis.
            /// </summary>
            TComponent InputX { get; }

            /// <summary>
            /// The input coefficient for the Y axis.
            /// </summary>
            TComponent InputY { get; }

            /// <summary>
            /// The output coefficient for the X axis. Should be the inverse (with respect to the operation) of <see cref="InputX"/>.
            /// </summary>
            TComponent OutputX { get; }

            /// <summary>
            /// The output coefficient for the Y axis. Should be the inverse (with respect to the operation) of <see cref="InputY"/>.
            /// </summary>
            TComponent OutputY { get; }
        }
    }
}
