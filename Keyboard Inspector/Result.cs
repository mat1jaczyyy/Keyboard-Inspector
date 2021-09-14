using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace Keyboard_Inspector {
    class Result: IXML {
        public double Time;

        public ReadOnlyCollection<Event> Events;
        public ReadOnlyCollection<Poll> Polls;

        public Result(double time, List<Event> events, List<Poll> polls) {
            Time = time;
            Events = events.AsReadOnly();
            Polls = polls.AsReadOnly();
        }

        public StringBuilder ToXML(StringBuilder sb = null) {
            if (sb == null) sb = new StringBuilder();

            sb.Append($"<r><t>{Time}</t>");
            
            Events.ToXML("es", sb);
            Polls.ToXML("ps", sb);

            sb.Append("</r>");

            return sb;
        }

        public static Result FromXML(XmlNode node) {
            node.Ensure("r");

            return new Result(
                double.Parse(node.GetNode("t").InnerText),
                Event.ListFromXML(node.GetNode("es")),
                Poll.ListFromXML(node.GetNode("ps"))
            );
        }
    }
}
