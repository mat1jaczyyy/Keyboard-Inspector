using System;
using System.Drawing;

namespace Keyboard_Inspector {
    abstract class Input: IEquatable<Input> {
        public abstract string Source { get; }

        protected abstract bool EqualsDerived(Input other);

        public bool Equals(Input other)
            => !(other is null) && EqualsDerived(other);

        public override bool Equals(object obj)
            => Equals(obj as Input);

        public static bool operator ==(Input left, Input right)
            => left.Equals(right);

        public static bool operator !=(Input left, Input right)
            => !(left == right);

        protected abstract int GetHashCodeDerived();

        public override int GetHashCode()
            => Tuple.Create(Source, GetHashCodeDerived()).GetHashCode();

        public abstract override string ToString();

        public string ToString(bool includeSource) => includeSource? $"[{Source}] {ToString()}" : ToString();

        public abstract Color DefaultColor { get; }
    }

    abstract class Input<T>: Input, IEquatable<Input<T>> {
        public readonly T Key;

        protected Input(T k)
            => Key = k;

        public bool Equals(Input<T> other)
            => !(other is null) && Key.Equals(other.Key);

        protected override bool EqualsDerived(Input other)
            => Equals(other as Input<T>);

        protected override int GetHashCodeDerived()
            => Key.GetHashCode();

        public override string ToString()
            => Key.ToString();
    }
}
