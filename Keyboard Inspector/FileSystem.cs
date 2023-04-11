using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using DarkUI.Forms;

namespace Keyboard_Inspector {
    class FileSystem {
        public class Format {
            string Name;
            string[] Extensions;
            
            Func<string, Result> Reader;
            Action<Result, string> Writer;
            public bool CanWrite => Writer != null;

            string Disclaimer;
            bool DisclaimerShown;

            public IEnumerable<string> Asterisk => Extensions.Select(i => $"*{i}");
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

            public Format(string name, string[] extensions, Func<string, Result> reader, Action<Result, string> writer = null, string disclaimer = null) {
                Name = name;
                Extensions = extensions.Select(i => i.StartsWith(".") ? i : $".{i}").ToArray();
                Reader = reader;
                Writer = writer;
                Disclaimer = disclaimer;
            }
        }

        static string GetSaveFilter(IEnumerable<Format> formats)
            => $"{GetEachFilter(formats.Where(i => i.CanWrite))}|{AllFiles}";

        static Format[] formats = new Format[] {
            new Format("Keyboard Inspector Files", new string[] { "kbi" },
                Result.FromPath,
                (result, path) => {
                    using (MemoryStream ms = new MemoryStream()) {
                        using (BinaryWriter bw = new BinaryWriter(ms)) {
                            result.ToBinary(bw);
                            File.WriteAllBytes(path, ms.ToArray());
                        }
                    }
                }
            ),
            new Format("TETR.IO Replay Files", new string[] { "ttr", "ttrm" },
                TetrioReplay.ConvertToResult,
                disclaimer:
                "You are analyzing a TETR.IO replay file.\n\nTETR.IO downsamples input data to 600 Hz to fit it onto the subframe grid which " +
                "you will notice as a peak at 600 Hz in the frequency domain. This adds another \"sampling rate\" (in addition to the usual " +
                "USB poll rate and matrix scan rate) to the process.\n\nThis is unlike a regular Keyboard Inspector recording which tries to " +
                "get the most accurate real-time timestamp it can without any additional downsampling.\n\nYou may still analyze the recording, " +
                "but be vary of the limitations of the TETR.IO replay format."
            ),
        };

        static readonly string AllFiles = "All Files (*.*)|*.*";

        static string GetEachFilter(IEnumerable<Format> formats)
            => string.Join("|", formats.Select(i => i.Filter));

        static string GetOpenFilter(IEnumerable<Format> formats) {
            var all = formats.SelectMany(i => i.Asterisk);
            return $"All Supported Files ({string.Join(", ", all)})|{string.Join(";", all)}|{GetEachFilter(formats)}|{AllFiles}";
        }

        static OpenFileDialog ofd = new OpenFileDialog() {
            Filter = GetOpenFilter(formats),
            Title = "Open Recording"
        };
        static SaveFileDialog sfd = new SaveFileDialog() {
            Filter = GetSaveFilter(formats),
            Title = "Save Recording"
        };

        static Format FindFormat(string filename)
            => formats.FirstOrDefault(i => i.Match(filename));

        public static bool SupportsFormat(string filename)
            => FindFormat(filename) != null;

        public static bool OpenDialog(out string filename) {
            filename = null;

            if (ofd.ShowDialog() == DialogResult.OK) {
                filename = ofd.FileName;
                return true;
            }

            return false;
        }

        public static bool Open(string filename, out Result result, out string error) {
            result = null;
            error = null;

            try {
                result = FindFormat(filename).Read(filename);
                return true;

            } catch {
                error = "Couldn't parse file, it is likely corrupt or in an unsupported format.";
            }

            return false;
        }

        public static void Save(Result result, out string error) {
            error = null;

            if (sfd.ShowDialog() == DialogResult.OK) {
                try {
                    FindFormat(sfd.FileName).Write(result, sfd.FileName);

                } catch {
                    error = "Couldn't save file, try saving it to a different location or with a different file name.";
                }
            }
        }
    }
}
