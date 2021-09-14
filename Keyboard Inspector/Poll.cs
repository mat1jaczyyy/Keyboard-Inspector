using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Keyboard_Inspector {
    class Poll: IXML {
        public readonly double Time;

        public Poll(double time) {
            Time = time;
        }

        public static implicit operator double(Poll p) => p.Time;

        public StringBuilder ToXML(StringBuilder sb = null) {
            if (sb == null) sb = new StringBuilder();

            sb.Append($"<p>{Time}</p>");

            return sb;
        }

        public static List<Poll> ListFromXML(XmlNode node) {
            node.Ensure("ps");

            return node.GetNodes("p").Select(i => new Poll(double.Parse(i.InnerText))).ToList();
        }
    }
}
