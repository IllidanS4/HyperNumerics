﻿using IS4.HyperNumerics.Operations;
using System;
using System.Collections;
using System.Collections.Generic;

using static IS4.HyperNumerics.HyperMath;

namespace IS4.HyperNumerics.NumberTypes
{
    /// <summary>
    /// A utility number type whose default value is different from the default value
    /// of <typeparamref name="TInner"/>, and can be specified using a custom type.
    /// </summary>
    /// <typeparam name="TInner">The inner type.</typeparam>
    /// <typeparam name="TProvider">A type implementing <see cref="IDefaultValueProvider"/> which is constructed once for every number type and queried for the default value.</typeparam>
    [Serializable]
    public readonly partial struct CustomDefaultNumber<TInner, TProvider> : IWrapperNumber<CustomDefaultNumber<TInner, TProvider>, TInner>, INumber<CustomDefaultNumber<TInner, TProvider>, TInner>, INumber<TInner> where TInner : struct, INumber<TInner> where TProvider : struct, CustomDefaultNumber<TInner, TProvider>.IDefaultValueProvider
    {
        static TInner defaultValue = default(TProvider).DefaultValue;

        readonly TInner value;
        readonly bool initialized;

        public TInner Value => initialized ? value : defaultValue;

        public bool IsInvertible => initialized ? CanInv(value) : defaultValue.IsInvertible;

        public bool IsFinite => initialized ? IsFin(value) : defaultValue.IsFinite;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;
        
        public CustomDefaultNumber(in TInner value)
        {
            this.value = value;
            this.initialized = true;
        }

        public CustomDefaultNumber<TInner, TProvider> Clone()
        {
            if(initialized)
            {
                return HyperMath.Clone(value);
            }
            return defaultValue.Clone();
        }

        TInner INumber<TInner>.Clone()
        {
            if(initialized)
            {
                return HyperMath.Clone(value);
            }
            return defaultValue.Clone();
        }

        public CustomDefaultNumber<TInner, TProvider> Call(StandardBinaryOperation operation, in CustomDefaultNumber<TInner, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Call(operation, value, other.value);
                }
                return HyperMath.Call(operation, value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Call(operation, defaultValue, other.value);
            }
            return HyperMath.Call(operation, defaultValue, defaultValue);
        }

        public CustomDefaultNumber<TInner, TProvider> Call(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        public CustomDefaultNumber<TInner, TProvider> CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        TInner INumber<TInner>.Call(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        TInner INumber<TInner>.CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        public CustomDefaultNumber<TInner, TProvider> Call(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value);
            }
            return defaultValue.Call(operation);
        }

        TInner INumber<TInner>.Call(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value);
            }
            return defaultValue.Call(operation);
        }

        public TInner CallComponent(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value);
            }
            return defaultValue.Call(operation);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomDefaultNumber<TInner, TProvider> value && Equals(in value) || Value.Equals(obj);
        }

        public bool Equals(in CustomDefaultNumber<TInner, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Equals(value, other.value);
                }
                return HyperMath.Equals(value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Equals(defaultValue, other.value);
            }
            return true;
        }

        public bool Equals(in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Equals(value, other);
            }
            return HyperMath.Equals(defaultValue, other);
        }

        public int CompareTo(in CustomDefaultNumber<TInner, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Compare(value, other.value);
                }
                return HyperMath.Compare(value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Compare(defaultValue, other.value);
            }
            return 0;
        }

        public int CompareTo(in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Compare(value, other);
            }
            return HyperMath.Compare(defaultValue, other);
        }

        public override int GetHashCode()
        {
            if(initialized)
            {
                return value.GetHashCode();
            }
            return defaultValue.GetHashCode();
        }

        public override string ToString()
        {
            if(initialized)
            {
                return value.ToString();
            }
            return defaultValue.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if(initialized)
            {
                return value.ToString(format, formatProvider);
            }
            return defaultValue.ToString(format, formatProvider);
        }

        partial class Operations : NumberOperations<CustomDefaultNumber<TInner, TProvider>>, IExtendedNumberOperations<CustomDefaultNumber<TInner, TProvider>, TInner>, INumberOperations<CustomDefaultNumber<TInner, TProvider>, TInner>
        {
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;

            public virtual CustomDefaultNumber<TInner, TProvider> Create(StandardNumber num)
            {
                return HyperMath.Create<TInner>(num);
            }

            public virtual CustomDefaultNumber<TInner, TProvider> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new CustomDefaultNumber<TInner, TProvider>(realUnit);
            }

            public virtual CustomDefaultNumber<TInner, TProvider> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual CustomDefaultNumber<TInner, TProvider> Create(IEnumerator<TInner> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new CustomDefaultNumber<TInner, TProvider>(value);
            }
        }

        /// <summary>
        /// An interface that a user of <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/> must provide
        /// an implementation of to specify the default value of <typeparamref name="TInner"/>.
        /// </summary>
        public interface IDefaultValueProvider
        {
            /// <summary>
            /// Obtains the default value of the type.
            /// </summary>
            TInner DefaultValue { get; }
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
    /// A utility number type whose default value is different from the default value
    /// of <typeparamref name="TInner"/>, and can be specified using a custom type.
    /// </summary>
    /// <typeparam name="TInner">The inner type.</typeparam>
    /// <typeparam name="TComponent">The component type the number uses.</typeparam>
    /// <typeparam name="TProvider">A type implementing <see cref="CustomDefaultNumber{TInner, TProvider}.IDefaultValueProvider"/> which is constructed once for every number type and queried for the default value.</typeparam>
    [Serializable]
    public readonly partial struct CustomDefaultNumber<TInner, TComponent, TProvider> : IWrapperNumber<CustomDefaultNumber<TInner, TComponent, TProvider>, TInner, TComponent>, INumber<TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent> where TProvider : struct, CustomDefaultNumber<TInner, TProvider>.IDefaultValueProvider
    {
        static TInner defaultValue = default(TProvider).DefaultValue;

        readonly TInner value;
        readonly bool initialized;

        public TInner Value => initialized ? value : defaultValue;

        public bool IsInvertible => initialized ? CanInv(value) : defaultValue.IsInvertible;

        public bool IsFinite => initialized ? IsFin(value) : defaultValue.IsFinite;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;
        
        public CustomDefaultNumber(in TInner value)
        {
            this.value = value;
            this.initialized = true;
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> Clone()
        {
            if(initialized)
            {
                return HyperMath.Clone(value);
            }
            return defaultValue.Clone();
        }

        TInner INumber<TInner>.Clone()
        {
            if(initialized)
            {
                return HyperMath.Clone(value);
            }
            return defaultValue.Clone();
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> Call(StandardBinaryOperation operation, in CustomDefaultNumber<TInner, TComponent, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Call(operation, value, other.value);
                }
                return HyperMath.Call(operation, value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Call(operation, defaultValue, other.value);
            }
            return HyperMath.Call(operation, defaultValue, defaultValue);
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> Call(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        TInner INumber<TInner>.Call(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        TInner INumber<TInner>.CallReversed(StandardBinaryOperation operation, in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> Call(StandardBinaryOperation operation, in TComponent other)
        {
            if(initialized)
            {
                return HyperMath.CallComponent(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> CallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            if(initialized)
            {
                return HyperMath.CallComponentReversed(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        TInner INumber<TInner, TComponent>.Call(StandardBinaryOperation operation, in TComponent other)
        {
            if(initialized)
            {
                return HyperMath.CallComponent(operation, value, other);
            }
            return defaultValue.Call(operation, other);
        }

        TInner INumber<TInner, TComponent>.CallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            if(initialized)
            {
                return HyperMath.CallComponentReversed(operation, other, value);
            }
            return defaultValue.CallReversed(operation, other);
        }

        public CustomDefaultNumber<TInner, TComponent, TProvider> Call(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value);
            }
            return defaultValue.Call(operation);
        }

        TInner INumber<TInner>.Call(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.Call(operation, value);
            }
            return defaultValue.Call(operation);
        }

        public TComponent CallComponent(StandardUnaryOperation operation)
        {
            if(initialized)
            {
                return HyperMath.CallComponent<TInner, TComponent>(operation, value);
            }
            return defaultValue.CallComponent(operation);
        }

        public override bool Equals(object obj)
        {
            return obj is CustomDefaultNumber<TInner, TComponent, TProvider> value && Equals(in value) || Value.Equals(obj);
        }

        public bool Equals(in CustomDefaultNumber<TInner, TComponent, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Equals(value, other.value);
                }
                return HyperMath.Equals(value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Equals(defaultValue, other.value);
            }
            return true;
        }

        public bool Equals(in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Equals(value, other);
            }
            return HyperMath.Equals(defaultValue, other);
        }

        public int CompareTo(in CustomDefaultNumber<TInner, TComponent, TProvider> other)
        {
            if(initialized)
            {
                if(other.initialized)
                {
                    return HyperMath.Compare(value, other.value);
                }
                return HyperMath.Compare(value, defaultValue);
            }
            if(other.initialized)
            {
                return HyperMath.Compare(defaultValue, other.value);
            }
            return 0;
        }

        public int CompareTo(in TInner other)
        {
            if(initialized)
            {
                return HyperMath.Compare(value, other);
            }
            return HyperMath.Compare(defaultValue, other);
        }

        public override int GetHashCode()
        {
            if(initialized)
            {
                return value.GetHashCode();
            }
            return defaultValue.GetHashCode();
        }

        public override string ToString()
        {
            if(initialized)
            {
                return value.ToString();
            }
            return defaultValue.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if(initialized)
            {
                return value.ToString(format, formatProvider);
            }
            return defaultValue.ToString(format, formatProvider);
        }

        partial class Operations : NumberOperations<CustomDefaultNumber<TInner, TComponent, TProvider>>, IExtendedNumberOperations<CustomDefaultNumber<TInner, TComponent, TProvider>, TInner, TComponent>
        {
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension;

            public virtual CustomDefaultNumber<TInner, TComponent, TProvider> Create(StandardNumber num)
            {
                return HyperMath.Create<TInner>(num);
            }

            public virtual CustomDefaultNumber<TInner, TComponent, TProvider> Create(in TComponent num)
            {
                return new CustomDefaultNumber<TInner, TComponent, TProvider>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual CustomDefaultNumber<TInner, TComponent, TProvider> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, allUnitsCombined);
            }

            public virtual CustomDefaultNumber<TInner, TComponent, TProvider> Create(IEnumerable<TComponent> units)
            {
                return new CustomDefaultNumber<TInner, TComponent, TProvider>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units));
            }

            public virtual CustomDefaultNumber<TInner, TComponent, TProvider> Create(IEnumerator<TComponent> units)
            {
                return new CustomDefaultNumber<TInner, TComponent, TProvider>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units));
            }
        }
    }

    namespace Parameters
    {
        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.Zero"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct Zero<TNumber> : CustomDefaultNumber<TNumber, Zero<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.Zero;
            TNumber CustomDefaultNumber<TNumber, Zero<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.One"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct One<TNumber> : CustomDefaultNumber<TNumber, One<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.One;
            TNumber CustomDefaultNumber<TNumber, One<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.NegativeOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct NegativeOne<TNumber> : CustomDefaultNumber<TNumber, NegativeOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.NegativeOne;
            TNumber CustomDefaultNumber<TNumber, NegativeOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.Two"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct Two<TNumber> : CustomDefaultNumber<TNumber, Two<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.Two;
            TNumber CustomDefaultNumber<TNumber, Two<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.SpecialOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct SpecialOne<TNumber> : CustomDefaultNumber<TNumber, SpecialOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.SpecialOne;
            TNumber CustomDefaultNumber<TNumber, SpecialOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.UnitsOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct UnitsOne<TNumber> : CustomDefaultNumber<TNumber, UnitsOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.UnitsOne;
            TNumber CustomDefaultNumber<TNumber, UnitsOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.NonRealUnitsOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct NonRealUnitsOne<TNumber> : CustomDefaultNumber<TNumber, NonRealUnitsOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.NonRealUnitsOne;
            TNumber CustomDefaultNumber<TNumber, NonRealUnitsOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.CombinedOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct CombinedOne<TNumber> : CustomDefaultNumber<TNumber, CombinedOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.CombinedOne;
            TNumber CustomDefaultNumber<TNumber, CombinedOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }

        /// <summary>
        /// Parameter type for <see cref="CustomDefaultNumber{TInner, TProvider}"/> or <see cref="CustomDefaultNumber{TInner, TComponent, TProvider}"/>
        /// that initializes the default value with <see cref="StandardNumber.AllOne"/>.
        /// </summary>
        /// <typeparam name="TNumber">The produced number type.</typeparam>
        public struct AllOne<TNumber> : CustomDefaultNumber<TNumber, AllOne<TNumber>>.IDefaultValueProvider where TNumber : struct, INumber<TNumber>
        {
            const StandardNumber Value = StandardNumber.AllOne;
            TNumber CustomDefaultNumber<TNumber, AllOne<TNumber>>.IDefaultValueProvider.DefaultValue => HyperMath.Operations.For<TNumber>.Instance.Create(Value);
        }
    }
}
