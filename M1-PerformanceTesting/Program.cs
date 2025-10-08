using System.Collections.Concurrent;
using System.Diagnostics;

// -- PERFORMANCE TESTING --
//Console.WriteLine("Calcul de performance.");

//var sw = Stopwatch.StartNew();
//// on éxécute 50 millions de calculs
//double sum = 1;
//for (int i = 0; i < 50_000_000; i++)
//{
//    //cosinus
//    sum += Math.Sin(i) + Math.Cos(i);
//    //Racine carrée
//    sum += Math.Sqrt(i);
//    // Exp + Log
//    sum += Math.Exp(i % 10) + Math.Log(i);
//    //Puissances
//    sum += Math.Pow(i % 100, 3);
//    //Multiplication rule
//    sum *= 1.0000001;
//}

//sw.Stop();
//Console.WriteLine($"Temps de calcul sans parallèle : {sw.ElapsedMilliseconds} ms");


//var sw2 = Stopwatch.StartNew();
//// on éxécute 50 millions de calculs
//double sum2 = 1;
//Parallel.For(0, 50_000_000, i =>
//{
//    //cosinus
//    sum2 += Math.Sin(i) + Math.Cos(i);
//    //Racine carrée
//    sum2 += Math.Sqrt(i);
//    // Exp + Log
//    sum2 += Math.Exp(i % 10) + Math.Log(i);
//    //Puissances
//    sum2 += Math.Pow(i % 100, 3);
//    //Multiplication rule
//    sum2 *= 1.0000001;
//});

//sw2.Stop();
//Console.WriteLine($"Temps de calcul avec parallèle : {sw2.ElapsedMilliseconds} ms");


// --- Download test ---

//static async Task DownloadSingleImageAsync(HttpClient http, string url, string path, int index, int total)
//{
//    var bytes = await http.GetByteArrayAsync(url);
//    await File.WriteAllBytesAsync(path, bytes); // Écriture asynchrone
//    Console.WriteLine($"[Async] Image {index}/{total} téléchargée");
//}

//static void DownloadSingleImage(HttpClient http, string url, string path, int index, int total)
//{
//    var bytes = http.GetByteArrayAsync(url).Result;
//    File.WriteAllBytes(path, bytes); // Écriture synchrone
//    Console.WriteLine($"[Sync] Image {index}/{total} téléchargée");
//}
//var sw = Stopwatch.StartNew();

//string outputDir = "imagesDownloaded";
//Directory.CreateDirectory(outputDir);

//int count = 10; // nombre d'images à télécharger
//string url = "https://picsum.photos/1920/1080";
//sw.Restart();
//DownloadImagesSequential(url, outputDir, count);
//sw.Stop();
//Console.WriteLine($"⏱️ Séquentiel : {sw.ElapsedMilliseconds} ms\n");

//sw.Restart();
//await DownloadImagesAsync(url, outputDir, count);
//sw.Stop();
//Console.WriteLine($"⚡ Asynchrone : {sw.ElapsedMilliseconds} ms\n");

//// 🧱 VERSION 1 — Séquentielle (bloquante)
//static void DownloadImagesSequential(string url, string outputDir, int count)
//{
//    using var http = new HttpClient();

//    Parallel.For(0, count, i =>
//    {
//        var bytes = http.GetByteArrayAsync(url).Result; // ⚠️ blocant
//        string path = Path.Combine(outputDir, $"seq_{i + 1}.jpg");
//        File.WriteAllBytes(path, bytes);
//        Console.WriteLine($"[Séquentiel] Image {i + 1}/{count} téléchargée");
//    });
//}

//// ⚡ VERSION 2 — Asynchrone (non bloquante)
//static async Task DownloadImagesAsync(string url, string outputDir, int count)
//{
//    using var http = new HttpClient();
//    var tasks = new List<Task>();

//    for (int i = 0; i < count; i++)
//    {
//        string path = Path.Combine(outputDir, $"async_{i + 1}.jpg");
//        tasks.Add(DownloadSingleImageAsync(http, url, path, i + 1, count));
//    }

//    await Task.WhenAll(tasks); // ✅ tous les téléchargements en parallèle
//}

// -- Synchronisation
var numbers = new ConcurrentBag<int>();

Parallel.For(0, 10000, i =>
{
    numbers.Add(i);
});

Console.WriteLine($"Total ajouté : {numbers.Count}");