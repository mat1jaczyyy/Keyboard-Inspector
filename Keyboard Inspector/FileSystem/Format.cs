using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    class FileFormat {
        public string Name { get; private set; }
        string[] Extensions;
        public FileOptions FileOptions { get; private set; }
            
        Func<Stream, Result> Reader;
        Action<Result, string> Writer;
        public bool CanWrite => Writer != null;

        Func<string, Uri> CustomURLFilter;

        string Disclaimer;
        bool DisclaimerShown;

        public IEnumerable<string> Asterisk => Extensions.Select(i => $"*{i}");
        public string Filter => $"{Name} ({string.Join(", ", Asterisk)})|{string.Join(";", Asterisk)}";

        public bool Match(string path)
            => Extensions.Any(path.EndsWith);

        public Result Read(Stream stream) {
            var result = Reader(stream);

            if (result != null && Disclaimer != null && !DisclaimerShown) {
                var dialog = new DarkMessageBox(Disclaimer, "Disclaimer", DarkMessageBoxIcon.Information) {
                    StartPosition = FormStartPosition.CenterParent,
                    MaximumWidth = 600
                };

                MainForm.Instance.InvokeIfRequired(dialog.ShowDialog);
                DisclaimerShown = true;
            }

            return result;
        }

        public void Write(Result result, string path)
            => Writer(result, path);

        Uri DefaultURLFilter(string url) {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri result)) return null;
            if (!FileSystem.AllowedSchemes.Contains(result.Scheme)) return null;
            if (!Extensions.Any(Path.GetExtension(result.AbsolutePath).EndsWith)) return null;
            return result;
        }

        public Uri FilterURL(string url)
            => DefaultURLFilter(url)?? CustomURLFilter?.Invoke(url);

        public FileFormat(string name, string[] extensions, Func<Stream, Result> reader, Action<Result, string> writer = null, Func<string, Uri> urlFilter = null, string disclaimer = null, FileOptions fileOptions = FileOptions.None) {
            Name = name;
            Extensions = extensions.Select(i => i.StartsWith(".")? i : $".{i}").ToArray();
            Reader = reader;
            Writer = writer;
            CustomURLFilter = urlFilter;
            Disclaimer = disclaimer;
            FileOptions = fileOptions;
        }
    }
}
