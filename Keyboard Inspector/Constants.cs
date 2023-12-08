using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Constants {
        public static readonly string Name, Version, GitHubURL, DiscordURL, KoFiURL, DataDir, WisdomFile;
        public static readonly string VersionSuffix = null;

        static Constants() {
            Name = Application.ProductName;

            var ver = Assembly.GetEntryAssembly().GetName().Version;
            Version = ver.ToString(ver.Build > 0? 3 : 2);

            if (!string.IsNullOrWhiteSpace(VersionSuffix))
                Version += $"-{VersionSuffix}";

            #if DEBUG
                Version += "-debug";
            #endif

            // TODO get relevant links from an API in the future
            GitHubURL = "https://github.com/mat1jaczyyy/Keyboard-Inspector";
            DiscordURL = "https://discord.gg/kX4cJQH5Zn"; 
            KoFiURL = "https://ko-fi.com/mat1jaczyyy";

            DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Name);
            WisdomFile = Path.Combine(DataDir, "wisdom");
        }
    }
}
