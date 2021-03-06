﻿using IS4.HyperNumerics.Operations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IS4.HyperNumerics.NumberTypes
{
	partial struct HyperComplex<TInner> : IHyperNumber<HyperComplex<TInner>, TInner>, INumber<HyperComplex<TInner>, TInner> where TInner : struct, INumber<TInner>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperComplex(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperComplex(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperComplex(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperComplex<TInner> Clone()
        {
            return new HyperComplex<TInner>(first.Clone(), second.Clone());
        }

        public HyperComplex<TInner> WithFirst(in TInner first)
        {
            return new HyperComplex<TInner>(first, second);
        }

        public HyperComplex<TInner> WithSecond(in TInner second)
        {
            return new HyperComplex<TInner>(first, second);
        }

        public static implicit operator HyperComplex<TInner>((TInner first, TInner second) tuple)
        {
            return new HyperComplex<TInner>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperComplex<TInner> value)
        {
            return (value.first, value.second);
        }

        public HyperComplex<TInner> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner>(HyperMath.Call(operation, first, other), second);
        }

        public HyperComplex<TInner> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner>(HyperMath.Call(operation, other, first), second);
        }

        public HyperComplex<TInner> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner>(first, HyperMath.Call(operation, second, other));
        }

        public HyperComplex<TInner> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner>(first, HyperMath.Call(operation, other, second));
        }
		
        public HyperComplex<TInner> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperComplex<TInner>(HyperMath.Call(operation, first), second);
        }

        public HyperComplex<TInner> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperComplex<TInner>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperComplex<TInner> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperComplex<TInner> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Complex(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Complex(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperComplex<TInner>, TInner> IHyperNumber<HyperComplex<TInner>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperComplex<TInner>>, IHyperNumberOperations<HyperComplex<TInner>, TInner>, INumberOperations<HyperComplex<TInner>, TInner>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperComplex<TInner> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperComplex<TInner>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperComplex<TInner>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperComplex<TInner> Create(in TInner first, in TInner second)
            {
                return new HyperComplex<TInner>(first, second);
            }

			public virtual TInner GetFirst(in HyperComplex<TInner> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperComplex<TInner> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperComplex<TInner> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperComplex<TInner> num)
			{
				return ref num.second;
			}
			
			public virtual HyperComplex<TInner> WithFirst(in HyperComplex<TInner> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperComplex<TInner> WithSecond(in HyperComplex<TInner> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperComplex<TInner> FirstCall(StandardUnaryOperation operation, in HyperComplex<TInner> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperComplex<TInner> SecondCall(StandardUnaryOperation operation, in HyperComplex<TInner> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperComplex<TInner> FirstCall(StandardBinaryOperation operation, in HyperComplex<TInner> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperComplex<TInner> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperComplex<TInner> SecondCall(StandardBinaryOperation operation, in HyperComplex<TInner> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperComplex<TInner> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			

            public virtual HyperComplex<TInner> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new HyperComplex<TInner>(realUnit, otherUnits);
            }

            public virtual HyperComplex<TInner> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperComplex<TInner> Create(IEnumerator<TInner> units)
            {
                var first = units.Current;
				units.MoveNext();
                var second = units.Current;
				units.MoveNext();
                return new HyperComplex<TInner>(first, second);
            }
		}

		int ICollection<TInner>.Count => 2;

		int IReadOnlyCollection<TInner>.Count => 2;

		TInner IReadOnlyList<TInner>.this[int index] => index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));

		TInner IList<TInner>.this[int index]
		{
			get{
				return index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));
			}
			set{
				throw new NotSupportedException();
			}
		}

		int IList<TInner>.IndexOf(TInner item)
		{
			return item.Equals(in first) ? 0 : item.Equals(in second) ? 1 : -1;
		}

		bool ICollection<TInner>.Contains(TInner item)
		{
			return item.Equals(in first) || item.Equals(in second);
		}

		void ICollection<TInner>.CopyTo(TInner[] array, int arrayIndex)
		{
			array[arrayIndex] = first;
			array[arrayIndex + 1] = second;
		}

		IEnumerator<TInner> IEnumerable<TInner>.GetEnumerator()
		{
			yield return first;
			yield return second;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var obj in first)
			{
				yield return obj;
			}
			foreach(var obj in second)
			{
				yield return obj;
			}
        }
	}

	partial struct HyperComplex<TInner, TComponent> : IHyperNumber<HyperComplex<TInner, TComponent>, TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperComplex(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperComplex(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperComplex(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperComplex<TInner, TComponent> Clone()
        {
            return new HyperComplex<TInner, TComponent>(first.Clone(), second.Clone());
        }

        public HyperComplex<TInner, TComponent> WithFirst(in TInner first)
        {
            return new HyperComplex<TInner, TComponent>(first, second);
        }

        public HyperComplex<TInner, TComponent> WithSecond(in TInner second)
        {
            return new HyperComplex<TInner, TComponent>(first, second);
        }

        public static implicit operator HyperComplex<TInner, TComponent>((TInner first, TInner second) tuple)
        {
            return new HyperComplex<TInner, TComponent>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperComplex<TInner, TComponent> value)
        {
            return (value.first, value.second);
        }

        public HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner, TComponent>(HyperMath.Call(operation, first, other), second);
        }

        public HyperComplex<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner, TComponent>(HyperMath.Call(operation, other, first), second);
        }

        public HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner, TComponent>(first, HyperMath.Call(operation, second, other));
        }

        public HyperComplex<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperComplex<TInner, TComponent>(first, HyperMath.Call(operation, other, second));
        }
				
        public HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperComplex<TInner, TComponent>(HyperMath.CallComponent(operation, first, other), second);
        }

        public HyperComplex<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperComplex<TInner, TComponent>(HyperMath.CallComponentReversed(operation, other, first), second);
        }

        public HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperComplex<TInner, TComponent>(first, HyperMath.CallComponent(operation, second, other));
        }

        public HyperComplex<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperComplex<TInner, TComponent>(first, HyperMath.CallComponentReversed(operation, other, second));
        }
		
        public HyperComplex<TInner, TComponent> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperComplex<TInner, TComponent>(HyperMath.Call(operation, first), second);
        }

        public HyperComplex<TInner, TComponent> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperComplex<TInner, TComponent>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperComplex<TInner, TComponent> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperComplex<TInner, TComponent> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Complex(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Complex(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperComplex<TInner, TComponent>, TInner> IHyperNumber<HyperComplex<TInner, TComponent>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }		

        IHyperNumberOperations<HyperComplex<TInner, TComponent>, TInner, TComponent> IHyperNumber<HyperComplex<TInner, TComponent>, TInner, TComponent>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperComplex<TInner, TComponent>>, IHyperNumberOperations<HyperComplex<TInner, TComponent>, TInner, TComponent>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperComplex<TInner, TComponent> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperComplex<TInner, TComponent>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperComplex<TInner, TComponent> Create(in TInner first, in TInner second)
            {
                return new HyperComplex<TInner, TComponent>(first, second);
            }

			public virtual TInner GetFirst(in HyperComplex<TInner, TComponent> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperComplex<TInner, TComponent> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperComplex<TInner, TComponent> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperComplex<TInner, TComponent> num)
			{
				return ref num.second;
			}
			
			public virtual HyperComplex<TInner, TComponent> WithFirst(in HyperComplex<TInner, TComponent> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperComplex<TInner, TComponent> WithSecond(in HyperComplex<TInner, TComponent> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperComplex<TInner, TComponent> FirstCall(StandardUnaryOperation operation, in HyperComplex<TInner, TComponent> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperComplex<TInner, TComponent> SecondCall(StandardUnaryOperation operation, in HyperComplex<TInner, TComponent> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperComplex<TInner, TComponent> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperComplex<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperComplex<TInner, TComponent> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperComplex<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			
			
			public virtual HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperComplex<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent num1, in HyperComplex<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperComplex<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent num1, in HyperComplex<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }
			
            public virtual HyperComplex<TInner, TComponent> Create(in TComponent num)
            {
                return new HyperComplex<TInner, TComponent>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual HyperComplex<TInner, TComponent> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return new HyperComplex<TInner, TComponent>(HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, someUnitsCombined), HyperMath.Create<TInner, TComponent>(otherUnits, someUnitsCombined, someUnitsCombined, allUnitsCombined));
            }

            public virtual HyperComplex<TInner, TComponent> Create(IEnumerable<TComponent> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperComplex<TInner, TComponent> Create(IEnumerator<TComponent> units)
            {
                var first = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                var second = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                return new HyperComplex<TInner, TComponent>(first, second);
            }
		}
	}

	partial struct HyperDiagonal<TInner> : IHyperNumber<HyperDiagonal<TInner>, TInner>, INumber<HyperDiagonal<TInner>, TInner> where TInner : struct, INumber<TInner>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperDiagonal(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperDiagonal(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperDiagonal(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperDiagonal<TInner> Clone()
        {
            return new HyperDiagonal<TInner>(first.Clone(), second.Clone());
        }

        public HyperDiagonal<TInner> WithFirst(in TInner first)
        {
            return new HyperDiagonal<TInner>(first, second);
        }

        public HyperDiagonal<TInner> WithSecond(in TInner second)
        {
            return new HyperDiagonal<TInner>(first, second);
        }

        public static implicit operator HyperDiagonal<TInner>((TInner first, TInner second) tuple)
        {
            return new HyperDiagonal<TInner>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperDiagonal<TInner> value)
        {
            return (value.first, value.second);
        }

        public HyperDiagonal<TInner> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner>(HyperMath.Call(operation, first, other), second);
        }

        public HyperDiagonal<TInner> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner>(HyperMath.Call(operation, other, first), second);
        }

        public HyperDiagonal<TInner> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner>(first, HyperMath.Call(operation, second, other));
        }

        public HyperDiagonal<TInner> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner>(first, HyperMath.Call(operation, other, second));
        }
		
        public HyperDiagonal<TInner> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperDiagonal<TInner>(HyperMath.Call(operation, first), second);
        }

        public HyperDiagonal<TInner> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperDiagonal<TInner>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperDiagonal<TInner> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperDiagonal<TInner> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Diagonal(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Diagonal(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperDiagonal<TInner>, TInner> IHyperNumber<HyperDiagonal<TInner>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperDiagonal<TInner>>, IHyperNumberOperations<HyperDiagonal<TInner>, TInner>, INumberOperations<HyperDiagonal<TInner>, TInner>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperDiagonal<TInner> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperDiagonal<TInner>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperDiagonal<TInner>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperDiagonal<TInner> Create(in TInner first, in TInner second)
            {
                return new HyperDiagonal<TInner>(first, second);
            }

			public virtual TInner GetFirst(in HyperDiagonal<TInner> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperDiagonal<TInner> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperDiagonal<TInner> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperDiagonal<TInner> num)
			{
				return ref num.second;
			}
			
			public virtual HyperDiagonal<TInner> WithFirst(in HyperDiagonal<TInner> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperDiagonal<TInner> WithSecond(in HyperDiagonal<TInner> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperDiagonal<TInner> FirstCall(StandardUnaryOperation operation, in HyperDiagonal<TInner> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperDiagonal<TInner> SecondCall(StandardUnaryOperation operation, in HyperDiagonal<TInner> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperDiagonal<TInner> FirstCall(StandardBinaryOperation operation, in HyperDiagonal<TInner> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperDiagonal<TInner> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDiagonal<TInner> SecondCall(StandardBinaryOperation operation, in HyperDiagonal<TInner> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperDiagonal<TInner> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			

            public virtual HyperDiagonal<TInner> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new HyperDiagonal<TInner>(realUnit, otherUnits);
            }

            public virtual HyperDiagonal<TInner> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperDiagonal<TInner> Create(IEnumerator<TInner> units)
            {
                var first = units.Current;
				units.MoveNext();
                var second = units.Current;
				units.MoveNext();
                return new HyperDiagonal<TInner>(first, second);
            }
		}

		int ICollection<TInner>.Count => 2;

		int IReadOnlyCollection<TInner>.Count => 2;

		TInner IReadOnlyList<TInner>.this[int index] => index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));

		TInner IList<TInner>.this[int index]
		{
			get{
				return index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));
			}
			set{
				throw new NotSupportedException();
			}
		}

		int IList<TInner>.IndexOf(TInner item)
		{
			return item.Equals(in first) ? 0 : item.Equals(in second) ? 1 : -1;
		}

		bool ICollection<TInner>.Contains(TInner item)
		{
			return item.Equals(in first) || item.Equals(in second);
		}

		void ICollection<TInner>.CopyTo(TInner[] array, int arrayIndex)
		{
			array[arrayIndex] = first;
			array[arrayIndex + 1] = second;
		}

		IEnumerator<TInner> IEnumerable<TInner>.GetEnumerator()
		{
			yield return first;
			yield return second;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var obj in first)
			{
				yield return obj;
			}
			foreach(var obj in second)
			{
				yield return obj;
			}
        }
	}

	partial struct HyperDiagonal<TInner, TComponent> : IHyperNumber<HyperDiagonal<TInner, TComponent>, TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperDiagonal(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperDiagonal(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperDiagonal(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperDiagonal<TInner, TComponent> Clone()
        {
            return new HyperDiagonal<TInner, TComponent>(first.Clone(), second.Clone());
        }

        public HyperDiagonal<TInner, TComponent> WithFirst(in TInner first)
        {
            return new HyperDiagonal<TInner, TComponent>(first, second);
        }

        public HyperDiagonal<TInner, TComponent> WithSecond(in TInner second)
        {
            return new HyperDiagonal<TInner, TComponent>(first, second);
        }

        public static implicit operator HyperDiagonal<TInner, TComponent>((TInner first, TInner second) tuple)
        {
            return new HyperDiagonal<TInner, TComponent>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperDiagonal<TInner, TComponent> value)
        {
            return (value.first, value.second);
        }

        public HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner, TComponent>(HyperMath.Call(operation, first, other), second);
        }

        public HyperDiagonal<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner, TComponent>(HyperMath.Call(operation, other, first), second);
        }

        public HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner, TComponent>(first, HyperMath.Call(operation, second, other));
        }

        public HyperDiagonal<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDiagonal<TInner, TComponent>(first, HyperMath.Call(operation, other, second));
        }
				
        public HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDiagonal<TInner, TComponent>(HyperMath.CallComponent(operation, first, other), second);
        }

        public HyperDiagonal<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDiagonal<TInner, TComponent>(HyperMath.CallComponentReversed(operation, other, first), second);
        }

        public HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDiagonal<TInner, TComponent>(first, HyperMath.CallComponent(operation, second, other));
        }

        public HyperDiagonal<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDiagonal<TInner, TComponent>(first, HyperMath.CallComponentReversed(operation, other, second));
        }
		
        public HyperDiagonal<TInner, TComponent> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperDiagonal<TInner, TComponent>(HyperMath.Call(operation, first), second);
        }

        public HyperDiagonal<TInner, TComponent> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperDiagonal<TInner, TComponent>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperDiagonal<TInner, TComponent> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperDiagonal<TInner, TComponent> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Diagonal(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Diagonal(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperDiagonal<TInner, TComponent>, TInner> IHyperNumber<HyperDiagonal<TInner, TComponent>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }		

        IHyperNumberOperations<HyperDiagonal<TInner, TComponent>, TInner, TComponent> IHyperNumber<HyperDiagonal<TInner, TComponent>, TInner, TComponent>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperDiagonal<TInner, TComponent>>, IHyperNumberOperations<HyperDiagonal<TInner, TComponent>, TInner, TComponent>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperDiagonal<TInner, TComponent> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperDiagonal<TInner, TComponent>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperDiagonal<TInner, TComponent> Create(in TInner first, in TInner second)
            {
                return new HyperDiagonal<TInner, TComponent>(first, second);
            }

			public virtual TInner GetFirst(in HyperDiagonal<TInner, TComponent> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperDiagonal<TInner, TComponent> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperDiagonal<TInner, TComponent> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperDiagonal<TInner, TComponent> num)
			{
				return ref num.second;
			}
			
			public virtual HyperDiagonal<TInner, TComponent> WithFirst(in HyperDiagonal<TInner, TComponent> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> WithSecond(in HyperDiagonal<TInner, TComponent> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> FirstCall(StandardUnaryOperation operation, in HyperDiagonal<TInner, TComponent> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> SecondCall(StandardUnaryOperation operation, in HyperDiagonal<TInner, TComponent> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperDiagonal<TInner, TComponent> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperDiagonal<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperDiagonal<TInner, TComponent> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperDiagonal<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			
			
			public virtual HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperDiagonal<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent num1, in HyperDiagonal<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperDiagonal<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDiagonal<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent num1, in HyperDiagonal<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }
			
            public virtual HyperDiagonal<TInner, TComponent> Create(in TComponent num)
            {
                return new HyperDiagonal<TInner, TComponent>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual HyperDiagonal<TInner, TComponent> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return new HyperDiagonal<TInner, TComponent>(HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, someUnitsCombined), HyperMath.Create<TInner, TComponent>(otherUnits, someUnitsCombined, someUnitsCombined, allUnitsCombined));
            }

            public virtual HyperDiagonal<TInner, TComponent> Create(IEnumerable<TComponent> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperDiagonal<TInner, TComponent> Create(IEnumerator<TComponent> units)
            {
                var first = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                var second = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                return new HyperDiagonal<TInner, TComponent>(first, second);
            }
		}
	}

	partial struct HyperDual<TInner> : IHyperNumber<HyperDual<TInner>, TInner>, INumber<HyperDual<TInner>, TInner> where TInner : struct, INumber<TInner>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperDual(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperDual(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperDual(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperDual<TInner> Clone()
        {
            return new HyperDual<TInner>(first.Clone(), second.Clone());
        }

        public HyperDual<TInner> WithFirst(in TInner first)
        {
            return new HyperDual<TInner>(first, second);
        }

        public HyperDual<TInner> WithSecond(in TInner second)
        {
            return new HyperDual<TInner>(first, second);
        }

        public static implicit operator HyperDual<TInner>((TInner first, TInner second) tuple)
        {
            return new HyperDual<TInner>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperDual<TInner> value)
        {
            return (value.first, value.second);
        }

        public HyperDual<TInner> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner>(HyperMath.Call(operation, first, other), second);
        }

        public HyperDual<TInner> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner>(HyperMath.Call(operation, other, first), second);
        }

        public HyperDual<TInner> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner>(first, HyperMath.Call(operation, second, other));
        }

        public HyperDual<TInner> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner>(first, HyperMath.Call(operation, other, second));
        }
		
        public HyperDual<TInner> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperDual<TInner>(HyperMath.Call(operation, first), second);
        }

        public HyperDual<TInner> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperDual<TInner>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperDual<TInner> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperDual<TInner> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Dual(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Dual(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperDual<TInner>, TInner> IHyperNumber<HyperDual<TInner>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperDual<TInner>>, IHyperNumberOperations<HyperDual<TInner>, TInner>, INumberOperations<HyperDual<TInner>, TInner>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperDual<TInner> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperDual<TInner>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperDual<TInner>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperDual<TInner> Create(in TInner first, in TInner second)
            {
                return new HyperDual<TInner>(first, second);
            }

			public virtual TInner GetFirst(in HyperDual<TInner> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperDual<TInner> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperDual<TInner> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperDual<TInner> num)
			{
				return ref num.second;
			}
			
			public virtual HyperDual<TInner> WithFirst(in HyperDual<TInner> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperDual<TInner> WithSecond(in HyperDual<TInner> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperDual<TInner> FirstCall(StandardUnaryOperation operation, in HyperDual<TInner> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperDual<TInner> SecondCall(StandardUnaryOperation operation, in HyperDual<TInner> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperDual<TInner> FirstCall(StandardBinaryOperation operation, in HyperDual<TInner> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDual<TInner> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperDual<TInner> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDual<TInner> SecondCall(StandardBinaryOperation operation, in HyperDual<TInner> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDual<TInner> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperDual<TInner> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			

            public virtual HyperDual<TInner> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new HyperDual<TInner>(realUnit, otherUnits);
            }

            public virtual HyperDual<TInner> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperDual<TInner> Create(IEnumerator<TInner> units)
            {
                var first = units.Current;
				units.MoveNext();
                var second = units.Current;
				units.MoveNext();
                return new HyperDual<TInner>(first, second);
            }
		}

		int ICollection<TInner>.Count => 2;

		int IReadOnlyCollection<TInner>.Count => 2;

		TInner IReadOnlyList<TInner>.this[int index] => index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));

		TInner IList<TInner>.this[int index]
		{
			get{
				return index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));
			}
			set{
				throw new NotSupportedException();
			}
		}

		int IList<TInner>.IndexOf(TInner item)
		{
			return item.Equals(in first) ? 0 : item.Equals(in second) ? 1 : -1;
		}

		bool ICollection<TInner>.Contains(TInner item)
		{
			return item.Equals(in first) || item.Equals(in second);
		}

		void ICollection<TInner>.CopyTo(TInner[] array, int arrayIndex)
		{
			array[arrayIndex] = first;
			array[arrayIndex + 1] = second;
		}

		IEnumerator<TInner> IEnumerable<TInner>.GetEnumerator()
		{
			yield return first;
			yield return second;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var obj in first)
			{
				yield return obj;
			}
			foreach(var obj in second)
			{
				yield return obj;
			}
        }
	}

	partial struct HyperDual<TInner, TComponent> : IHyperNumber<HyperDual<TInner, TComponent>, TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperDual(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperDual(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperDual(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperDual<TInner, TComponent> Clone()
        {
            return new HyperDual<TInner, TComponent>(first.Clone(), second.Clone());
        }

        public HyperDual<TInner, TComponent> WithFirst(in TInner first)
        {
            return new HyperDual<TInner, TComponent>(first, second);
        }

        public HyperDual<TInner, TComponent> WithSecond(in TInner second)
        {
            return new HyperDual<TInner, TComponent>(first, second);
        }

        public static implicit operator HyperDual<TInner, TComponent>((TInner first, TInner second) tuple)
        {
            return new HyperDual<TInner, TComponent>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperDual<TInner, TComponent> value)
        {
            return (value.first, value.second);
        }

        public HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner, TComponent>(HyperMath.Call(operation, first, other), second);
        }

        public HyperDual<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner, TComponent>(HyperMath.Call(operation, other, first), second);
        }

        public HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner, TComponent>(first, HyperMath.Call(operation, second, other));
        }

        public HyperDual<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperDual<TInner, TComponent>(first, HyperMath.Call(operation, other, second));
        }
				
        public HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDual<TInner, TComponent>(HyperMath.CallComponent(operation, first, other), second);
        }

        public HyperDual<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDual<TInner, TComponent>(HyperMath.CallComponentReversed(operation, other, first), second);
        }

        public HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDual<TInner, TComponent>(first, HyperMath.CallComponent(operation, second, other));
        }

        public HyperDual<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperDual<TInner, TComponent>(first, HyperMath.CallComponentReversed(operation, other, second));
        }
		
        public HyperDual<TInner, TComponent> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperDual<TInner, TComponent>(HyperMath.Call(operation, first), second);
        }

        public HyperDual<TInner, TComponent> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperDual<TInner, TComponent>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperDual<TInner, TComponent> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperDual<TInner, TComponent> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "Dual(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "Dual(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperDual<TInner, TComponent>, TInner> IHyperNumber<HyperDual<TInner, TComponent>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }		

        IHyperNumberOperations<HyperDual<TInner, TComponent>, TInner, TComponent> IHyperNumber<HyperDual<TInner, TComponent>, TInner, TComponent>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperDual<TInner, TComponent>>, IHyperNumberOperations<HyperDual<TInner, TComponent>, TInner, TComponent>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperDual<TInner, TComponent> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperDual<TInner, TComponent>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperDual<TInner, TComponent> Create(in TInner first, in TInner second)
            {
                return new HyperDual<TInner, TComponent>(first, second);
            }

			public virtual TInner GetFirst(in HyperDual<TInner, TComponent> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperDual<TInner, TComponent> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperDual<TInner, TComponent> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperDual<TInner, TComponent> num)
			{
				return ref num.second;
			}
			
			public virtual HyperDual<TInner, TComponent> WithFirst(in HyperDual<TInner, TComponent> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperDual<TInner, TComponent> WithSecond(in HyperDual<TInner, TComponent> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperDual<TInner, TComponent> FirstCall(StandardUnaryOperation operation, in HyperDual<TInner, TComponent> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperDual<TInner, TComponent> SecondCall(StandardUnaryOperation operation, in HyperDual<TInner, TComponent> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperDual<TInner, TComponent> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperDual<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperDual<TInner, TComponent> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperDual<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			
			
			public virtual HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperDual<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperDual<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent num1, in HyperDual<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperDual<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperDual<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent num1, in HyperDual<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }
			
            public virtual HyperDual<TInner, TComponent> Create(in TComponent num)
            {
                return new HyperDual<TInner, TComponent>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual HyperDual<TInner, TComponent> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return new HyperDual<TInner, TComponent>(HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, someUnitsCombined), HyperMath.Create<TInner, TComponent>(otherUnits, someUnitsCombined, someUnitsCombined, allUnitsCombined));
            }

            public virtual HyperDual<TInner, TComponent> Create(IEnumerable<TComponent> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperDual<TInner, TComponent> Create(IEnumerator<TComponent> units)
            {
                var first = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                var second = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                return new HyperDual<TInner, TComponent>(first, second);
            }
		}
	}

	partial struct HyperSplitComplex<TInner> : IHyperNumber<HyperSplitComplex<TInner>, TInner>, INumber<HyperSplitComplex<TInner>, TInner> where TInner : struct, INumber<TInner>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperSplitComplex(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperSplitComplex(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperSplitComplex(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperSplitComplex<TInner> Clone()
        {
            return new HyperSplitComplex<TInner>(first.Clone(), second.Clone());
        }

        public HyperSplitComplex<TInner> WithFirst(in TInner first)
        {
            return new HyperSplitComplex<TInner>(first, second);
        }

        public HyperSplitComplex<TInner> WithSecond(in TInner second)
        {
            return new HyperSplitComplex<TInner>(first, second);
        }

        public static implicit operator HyperSplitComplex<TInner>((TInner first, TInner second) tuple)
        {
            return new HyperSplitComplex<TInner>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperSplitComplex<TInner> value)
        {
            return (value.first, value.second);
        }

        public HyperSplitComplex<TInner> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner>(HyperMath.Call(operation, first, other), second);
        }

        public HyperSplitComplex<TInner> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner>(HyperMath.Call(operation, other, first), second);
        }

        public HyperSplitComplex<TInner> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner>(first, HyperMath.Call(operation, second, other));
        }

        public HyperSplitComplex<TInner> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner>(first, HyperMath.Call(operation, other, second));
        }
		
        public HyperSplitComplex<TInner> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperSplitComplex<TInner>(HyperMath.Call(operation, first), second);
        }

        public HyperSplitComplex<TInner> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperSplitComplex<TInner>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperSplitComplex<TInner> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperSplitComplex<TInner> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "SplitComplex(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "SplitComplex(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperSplitComplex<TInner>, TInner> IHyperNumber<HyperSplitComplex<TInner>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperSplitComplex<TInner>>, IHyperNumberOperations<HyperSplitComplex<TInner>, TInner>, INumberOperations<HyperSplitComplex<TInner>, TInner>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperSplitComplex<TInner> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperSplitComplex<TInner>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperSplitComplex<TInner>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperSplitComplex<TInner> Create(in TInner first, in TInner second)
            {
                return new HyperSplitComplex<TInner>(first, second);
            }

			public virtual TInner GetFirst(in HyperSplitComplex<TInner> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperSplitComplex<TInner> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperSplitComplex<TInner> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperSplitComplex<TInner> num)
			{
				return ref num.second;
			}
			
			public virtual HyperSplitComplex<TInner> WithFirst(in HyperSplitComplex<TInner> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperSplitComplex<TInner> WithSecond(in HyperSplitComplex<TInner> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperSplitComplex<TInner> FirstCall(StandardUnaryOperation operation, in HyperSplitComplex<TInner> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperSplitComplex<TInner> SecondCall(StandardUnaryOperation operation, in HyperSplitComplex<TInner> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperSplitComplex<TInner> FirstCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperSplitComplex<TInner> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperSplitComplex<TInner> SecondCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperSplitComplex<TInner> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			

            public virtual HyperSplitComplex<TInner> Create(in TInner realUnit, in TInner otherUnits, in TInner someUnitsCombined, in TInner allUnitsCombined)
            {
                return new HyperSplitComplex<TInner>(realUnit, otherUnits);
            }

            public virtual HyperSplitComplex<TInner> Create(IEnumerable<TInner> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperSplitComplex<TInner> Create(IEnumerator<TInner> units)
            {
                var first = units.Current;
				units.MoveNext();
                var second = units.Current;
				units.MoveNext();
                return new HyperSplitComplex<TInner>(first, second);
            }
		}

		int ICollection<TInner>.Count => 2;

		int IReadOnlyCollection<TInner>.Count => 2;

		TInner IReadOnlyList<TInner>.this[int index] => index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));

		TInner IList<TInner>.this[int index]
		{
			get{
				return index == 0 ? first : index == 1 ? second : throw new ArgumentOutOfRangeException(nameof(index));
			}
			set{
				throw new NotSupportedException();
			}
		}

		int IList<TInner>.IndexOf(TInner item)
		{
			return item.Equals(in first) ? 0 : item.Equals(in second) ? 1 : -1;
		}

		bool ICollection<TInner>.Contains(TInner item)
		{
			return item.Equals(in first) || item.Equals(in second);
		}

		void ICollection<TInner>.CopyTo(TInner[] array, int arrayIndex)
		{
			array[arrayIndex] = first;
			array[arrayIndex + 1] = second;
		}

		IEnumerator<TInner> IEnumerable<TInner>.GetEnumerator()
		{
			yield return first;
			yield return second;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var obj in first)
			{
				yield return obj;
			}
			foreach(var obj in second)
			{
				yield return obj;
			}
        }
	}

	partial struct HyperSplitComplex<TInner, TComponent> : IHyperNumber<HyperSplitComplex<TInner, TComponent>, TInner, TComponent> where TInner : struct, INumber<TInner, TComponent> where TComponent : struct, IEquatable<TComponent>, IComparable<TComponent>
	{
        readonly TInner first;
        readonly TInner second;

        public TInner First => first;
        public TInner Second => second;

        TInner IWrapperNumber<TInner>.Value => first;

        int INumber.Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

        public HyperSplitComplex(in TInner first)
        {
            this.first = first;
            second = HyperMath.Create<TInner>(StandardNumber.Zero);
        }

        public HyperSplitComplex(in TInner first, in TInner second)
        {
            this.first = first;
            this.second = second;
        }

        public HyperSplitComplex(in (TInner first, TInner second) tuple)
        {
            first = tuple.first;
            second = tuple.second;
        }

        public void Deconstruct(out TInner first, out TInner second)
        {
            first = this.first;
            second = this.second;
        }

        public HyperSplitComplex<TInner, TComponent> Clone()
        {
            return new HyperSplitComplex<TInner, TComponent>(first.Clone(), second.Clone());
        }

        public HyperSplitComplex<TInner, TComponent> WithFirst(in TInner first)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, second);
        }

        public HyperSplitComplex<TInner, TComponent> WithSecond(in TInner second)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, second);
        }

        public static implicit operator HyperSplitComplex<TInner, TComponent>((TInner first, TInner second) tuple)
        {
            return new HyperSplitComplex<TInner, TComponent>(tuple.first, tuple.second);
        }

        public static implicit operator (TInner first, TInner second)(HyperSplitComplex<TInner, TComponent> value)
        {
            return (value.first, value.second);
        }

        public HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner, TComponent>(HyperMath.Call(operation, first, other), second);
        }

        public HyperSplitComplex<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner, TComponent>(HyperMath.Call(operation, other, first), second);
        }

        public HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, HyperMath.Call(operation, second, other));
        }

        public HyperSplitComplex<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TInner other)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, HyperMath.Call(operation, other, second));
        }
				
        public HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperSplitComplex<TInner, TComponent>(HyperMath.CallComponent(operation, first, other), second);
        }

        public HyperSplitComplex<TInner, TComponent> FirstCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperSplitComplex<TInner, TComponent>(HyperMath.CallComponentReversed(operation, other, first), second);
        }

        public HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, HyperMath.CallComponent(operation, second, other));
        }

        public HyperSplitComplex<TInner, TComponent> SecondCallReversed(StandardBinaryOperation operation, in TComponent other)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, HyperMath.CallComponentReversed(operation, other, second));
        }
		
        public HyperSplitComplex<TInner, TComponent> FirstCall(StandardUnaryOperation operation)
        {
            return new HyperSplitComplex<TInner, TComponent>(HyperMath.Call(operation, first), second);
        }

        public HyperSplitComplex<TInner, TComponent> SecondCall(StandardUnaryOperation operation)
        {
            return new HyperSplitComplex<TInner, TComponent>(first, HyperMath.Call(operation, second));
        }

        public bool Equals(in HyperSplitComplex<TInner, TComponent> other)
        {
            return HyperMath.Equals(first, other.first) && HyperMath.Equals(second, other.second);
        }

        public int CompareTo(in HyperSplitComplex<TInner, TComponent> other)
        {
            int value = HyperMath.Compare(first, other.first);
            return value != 0 ? value : HyperMath.Compare(second, other.second);
        }

        public bool Equals(in TInner other)
        {
            return HyperMath.Equals(first, other) && HyperMath.Create<TInner>(StandardNumber.Zero).Equals(second);
        }

        public int CompareTo(in TInner other)
        {
            int value = HyperMath.Compare(first, other);
            return value != 0 ? value : -HyperMath.Create<TInner>(StandardNumber.Zero).CompareTo(second);
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() * 17 + second.GetHashCode();
        }

        public override string ToString()
        {
            return "SplitComplex(" + first.ToString() + ", " + second.ToString() + ")";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "SplitComplex(" + first.ToString(format, formatProvider) + ", " + second.ToString(format, formatProvider) + ")";
        }

        IHyperNumberOperations<HyperSplitComplex<TInner, TComponent>, TInner> IHyperNumber<HyperSplitComplex<TInner, TComponent>, TInner>.GetOperations()
        {
            return Operations.Instance;
        }		

        IHyperNumberOperations<HyperSplitComplex<TInner, TComponent>, TInner, TComponent> IHyperNumber<HyperSplitComplex<TInner, TComponent>, TInner, TComponent>.GetOperations()
        {
            return Operations.Instance;
        }

		partial class Operations : NumberOperations<HyperSplitComplex<TInner, TComponent>>, IHyperNumberOperations<HyperSplitComplex<TInner, TComponent>, TInner, TComponent>
		{
            public override int Dimension => HyperMath.Operations.For<TInner>.Instance.Dimension * 2;

			public virtual HyperSplitComplex<TInner, TComponent> Create(StandardNumber num)
            {
                switch(num)
                {
                    case StandardNumber.Zero:
                    {
                        var zero = HyperMath.Create<TInner>(StandardNumber.Zero);
                        return new HyperSplitComplex<TInner, TComponent>(zero, zero);
                    }
                    case StandardNumber.SpecialOne:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.UnitsOne:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.UnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.NonRealUnitsOne:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.NonRealUnitsOne), HyperMath.Create<TInner>(StandardNumber.One));
                    case StandardNumber.CombinedOne:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.Zero), HyperMath.Create<TInner>(StandardNumber.CombinedOne));
                    case StandardNumber.AllOne:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(StandardNumber.AllOne), HyperMath.Create<TInner>(StandardNumber.AllOne));
                    default:
                        return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner>(num), HyperMath.Create<TInner>(StandardNumber.Zero));
                }
            }

            public virtual HyperSplitComplex<TInner, TComponent> Create(in TInner first, in TInner second)
            {
                return new HyperSplitComplex<TInner, TComponent>(first, second);
            }

			public virtual TInner GetFirst(in HyperSplitComplex<TInner, TComponent> num)
			{
				return num.first;
			}

			public virtual ref readonly TInner GetFirstReference(in HyperSplitComplex<TInner, TComponent> num)
			{
				return ref num.first;
			}

			public virtual TInner GetSecond(in HyperSplitComplex<TInner, TComponent> num)
			{
				return num.second;
			}

			public virtual ref readonly TInner GetSecondReference(in HyperSplitComplex<TInner, TComponent> num)
			{
				return ref num.second;
			}
			
			public virtual HyperSplitComplex<TInner, TComponent> WithFirst(in HyperSplitComplex<TInner, TComponent> num, in TInner first)
            {
                return num.WithFirst(first);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> WithSecond(in HyperSplitComplex<TInner, TComponent> num, in TInner second)
            {
                return num.WithSecond(second);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> FirstCall(StandardUnaryOperation operation, in HyperSplitComplex<TInner, TComponent> num)
            {
                return num.FirstCall(operation);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> SecondCall(StandardUnaryOperation operation, in HyperSplitComplex<TInner, TComponent> num)
            {
                return num.SecondCall(operation);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner, TComponent> num1, in TInner num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TInner num1, in HyperSplitComplex<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner, TComponent> num1, in TInner num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TInner num1, in HyperSplitComplex<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }			
			
			public virtual HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.FirstCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> FirstCall(StandardBinaryOperation operation, in TComponent num1, in HyperSplitComplex<TInner, TComponent> num2)
            {
                return num2.FirstCallReversed(operation, num1);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in HyperSplitComplex<TInner, TComponent> num1, in TComponent num2)
            {
                return num1.SecondCall(operation, num2);
            }
			
			public virtual HyperSplitComplex<TInner, TComponent> SecondCall(StandardBinaryOperation operation, in TComponent num1, in HyperSplitComplex<TInner, TComponent> num2)
            {
                return num2.SecondCallReversed(operation, num1);
            }
			
            public virtual HyperSplitComplex<TInner, TComponent> Create(in TComponent num)
            {
                return new HyperSplitComplex<TInner, TComponent>(HyperMath.Operations.For<TInner, TComponent>.Instance.Create(num));
            }

            public virtual HyperSplitComplex<TInner, TComponent> Create(in TComponent realUnit, in TComponent otherUnits, in TComponent someUnitsCombined, in TComponent allUnitsCombined)
            {
                return new HyperSplitComplex<TInner, TComponent>(HyperMath.Create<TInner, TComponent>(realUnit, otherUnits, someUnitsCombined, someUnitsCombined), HyperMath.Create<TInner, TComponent>(otherUnits, someUnitsCombined, someUnitsCombined, allUnitsCombined));
            }

            public virtual HyperSplitComplex<TInner, TComponent> Create(IEnumerable<TComponent> units)
            {
                var ienum = units.GetEnumerator();
                ienum.MoveNext();
                return Create(ienum);
            }

            public virtual HyperSplitComplex<TInner, TComponent> Create(IEnumerator<TComponent> units)
            {
                var first = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                var second = HyperMath.Operations.For<TInner, TComponent>.Instance.Create(units);
                return new HyperSplitComplex<TInner, TComponent>(first, second);
            }
		}
	}

}