using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Keyboard_Inspector {
    static class HTTP {
        static readonly HttpClient client = new HttpClient(
            new WebRequestHandler() { AllowAutoRedirect = false }
        );

        static HTTP() {
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    Regex.Replace(Constants.Name, @"\s+", ""),
                    Constants.Version
                )
            );
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    $"(+{Constants.GitHubURL})"
                )
            );
        }

        public static Task<HttpResponseMessage> Fetch(string url)
            => client.GetAsync(url);

        public static Task<HttpResponseMessage> Fetch(Uri url, CancellationToken ct)
            => client.GetAsync(url, ct);
    }
}
