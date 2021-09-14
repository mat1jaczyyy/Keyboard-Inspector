using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Keyboard_Inspector {
    class Event: IXML {
        public readonly double Time;
        public readonly bool Pressed;
        public readonly Input Input;

        public Event(double time, bool pressed, Input input) {
            Time = time;
            Pressed = pressed;
            Input = input;
        }

        public override string ToString()
            => $"{Time:0.00000} {(Pressed? "DN" : "UP")} {Input}";

        public StringBuilder ToXML(StringBuilder sb = null) {
            if (sb == null) sb = new StringBuilder();

            sb.Append($"<e><t>{Time}</t><b>{Convert.ToInt32(Pressed)}</b>");
            
            Input.ToXML(sb);

            sb.Append("</e>");

            return sb;
        }

        public static List<Event> ListFromXML(XmlNode node) {
            node.Ensure("es");

            return node.GetNodes("e").Select(i => new Event(
                double.Parse(i.GetNode("t").InnerText),
                Convert.ToBoolean(int.Parse(i.GetNode("b").InnerText)),
                Input.FromXML(i.GetNode("i"))
            )).ToList();
        }
    }
}
