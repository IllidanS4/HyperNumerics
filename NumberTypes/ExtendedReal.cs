﻿using IS4.HyperNumerics.Operations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IS4.HyperNumerics.NumberTypes
{
    /// <summary>
    /// Represents an extended real number, with its value stored as a <see cref="System.Double"/>, allowing for infinities and NaNs.
    /// </summary>
    [Serializable]
    public readonly struct ExtendedReal : ISimpleNumber<ExtendedReal, double>, ISimpleNumber<ExtendedReal, float>, ISimpleNumber<ExtendedReal, ExtendedReal>, IWrapperNumber<ExtendedReal, ExtendedReal, double>, IWrapperNumber<ExtendedReal, ExtendedReal, float>, IWrapperNumber<ExtendedReal, ExtendedReal, ExtendedReal>, IExtendedNumber<ExtendedReal, Real, double>, IExtendedNumber<ExtendedReal, Real, float>
    {
        public static readonly ExtendedReal Zero = new ExtendedReal(0.0);
        public static readonly ExtendedReal One = new ExtendedReal(1.0);

        public double Value { get; }

        float ISimpleNumber<float>.Value => (float)Value;

        ExtendedReal IWrapperNumber<ExtendedReal>.Value => this;

        ExtendedReal ISimpleNumber<ExtendedReal>.Value => this;

        public bool IsInvertible => true;

        public bool IsFinite => !Double.IsInfinity(Value) && !Double.IsNaN(Value);

        int INumber.Dimension => 1;

        /// <summary>
        /// Constructs a new value from its inner <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="value">The value of the real number.</param>
        public ExtendedReal(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructs a new value from its inner <see cref="System.Single"/> value.
        /// </summary>
        /// <param name="value">The value of the real number.</param>
        public ExtendedReal(float value)
        {
            Value = (float)value;
        }

        ExtendedReal INumber<ExtendedReal>.Clone()
        {
            return this;
        }

        object ICloneable.Clone()
        {
            return this;
        }

        public ExtendedReal Call(BinaryOperation operation, in ExtendedReal other)
        {
            switch(operation)
            {
                case BinaryOperation.Add:
                    return Value + other.Value;
                case BinaryOperation.Subtract:
                    return Value - other.Value;
                case BinaryOperation.Multiply:
                    return Value * other.Value;
                case BinaryOperation.Divide:
                    return Value / other.Value;
                case BinaryOperation.Power:
                    return Math.Pow(Value, other.Value);
                case BinaryOperation.Atan2:
                    return Math.Atan2(Value, other.Value);
                default:
                    throw new NotSupportedException();
            }
        }

        public ExtendedReal Call(BinaryOperation operation, in Real other)
        {
            switch(operation)
            {
                case BinaryOperation.Add:
                    return Value + other.Value;
                case BinaryOperation.Subtract:
                    return Value - other.Value;
                case BinaryOperation.Multiply:
                    return Value * other.Value;
                case BinaryOperation.Divide:
                    return Value / other.Value;
                case BinaryOperation.Power:
                    return Math.Pow(Value, other.Value);
                case BinaryOperation.Atan2:
                    return Math.Atan2(Value, other.Value);
                default:
                    throw new NotSupportedException();
            }
        }

        public ExtendedReal Call(BinaryOperation operation, double other)
        {
            switch(operation)
            {
                case BinaryOperation.Add:
                    return Value + other;
                case BinaryOperation.Subtract:
                    return Value - other;
                case BinaryOperation.Multiply:
                    return Value * other;
                case BinaryOperation.Divide:
                    return Value / other;
                case BinaryOperation.Power:
                    return Math.Pow(Value, other);
                case BinaryOperation.Atan2:
                    return Math.Atan2(Value, other);
                default:
                    throw new NotSupportedException();
            }
        }

        public ExtendedReal Call(BinaryOperation operation, float other)
        {
            switch(operation)
            {
                case BinaryOperation.Add:
                    return Value + other;
                case BinaryOperation.Subtract:
                    return Value - other;
                case BinaryOperation.Multiply:
                    return Value * other;
                case BinaryOperation.Divide:
                    return Value / other;
                case BinaryOperation.Power:
                    return Math.Pow(Value, other);
                case BinaryOperation.Atan2:
                    return Math.Atan2(Value, other);
                default:
                    throw new NotSupportedException();
            }
        }

        ExtendedReal INumber<ExtendedReal, ExtendedReal>.Call(BinaryOperation operation, ExtendedReal other)
        {
            return Call(operation, in other);
        }

        public ExtendedReal Call(UnaryOperation operation)
        {
            switch(operation)
            {
                case UnaryOperation.Negate:
                    return -Value;
                case UnaryOperation.Increment:
                    return Value + 1;
                case UnaryOperation.Decrement:
                    return Value - 1;
                case UnaryOperation.Inverse:
                    return 1.0 / Value;
                case UnaryOperation.Conjugate:
                    return Value;
                case UnaryOperation.Modulus:
                    return Math.Abs(Value);
                case UnaryOperation.Double:
                    return Value * 2.0;
                case UnaryOperation.Half:
                    return Value * 0.5;
                case UnaryOperation.Square:
                    return Value * Value;
                case UnaryOperation.SquareRoot:
                    return Math.Sqrt(Value);
                case UnaryOperation.Exponentiate:
                    return Math.Exp(Value);
                case UnaryOperation.Logarithm:
                    return Math.Log(Value);
                case UnaryOperation.Sine:
                    return Math.Sin(Value);
                case UnaryOperation.Cosine:
                    return Math.Cos(Value);
                case UnaryOperation.Tangent:
                    return Math.Tan(Value);
                case UnaryOperation.HyperbolicSine:
                    return Math.Sinh(Value);
                case UnaryOperation.HyperbolicCosine:
                    return Math.Cosh(Value);
                case UnaryOperation.HyperbolicTangent:
                    return Math.Tanh(Value);
                case UnaryOperation.ArcSine:
                    return Math.Asin(Value);
                case UnaryOperation.ArcCosine:
                    return Math.Acos(Value);
                case UnaryOperation.ArcTangent:
                    return Math.Atan(Value);
                default:
                    throw new NotSupportedException();
            }
        }

        public double Call(PrimitiveUnaryOperation operation)
        {
            switch(operation)
            {
                case PrimitiveUnaryOperation.AbsoluteValue:
                    return Math.Abs(Value);
                case PrimitiveUnaryOperation.RealValue:
                    return Value;
                default:
                    throw new NotSupportedException();
            }
        }

        float INumber<ExtendedReal, float>.Call(PrimitiveUnaryOperation operation)
        {
            return (float)Call(operation);
        }

        ExtendedReal INumber<ExtendedReal, ExtendedReal>.Call(PrimitiveUnaryOperation operation)
        {
            return new ExtendedReal(Call(operation));
        }

        public override bool Equals(object obj)
        {
            return obj is ExtendedReal value && Equals(in value);
        }

        public bool Equals(ExtendedReal other)
        {
            return Equals(in other);
        }

        public bool Equals(in ExtendedReal other)
        {
            return Value.Equals(other.Value);
        }

        public int CompareTo(ExtendedReal other)
        {
            return CompareTo(in other);
        }

        public int CompareTo(in ExtendedReal other)
        {
            return Value.CompareTo(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        public static implicit operator ExtendedReal(double value)
        {
            return new ExtendedReal(value);
        }

        public static implicit operator ExtendedReal(float value)
        {
            return new ExtendedReal(value);
        }

        public static implicit operator double(ExtendedReal value)
        {
            return value.Value;
        }

        public static explicit operator float(ExtendedReal value)
        {
            return (float)value.Value;
        }

        public static bool operator==(ExtendedReal a, ExtendedReal b)
        {
            return a.Equals(in b);
        }

        public static bool operator!=(ExtendedReal a, ExtendedReal b)
        {
            return !a.Equals(in b);
        }

        public static bool operator>(ExtendedReal a, ExtendedReal b)
        {
            return a.CompareTo(in b) > 0;
        }

        public static bool operator<(ExtendedReal a, ExtendedReal b)
        {
            return a.CompareTo(in b) < 0;
        }

        public static bool operator>=(ExtendedReal a, ExtendedReal b)
        {
            return a.CompareTo(in b) >= 0;
        }

        public static bool operator<=(ExtendedReal a, ExtendedReal b)
        {
            return a.CompareTo(in b) <= 0;
        }

        INumberOperations INumber.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<ExtendedReal> INumber<ExtendedReal>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<ExtendedReal, double> INumber<ExtendedReal, double>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<ExtendedReal, float> INumber<ExtendedReal, float>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<ExtendedReal, ExtendedReal> INumber<ExtendedReal, ExtendedReal>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, Real> IExtendedNumber<ExtendedReal, Real>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, Real, double> IExtendedNumber<ExtendedReal, Real, double>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, Real, float> IExtendedNumber<ExtendedReal, Real, float>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, ExtendedReal> IExtendedNumber<ExtendedReal, ExtendedReal>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, ExtendedReal, double> IExtendedNumber<ExtendedReal, ExtendedReal, double>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, ExtendedReal, float> IExtendedNumber<ExtendedReal, ExtendedReal, float>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<ExtendedReal, ExtendedReal, ExtendedReal> IExtendedNumber<ExtendedReal, ExtendedReal, ExtendedReal>.GetOperations()
        {
            return Operations.Instance;
        }

        class Operations : NumberOperations<ExtendedReal>, IExtendedNumberOperations<ExtendedReal, Real, double>, IExtendedNumberOperations<ExtendedReal, Real, float>, IExtendedNumberOperations<ExtendedReal, ExtendedReal, double>, IExtendedNumberOperations<ExtendedReal, ExtendedReal, float>, IExtendedNumberOperations<ExtendedReal, ExtendedReal, ExtendedReal>
        {
            public static readonly Operations Instance = new Operations();

            public override int Dimension => 0;

            public bool IsInvertible(in ExtendedReal num)
            {
                return num.IsInvertible;
            }

            public bool IsFinite(in ExtendedReal num)
            {
                return num.IsFinite;
            }

            public ExtendedReal Clone(in ExtendedReal num)
            {
                return num;
            }

            public bool Equals(ExtendedReal num1, ExtendedReal num2)
            {
                return num1.Equals(in num2);
            }

            public int Compare(ExtendedReal num1, ExtendedReal num2)
            {
                return num1.CompareTo(in num2);
            }

            public bool Equals(in ExtendedReal num1, in ExtendedReal num2)
            {
                return num1.Equals(in num2);
            }

            public int Compare(in ExtendedReal num1, in ExtendedReal num2)
            {
                return num1.CompareTo(in num2);
            }

            public int GetHashCode(ExtendedReal num)
            {
                return num.GetHashCode();
            }

            public int GetHashCode(in ExtendedReal num)
            {
                return num.GetHashCode();
            }

            public ExtendedReal Call(NullaryOperation operation)
            {
                switch(operation)
                {
                    case NullaryOperation.RealOne:
                    case NullaryOperation.UnitsOne:
                    case NullaryOperation.AllOne:
                        return 1.0;
                    case NullaryOperation.Zero:
                    case NullaryOperation.SpecialOne:
                    case NullaryOperation.NonRealUnitsOne:
                    case NullaryOperation.CombinedOne:
                        return 0.0;
                    default:
                        throw new NotSupportedException();
                }
            }

            public ExtendedReal Call(UnaryOperation operation, in ExtendedReal num)
            {
                return num.Call(operation);
            }

            public ExtendedReal Call(BinaryOperation operation, in ExtendedReal num1, in ExtendedReal num2)
            {
                return num1.Call(operation, num2);
            }

            public ExtendedReal Call(BinaryOperation operation, in ExtendedReal num1, in Real num2)
            {
                return num1.Call(operation, num2);
            }

            public double Call(PrimitiveUnaryOperation operation, in ExtendedReal num)
            {
                return num.Call(operation);
            }

            float INumberOperations<ExtendedReal, float>.Call(PrimitiveUnaryOperation operation, in ExtendedReal num)
            {
                return (float)num.Call(operation);
            }

            ExtendedReal INumberOperations<ExtendedReal, ExtendedReal>.Call(PrimitiveUnaryOperation operation, in ExtendedReal num)
            {
                return new ExtendedReal(num.Call(operation));
            }

            public ExtendedReal Call(BinaryOperation operation, in ExtendedReal num1, double num2)
            {
                return num1.Call(operation, num2);
            }

            public ExtendedReal Call(BinaryOperation operation, in ExtendedReal num1, float num2)
            {
                return num1.Call(operation, num2);
            }

            ExtendedReal INumberOperations<ExtendedReal, ExtendedReal>.Call(BinaryOperation operation, in ExtendedReal num1, ExtendedReal num2)
            {
                return Call(operation, num1, num2);
            }

            public ExtendedReal Create(double realUnit, double otherUnits, double someUnitsCombined, double allUnitsCombined)
            {
                return new ExtendedReal(realUnit);
            }

            public ExtendedReal Create(float realUnit, float otherUnits, float someUnitsCombined, float allUnitsCombined)
            {
                return new ExtendedReal(realUnit);
            }

            public ExtendedReal Create(ExtendedReal realUnit, ExtendedReal otherUnits, ExtendedReal someUnitsCombined, ExtendedReal allUnitsCombined)
            {
                return realUnit;
            }

            public ExtendedReal Create(in Real num)
            {
                return new ExtendedReal(num.Value);
            }

            public ExtendedReal Create(in ExtendedReal num)
            {
                return num;
            }

            public ExtendedReal Create(IEnumerable<double> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new ExtendedReal(ienum.Current);
            }

            public ExtendedReal Create(IEnumerator<double> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new ExtendedReal(value);
            }

            public ExtendedReal Create(IEnumerable<float> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new ExtendedReal(ienum.Current);
            }

            public ExtendedReal Create(IEnumerator<float> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new ExtendedReal(value);
            }

            public ExtendedReal Create(IEnumerable<ExtendedReal> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return ienum.Current;
            }

            public ExtendedReal Create(IEnumerator<ExtendedReal> units)
            {
                var value = units.Current;
                units.MoveNext();
                return value;
            }
        }

        int ICollection<double>.Count => 1;

        bool ICollection<double>.IsReadOnly => true;

        int IReadOnlyCollection<double>.Count => 1;

        double IReadOnlyList<double>.this[int index] => index == 0 ? Value : throw new ArgumentOutOfRangeException(nameof(index));

        double IList<double>.this[int index]
        {
            get{
                return index == 0 ? Value : throw new ArgumentOutOfRangeException(nameof(index));
            }
            set{
                throw new NotSupportedException();
            }
        }

        int IList<double>.IndexOf(double item)
        {
            return Value == item ? 0 : -1;
        }

        void IList<double>.Insert(int index, double item)
        {
            throw new NotSupportedException();
        }

        void IList<double>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<double>.Add(double item)
        {
            throw new NotSupportedException();
        }

        void ICollection<double>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<double>.Contains(double item)
        {
            return Value == item;
        }

        void ICollection<double>.CopyTo(double[] array, int arrayIndex)
        {
            array[arrayIndex] = Value;
        }

        bool ICollection<double>.Remove(double item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<double> IEnumerable<double>.GetEnumerator()
        {
            yield return Value;
        }

        int ICollection<float>.Count => 1;

        bool ICollection<float>.IsReadOnly => true;

        int IReadOnlyCollection<float>.Count => 1;

        float IReadOnlyList<float>.this[int index] => index == 0 ? (float)Value : throw new ArgumentOutOfRangeException(nameof(index));

        float IList<float>.this[int index]
        {
            get{
                return index == 0 ? (float)Value : throw new ArgumentOutOfRangeException(nameof(index));
            }
            set{
                throw new NotSupportedException();
            }
        }

        int IList<float>.IndexOf(float item)
        {
            return Value == item ? 0 : -1;
        }

        void IList<float>.Insert(int index, float item)
        {
            throw new NotSupportedException();
        }

        void IList<float>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<float>.Add(float item)
        {
            throw new NotSupportedException();
        }

        void ICollection<float>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<float>.Contains(float item)
        {
            return Value == item;
        }

        void ICollection<float>.CopyTo(float[] array, int arrayIndex)
        {
            array[arrayIndex] = (float)Value;
        }

        bool ICollection<float>.Remove(float item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<float> IEnumerable<float>.GetEnumerator()
        {
            yield return (float)Value;
        }

        int ICollection<ExtendedReal>.Count => 1;

        bool ICollection<ExtendedReal>.IsReadOnly => true;

        int IReadOnlyCollection<ExtendedReal>.Count => 1;

        ExtendedReal IReadOnlyList<ExtendedReal>.this[int index] => index == 0 ? this : throw new ArgumentOutOfRangeException(nameof(index));

        ExtendedReal IList<ExtendedReal>.this[int index]
        {
            get{
                return index == 0 ? this : throw new ArgumentOutOfRangeException(nameof(index));
            }
            set{
                throw new NotSupportedException();
            }
        }

        int IList<ExtendedReal>.IndexOf(ExtendedReal item)
        {
            return Equals(in item) ? 0 : -1;
        }

        void IList<ExtendedReal>.Insert(int index, ExtendedReal item)
        {
            throw new NotSupportedException();
        }

        void IList<ExtendedReal>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<ExtendedReal>.Add(ExtendedReal item)
        {
            throw new NotSupportedException();
        }

        void ICollection<ExtendedReal>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<ExtendedReal>.Contains(ExtendedReal item)
        {
            return Equals(in item);
        }

        void ICollection<ExtendedReal>.CopyTo(ExtendedReal[] array, int arrayIndex)
        {
            array[arrayIndex] = this;
        }

        bool ICollection<ExtendedReal>.Remove(ExtendedReal item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<ExtendedReal> IEnumerable<ExtendedReal>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }
    }
}
