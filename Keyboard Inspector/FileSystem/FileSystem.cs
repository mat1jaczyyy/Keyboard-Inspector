using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class FileSystem {
        static readonly HttpClient HttpClient = new HttpClient();

        public static readonly string[] AllowedSchemes = new string[] {
            Uri.UriSchemeHttp, Uri.UriSchemeHttps
        };

        static FileSystem() {
            HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    Regex.Replace(Constants.Name, @"\s+", ""),
                    Constants.Version
                )
            );
            HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    $"(+{Constants.GitHubURL})"
                )
            );
        }

        public static FileFormat[] Formats { get; private set; } = new FileFormat[] {
            new FileFormat($"{Constants.Name} File", new string[] { "kbi" },
                Result.FromStream,
                (result, path) => {
                    using (MemoryStream ms = new MemoryStream()) {
                        using (BinaryWriter bw = new BinaryWriter(ms)) {
                            result.ToBinary(bw);
                            File.WriteAllBytes(path, ms.ToArray());
                        }
                    }
                },
                fileOptions: FileOptions.SequentialScan
            ),
            new FileFormat("TETR.IO Replay File", new string[] { "ttr", "ttrm" },
                TetrioReplay.StreamToResult,
                urlFilter: url => {
                    url = url.Replace("https://tetr.io/#", "").Replace("tetrio://", "");
                    if (!Regex.IsMatch(url, "(^[Rr]:.+$)")) return null;
                    url = url.Replace("R:", "").Replace("r:", "").Split('@')[0];
                    url = $"https://inoue.szy.lol/api/replay/{Uri.EscapeUriString(url)}";
                    return new Uri(url);
                },
                disclaimer:
                "You are analyzing a TETR.IO replay file.\n\nTETR.IO downsamples input data to 600 Hz to fit it onto the subframe grid which " +
                "you will notice as a peak at 600 Hz in the frequency domain. This adds another \"sampling rate\" (in addition to the usual " +
               $"USB poll rate and matrix scan rate) to the process.\n\nThis is unlike a regular {Constants.Name} recording which tries to get " +
                "the most accurate real-time timestamp it can without any additional downsampling.\n\nYou may still analyze the recording, " +
                "but be vary of the limitations of the TETR.IO replay format."
            ),
        };

        static readonly string AllFiles = "All Files (*.*)|*.*";

        static string GetEachFilter(IEnumerable<FileFormat> formats)
            => string.Join("|", formats.Select(i => i.Filter));

        static string GetOpenFilter(IEnumerable<FileFormat> formats) {
            var all = formats.SelectMany(i => i.Asterisk);
            return $"All Supported Files ({string.Join(", ", all)})|{string.Join(";", all)}|{GetEachFilter(formats)}|{AllFiles}";
        }

        static string GetSaveFilter(IEnumerable<FileFormat> formats)
            => $"{GetEachFilter(formats.Where(i => i.CanWrite))}|{AllFiles}";

        static OpenFileDialog ofd = new OpenFileDialog() {
            Filter = GetOpenFilter(Formats),
            Title = "Open Recording"
        };
        static ImportFileDialog ifd = new ImportFileDialog();
        static SaveFileDialog sfd = new SaveFileDialog() {
            Filter = GetSaveFilter(Formats),
            Title = "Save Recording"
        };

        static FileFormat FindFormat(string filename)
            => Formats.FirstOrDefault(i => i.Match(filename));

        public static FileFormat FindFormat(string url, out Uri filtered) {
            filtered = null;

            foreach (var format in Formats) {
                filtered = format.FilterURL(url);

                if (filtered != null)
                    return format;
            }

            return null;
        }

        public static bool SupportsFormat(string filename)
            => FindFormat(filename) != null;

        public static bool OpenDialog(out string filename, out FileFormat format) {
            filename = null;
            format = null;

            if (ofd.ShowDialog() == DialogResult.OK) {
                filename = ofd.FileName;
                format = FindFormat(filename);
                return true;
            }

            return false;
        }

        public static bool ImportDialog(out Uri url, out FileFormat format) {
            url = null;
            format = null;

            if (ifd.ShowDialog() == DialogResult.OK) {
                url = ifd.URL;
                format = ifd.Format;
                return true;
            }

            return false;
        }

        public class FileResult {
            public readonly Result Result = null;
            public readonly string Error = null;

            public FileResult(Result result) => Result = result;
            public FileResult(string error) => Error = error;
        }

        static async Task<FileResult> Load(Stream stream, FileFormat format) {
            try {
                return new FileResult(await Task.Run(() => format.Read(stream)));

            } catch (IBinaryException ex) {
                return new FileResult($"Unable to parse the file. {ex.Message}");
            } catch {
                return new FileResult("Unable to parse the file. It is likely corrupt, or it is in an unsupported format.");
            }
        }

        public static async Task<FileResult> Open(string filename, FileFormat format) {
            try {
                format = format?? FindFormat(filename);

                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, format.FileOptions))
                    return await Load(stream, format);

            } catch {
                return new FileResult("Unable to open the file. It could be in use by another process, or it is in an unsupported format.");
            }
        }

        public static async Task<FileResult> Import(Uri url, FileFormat format, CancellationToken ct) {
            try {
                var res = await HttpClient.GetAsync(url, ct);

                if (res.StatusCode != HttpStatusCode.OK)
                    return new FileResult($"Unable to download the file. Received status code {(int)res.StatusCode} ({res.StatusCode}).");

                using (var stream = await res.Content.ReadAsStreamAsync()) {
                    MainForm.Instance.DownloadFinished();
                    return await Load(stream, format);
                }

            } catch (TaskCanceledException) when (ct.IsCancellationRequested) {
                return new FileResult("");

            } catch {
                return new FileResult("Unable to connect to the server. Check your network connection.");
            }
        }

        public static string Save(Result result) {
            if (sfd.ShowDialog() == DialogResult.OK) {
                try {
                    FindFormat(sfd.FileName).Write(result, sfd.FileName);

                } catch {
                    return "Unable to save the file. Try saving to a different location or with a different file name.";
                }
            }
            return null;
        }
    }
}
