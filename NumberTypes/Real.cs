﻿using IS4.HyperNumerics.Operations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IS4.HyperNumerics.NumberTypes
{
    /// <summary>
    /// Represents a real number, with its value stored as a <see cref="System.Double"/>.
    /// </summary>
    /// <remarks>
    /// Not all possible values of <see cref="System.Double"/> are allowed, namely infinites and NaNs.
    /// </remarks>
    [Serializable]
    public readonly struct Real : ISimpleNumber<Real, double>, ISimpleNumber<Real, float>, ISimpleNumber<Real, Real>, IWrapperNumber<Real, Real, double>, IWrapperNumber<Real, Real, float>, IWrapperNumber<Real, Real, Real>, ISimpleNumber<ExtendedReal, double>, ISimpleNumber<ExtendedReal, float>, ISimpleNumber<ExtendedReal, ExtendedReal>
    {
        public static readonly Real Zero = new Real(0.0);
        public static readonly Real One = new Real(1.0);

        public double Value { get; }

        float ISimpleNumber<float>.Value => (float)Value;

        Real IWrapperNumber<Real>.Value => this;

        Real ISimpleNumber<Real>.Value => this;

        ExtendedReal ISimpleNumber<ExtendedReal>.Value => new ExtendedReal(Value);

        public bool IsInvertible => Value != 0.0;

        public bool IsFinite => true;

        int INumber.Dimension => 1;

        /// <summary>
        /// Constructs a new value from its inner <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="value">The value of the real number.</param>
        /// <exception cref="System.NotFiniteNumberException">Thrown when <paramref name="value"/> doesn't correspond to any real number.</exception>
        public Real(double value)
        {
            if(Double.IsInfinity(value) || Double.IsNaN(value))
            {
                throw new NotFiniteNumberException(value);
            }
            Value = value;
        }

        /// <summary>
        /// Constructs a new value from its inner <see cref="System.Single"/> value.
        /// </summary>
        /// <param name="value">The value of the real number.</param>
        /// <exception cref="System.NotFiniteNumberException">Thrown when <paramref name="value"/> doesn't correspond to any real number.</exception>
        public Real(float value)
        {
            if(Single.IsInfinity(value) || Single.IsNaN(value))
            {
                throw new NotFiniteNumberException(value);
            }
            Value = value;
        }

        Real INumber<Real>.Clone()
        {
            return this;
        }

        ExtendedReal INumber<ExtendedReal>.Clone()
        {
            return new ExtendedReal(Value);
        }

        object ICloneable.Clone()
        {
            return this;
        }

        Real INumber<Real, Real>.Call(BinaryOperation operation, Real other)
        {
            return Call(operation, in other);
        }

        public Real Call(BinaryOperation operation, in Real other)
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

        public Real Call(BinaryOperation operation, double other)
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

        ExtendedReal INumber<ExtendedReal, double>.Call(BinaryOperation operation, double other)
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

        public Real Call(BinaryOperation operation, float other)
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

        ExtendedReal INumber<ExtendedReal, float>.Call(BinaryOperation operation, float other)
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

        public Real Call(UnaryOperation operation)
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

        ExtendedReal INumber<ExtendedReal>.Call(UnaryOperation operation)
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

        float INumber<Real, float>.Call(PrimitiveUnaryOperation operation)
        {
            return (float)Call(operation);
        }

        float INumber<ExtendedReal, float>.Call(PrimitiveUnaryOperation operation)
        {
            return (float)Call(operation);
        }

        Real INumber<Real, Real>.Call(PrimitiveUnaryOperation operation)
        {
            return new Real(Call(operation));
        }

        ExtendedReal INumber<ExtendedReal, ExtendedReal>.Call(PrimitiveUnaryOperation operation)
        {
            return new ExtendedReal(Call(operation));
        }

        public override bool Equals(object obj)
        {
            return obj is Real value && Equals(in value) ||  new ExtendedReal(Value).Equals(obj);
        }

        public bool Equals(Real other)
        {
            return Equals(in other);
        }

        public bool Equals(in Real other)
        {
            return Value.Equals(other.Value);
        }

        public bool Equals(ExtendedReal other)
        {
            return Equals(in other);
        }

        public bool Equals(in ExtendedReal other)
        {
            return Value.Equals(other.Value);
        }

        public int CompareTo(Real other)
        {
            return CompareTo(in other);
        }

        public int CompareTo(in Real other)
        {
            return Value.CompareTo(other.Value);
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

        public static implicit operator Real(double value)
        {
            return new Real(value);
        }

        public static implicit operator Real(float value)
        {
            return new Real(value);
        }

        public static implicit operator ExtendedReal(Real value)
        {
            return new ExtendedReal(value.Value);
        }

        public static implicit operator double(Real value)
        {
            return value.Value;
        }

        public static explicit operator float(Real value)
        {
            return (float)value.Value;
        }

        public static bool operator==(Real a, Real b)
        {
            return a.Equals(in b);
        }

        public static bool operator!=(Real a, Real b)
        {
            return !a.Equals(in b);
        }

        public static bool operator>(Real a, Real b)
        {
            return a.CompareTo(in b) > 0;
        }

        public static bool operator<(Real a, Real b)
        {
            return a.CompareTo(in b) < 0;
        }

        public static bool operator>=(Real a, Real b)
        {
            return a.CompareTo(in b) >= 0;
        }

        public static bool operator<=(Real a, Real b)
        {
            return a.CompareTo(in b) <= 0;
        }

        INumberOperations INumber.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<Real> INumber<Real>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<Real, double> INumber<Real, double>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<Real, float> INumber<Real, float>.GetOperations()
        {
            return Operations.Instance;
        }

        INumberOperations<Real, Real> INumber<Real, Real>.GetOperations()
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

        IExtendedNumberOperations<Real, Real> IExtendedNumber<Real, Real>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<Real, Real, double> IExtendedNumber<Real, Real, double>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<Real, Real, float> IExtendedNumber<Real, Real, float>.GetOperations()
        {
            return Operations.Instance;
        }

        IExtendedNumberOperations<Real, Real, Real> IExtendedNumber<Real, Real, Real>.GetOperations()
        {
            return Operations.Instance;
        }

        class Operations : NumberOperations<Real>, INumberOperations<Real, double>, INumberOperations<Real, float>, INumberOperations<Real, Real>, INumberOperations<ExtendedReal, double>, INumberOperations<ExtendedReal, float>, INumberOperations<ExtendedReal, ExtendedReal>, IExtendedNumberOperations<Real, Real, double>, IExtendedNumberOperations<Real, Real, float>, IExtendedNumberOperations<Real, Real, Real>
        {
            public static readonly Operations Instance = new Operations();

            public override int Dimension => 0;

            public bool IsInvertible(in Real num)
            {
                return num.IsInvertible;
            }

            public bool IsFinite(in Real num)
            {
                return num.IsFinite;
            }

            public Real Clone(in Real num)
            {
                return num;
            }

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

            public bool Equals(Real num1, Real num2)
            {
                return num1.Equals(in num2);
            }

            public int Compare(Real num1, Real num2)
            {
                return num1.CompareTo(in num2);
            }

            public bool Equals(in Real num1, in Real num2)
            {
                return num1.Equals(in num2);
            }

            public int Compare(in Real num1, in Real num2)
            {
                return num1.CompareTo(in num2);
            }

            public int GetHashCode(Real num)
            {
                return num.GetHashCode();
            }

            public int GetHashCode(in Real num)
            {
                return num.GetHashCode();
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

            public Real Call(NullaryOperation operation)
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

            ExtendedReal INumberOperations<ExtendedReal>.Call(NullaryOperation operation)
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

            public Real Call(UnaryOperation operation, in Real num)
            {
                return num.Call(operation);
            }

            public Real Call(BinaryOperation operation, in Real num1, in Real num2)
            {
                return num1.Call(operation, num2);
            }

            public double Call(PrimitiveUnaryOperation operation, in Real num)
            {
                return num.Call(operation);
            }

            float INumberOperations<Real, float>.Call(PrimitiveUnaryOperation operation, in Real num)
            {
                return (float)num.Call(operation);
            }

            Real INumberOperations<Real, Real>.Call(PrimitiveUnaryOperation operation, in Real num)
            {
                return new Real(num.Call(operation));
            }

            public Real Call(BinaryOperation operation, in Real num1, double num2)
            {
                return num1.Call(operation, num2);
            }

            public Real Call(BinaryOperation operation, in Real num1, float num2)
            {
                return num1.Call(operation, num2);
            }

            Real INumberOperations<Real, Real>.Call(BinaryOperation operation, in Real num1, Real num2)
            {
                return Call(operation, num1, num2);
            }

            public ExtendedReal Call(UnaryOperation operation, in ExtendedReal num)
            {
                return num.Call(operation);
            }

            public ExtendedReal Call(BinaryOperation operation, in ExtendedReal num1, in ExtendedReal num2)
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

            public Real Create(double realUnit, double otherUnits, double someUnitsCombined, double allUnitsCombined)
            {
                return new Real(realUnit);
            }

            public Real Create(float realUnit, float otherUnits, float someUnitsCombined, float allUnitsCombined)
            {
                return new Real(realUnit);
            }

            public Real Create(Real realUnit, Real otherUnits, Real someUnitsCombined, Real allUnitsCombined)
            {
                return realUnit;
            }

            public ExtendedReal Create(ExtendedReal realUnit, ExtendedReal otherUnits, ExtendedReal someUnitsCombined, ExtendedReal allUnitsCombined)
            {
                return realUnit;
            }

            ExtendedReal INumberOperations<ExtendedReal, double>.Create(double realUnit, double otherUnits, double someUnitsCombined, double allUnitsCombined)
            {
                return new ExtendedReal(realUnit);
            }

            ExtendedReal INumberOperations<ExtendedReal, float>.Create(float realUnit, float otherUnits, float someUnitsCombined, float allUnitsCombined)
            {
                return new ExtendedReal(realUnit);
            }

            public Real Create(in Real num)
            {
                return num;
            }

            public Real Create(IEnumerable<double> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new Real(ienum.Current);
            }

            public Real Create(IEnumerator<double> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new Real(value);
            }

            public Real Create(IEnumerable<float> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new Real(ienum.Current);
            }

            public Real Create(IEnumerator<float> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new Real(value);
            }

            public Real Create(IEnumerable<Real> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return ienum.Current;
            }

            public Real Create(IEnumerator<Real> units)
            {
                var value = units.Current;
                units.MoveNext();
                return value;
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

            ExtendedReal INumberOperations<ExtendedReal, double>.Create(IEnumerable<double> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new ExtendedReal(ienum.Current);
            }

            ExtendedReal INumberOperations<ExtendedReal, double>.Create(IEnumerator<double> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new ExtendedReal(value);
            }

            ExtendedReal INumberOperations<ExtendedReal, float>.Create(IEnumerable<float> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return new ExtendedReal(ienum.Current);
            }

            ExtendedReal INumberOperations<ExtendedReal, float>.Create(IEnumerator<float> units)
            {
                var value = units.Current;
                units.MoveNext();
                return new ExtendedReal(value);
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

        int ICollection<Real>.Count => 1;

        bool ICollection<Real>.IsReadOnly => true;

        int IReadOnlyCollection<Real>.Count => 1;

        Real IReadOnlyList<Real>.this[int index] => index == 0 ? this : throw new ArgumentOutOfRangeException(nameof(index));

        Real IList<Real>.this[int index]
        {
            get{
                return index == 0 ? this : throw new ArgumentOutOfRangeException(nameof(index));
            }
            set{
                throw new NotSupportedException();
            }
        }

        int IList<Real>.IndexOf(Real item)
        {
            return Equals(in item) ? 0 : -1;
        }

        void IList<Real>.Insert(int index, Real item)
        {
            throw new NotSupportedException();
        }

        void IList<Real>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void ICollection<Real>.Add(Real item)
        {
            throw new NotSupportedException();
        }

        void ICollection<Real>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<Real>.Contains(Real item)
        {
            return Equals(in item);
        }

        void ICollection<Real>.CopyTo(Real[] array, int arrayIndex)
        {
            array[arrayIndex] = this;
        }

        bool ICollection<Real>.Remove(Real item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<Real> IEnumerable<Real>.GetEnumerator()
        {
            yield return this;
        }

        int ICollection<ExtendedReal>.Count => 1;

        bool ICollection<ExtendedReal>.IsReadOnly => true;

        int IReadOnlyCollection<ExtendedReal>.Count => 1;

        ExtendedReal IReadOnlyList<ExtendedReal>.this[int index] => index == 0 ? new ExtendedReal(Value) : throw new ArgumentOutOfRangeException(nameof(index));

        ExtendedReal IList<ExtendedReal>.this[int index]
        {
            get{
                return index == 0 ? new ExtendedReal(Value) : throw new ArgumentOutOfRangeException(nameof(index));
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
            array[arrayIndex] = new ExtendedReal(Value);
        }

        bool ICollection<ExtendedReal>.Remove(ExtendedReal item)
        {
            throw new NotSupportedException();
        }

        IEnumerator<ExtendedReal> IEnumerable<ExtendedReal>.GetEnumerator()
        {
            yield return new ExtendedReal(Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }
    }
}
