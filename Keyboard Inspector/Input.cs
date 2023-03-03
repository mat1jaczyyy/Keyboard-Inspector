using System;
using System.Drawing;
using System.IO;

namespace Keyboard_Inspector {
    abstract class Input: IEquatable<Input>, IBinary {
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

        public abstract void ToBinary(BinaryWriter bw);

        public static Input FromBinary(BinaryReader br) {
            switch (br.ReadChar()) {
                case 'k':
                    return KeyInput.FromBinaryDerived(br);

                case 'w':
                    return WiitarInput.FromBinaryDerived(br);
            }

            throw new InvalidDataException();
        }
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

        protected abstract char BinaryID { get; }

        public override void ToBinary(BinaryWriter bw) {
            bw.Write(BinaryID);
            bw.Write(Convert.ToInt32(Key));
        }
    }
}
