using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    class FileFormat {
        string Name;
        string[] Extensions;
        Func<string, Result> Reader;
        Action<Result, string> Writer;
        string Disclaimer;
        bool DisclaimerShown;

        IEnumerable<string> Asterisk => Extensions.Select(i => $"*{i}");
        public string Filter => $"{Name} ({string.Join(", ", Asterisk)})|{string.Join(";", Asterisk)}";

        public bool Match(string path)
            => Extensions.Any(path.EndsWith);

        public Result Read(string path) {
            var result = Reader(path);

            if (result != null && Disclaimer != null && !DisclaimerShown) {
                new DarkMessageBox(Disclaimer, "Disclaimer", DarkMessageBoxIcon.Information) {
                    StartPosition = FormStartPosition.CenterParent,
                    MaximumWidth = 600
                }.ShowDialog();

                DisclaimerShown = true;
            }

            return result;
        }

        public void Write(Result result, string path)
            => Writer(result, path);

        public FileFormat(string name, string[] extensions, Func<string, Result> reader, Action<Result, string> writer = null, string disclaimer = null) {
            Name = name;
            Extensions = extensions.Select(i => i.StartsWith(".") ? i : $".{i}").ToArray();
            Reader = reader;
            Writer = writer;
            Disclaimer = disclaimer;
        }

        static readonly string AllFiles = "All Files (*.*)|*.*";

        static string GetEachFilter(IEnumerable<FileFormat> formats)
            => string.Join("|", formats.Select(i => i.Filter));

        public static string GetOpenFilter(IEnumerable<FileFormat> formats) {
            var all = formats.SelectMany(i => i.Asterisk);
            return $"All Supported Files ({string.Join(", ", all)})|{string.Join(";", all)}|{GetEachFilter(formats)}|{AllFiles}";
        }

        public static string GetSaveFilter(IEnumerable<FileFormat> formats)
            => $"{GetEachFilter(formats.Where(i => i.Writer != null))}|{AllFiles}";
    }
}
