using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string fileName)
    {
        using (var s = await client.GetStreamAsync(uri))
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                await s.CopyToAsync(fs);
            }
        }
    }
}