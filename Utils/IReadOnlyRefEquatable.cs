﻿using System;

namespace IS4.HyperNumerics.Utils
{
    public interface IReadOnlyRefEquatable<T> : IEquatable<T>
    {
        bool Equals(in T other);
    }
}
