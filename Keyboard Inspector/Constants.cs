using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Keyboard_Inspector {
    static class Constants {
        public static readonly string Name, Version, GitHubURL, DiscordURL, DataDir, WisdomFile;

        static Constants() {
            Name = Application.ProductName;

            var ver = Assembly.GetEntryAssembly().GetName().Version;
            Version = ver.ToString(ver.Build > 0? 3 : 2);
            
            #if DEBUG
                Version += "-debug";
            #endif

            GitHubURL = "https://github.com/mat1jaczyyy/Keyboard-Inspector";
            DiscordURL = "https://discord.gg/kX4cJQH5Zn"; // TODO get relevant link from an API in the future

            DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Name);
            WisdomFile = Path.Combine(DataDir, "wisdom");
        }
    }
}
