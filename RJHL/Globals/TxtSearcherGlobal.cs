using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RJHL.Globals
{

    public class TxtSearcherGlobal
    {
        public async Task<Dictionary<string, string>> SearchValuesAsync(
            string filePath,
            HashSet<string> valuesToFind,
            char columnDelimiter = '|',
            int threadCount = 2,
            CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo no encontrado", filePath);

            var results = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var fileLength = new FileInfo(filePath).Length;
            var blockPerThread = fileLength / threadCount;

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var tasks = new List<Task>();

                for (int i = 0; i < threadCount; i++)
                {
                    long start = i * blockPerThread;
                    long end = (i == threadCount - 1) ? fileLength : (i + 1) * blockPerThread;

                    tasks.Add(Task.Run(() =>
                    {
                        ProcessUsingReadLineFast(
                            filePath, start, end,
                            columnDelimiter,
                            valuesToFind, results, cts, cts.Token);
                    }, cts.Token));
                }

                try
                {
                    while (tasks.Count > 0)
                    {
                        var completedTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                        tasks.Remove(completedTask);

                        if (cts.IsCancellationRequested)
                            break;
                    }

                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                catch (OperationCanceledException) { }

                return results.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        private void ProcessUsingReadLineFast(
            string filePath,
            long start,
            long end,
            char columnDelimiter,
            HashSet<string> valuesToFind,
            ConcurrentDictionary<string, string> results,
            CancellationTokenSource cts,
            CancellationToken token)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(start, SeekOrigin.Begin);

                using (var sr = new StreamReader(fs, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024 * 1024))
                {
                    if (start != 0)
                        sr.ReadLine(); // descartar línea incompleta al inicio

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (fs.Position > end || token.IsCancellationRequested)
                            break;

                        var idx = line.IndexOf(columnDelimiter);
                        if (idx <= 0) continue;

                        var columnValue = line.Substring(0, idx);

                        if (valuesToFind.Contains(columnValue))
                            if (results.TryAdd(columnValue, line))
                            {
                                if (results.Count == valuesToFind.Count)
                                {
                                    cts.Cancel(); // Se encontraron todos
                                    break;
                                }
                            }
                    }
                }
            }
        }
    }

}
