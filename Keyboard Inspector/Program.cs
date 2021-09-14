using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Keyboard_Inspector {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static StringBuilder ToXML<T>(this ReadOnlyCollection<T> c, string name, StringBuilder sb = null) where T: IXML {
            if (sb == null) sb = new StringBuilder();

            sb.Append($"<{name}>");

            foreach (var i in c)
                i.ToXML(sb);

            sb.Append($"</{name}>");

            return sb;
        }

        public static XmlNode GetNode(this XmlNode node, string name) {
            for (int i = 0; i < node.ChildNodes.Count; i++)
                if (node.ChildNodes[i].NodeType == XmlNodeType.Element && node.ChildNodes[i].Name == name)
                    return node.ChildNodes[i];
            
            throw new InvalidDataException();
        }

        public static void Ensure(this XmlNode node, params string[] names) {
            if (node.NodeType != XmlNodeType.Element || !names.Contains(node.Name))
                throw new InvalidDataException();
        }

        public static IEnumerable<XmlNode> GetNodes(this XmlNode node, string name) {
            for (int i = 0; i < node.ChildNodes.Count; i++)
                if (node.ChildNodes[i].NodeType == XmlNodeType.Element && node.ChildNodes[i].Name == name)
                    yield return node.ChildNodes[i];
        }

        public static T ToEnum<T>(this string str) where T: struct {
            if (Enum.TryParse(str, out T e))
                return e;

            throw new InvalidDataException();
        }
    }
}
