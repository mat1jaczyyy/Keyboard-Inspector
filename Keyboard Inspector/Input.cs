using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Keyboard_Inspector {
    abstract class Input: IEquatable<Input>, IXML {
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

        protected abstract StringBuilder ToXMLDerived(StringBuilder sb = null);

        public StringBuilder ToXML(StringBuilder sb = null) {
            if (sb == null) sb = new StringBuilder();

            sb.Append("<i>");

            ToXMLDerived(sb);

            sb.Append("</i>");

            return sb;
        }

        public static Input FromXML(XmlNode node) {
            node.Ensure("i");
            node.FirstChild.Ensure("ki", "wi");

            switch (node.FirstChild.Name) {
                case "ki":
                    return KeyInput.FromXMLDerived(node.FirstChild);

                case "wi":
                    return WiitarInput.FromXMLDerived(node.FirstChild);
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

        protected abstract string XMLName { get; }

        protected override StringBuilder ToXMLDerived(StringBuilder sb = null) {
            if (sb == null) sb = new StringBuilder();

            sb.Append($"<{XMLName}>{ToString()}</{XMLName}>");

            return sb;
        }
    }
}
