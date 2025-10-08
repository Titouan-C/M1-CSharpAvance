// Image optimisation
// Des images sont disponibles dans le dossier "img"
using System.Diagnostics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

static void SaveResized(Image src, int maxSize, string outputPath)
{
    using var clone = src.Clone(ctx =>
    {
        ctx.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,          // respecte le ratio
            Size = new Size(maxSize, maxSize),
        });
    });

    var encoder = new JpegEncoder { Quality = 85 }; // qualité web
    clone.Save(outputPath, encoder);
}

// Constant for image resolutions (1080p, 720p, 480p)
var resolutions = new (int size, string label)[]
{
    (1080, "1080p"),
    (720, "720p"),
    (480, "480p")
};

// Retrieve folder path from user
Console.WriteLine("Enter the path to the folder containing images to optimize:");
string folderPath = Console.ReadLine();
if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
{
    Console.WriteLine("Invalid folder path. Exiting.");
    return;
}


// -- Sequential version --
Console.WriteLine("Starting sequential optimization...");
var sw = Stopwatch.StartNew();
var imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
    .ToList();

// Create output directory
string outputDir = Path.Combine(folderPath, "output");
Directory.CreateDirectory(outputDir);

foreach (var imageFile in imageFiles)
{
    Console.WriteLine($"Optimizing {Path.GetFileName(imageFile)}...");
    var ext = Path.GetExtension(imageFile);

    using var image = Image.Load(imageFile);
    foreach (var (size, label) in resolutions)
    {
        string outputPath = Path.Combine(outputDir, $"{Path.GetFileNameWithoutExtension(imageFile)}_{label}{ext}");
        SaveResized(image, size, outputPath);
    }
}
sw.Stop();
Console.WriteLine($"Sequential optimization time: {sw.ElapsedMilliseconds} ms");


// -- With optimisation (asynchronous and parallel version) --
Console.WriteLine("Starting asynchronous optimization...");
sw.Restart();

string outputDirAsync = Path.Combine(folderPath, "output_async");
Directory.CreateDirectory(outputDirAsync);

// Create tasks for each image
var tasks = new List<Task>();
Parallel.ForEach(imageFiles, imageFile =>
{
    tasks.Add(Task.Run(() =>
    {
        Console.WriteLine($"Optimizing {Path.GetFileName(imageFile)}...");
        var ext = Path.GetExtension(imageFile);

        using var image = Image.Load(imageFile);
        Parallel.ForEach(resolutions, resolution =>
        {
            var (size, label) = resolution;
            string outputPath = Path.Combine(outputDirAsync, $"{Path.GetFileNameWithoutExtension(imageFile)}_{label}{ext}");
            SaveResized(image, size, outputPath);
        });
    }));
});
await Task.WhenAll(tasks);
sw.Stop();
Console.WriteLine($"Asynchronous optimization time: {sw.ElapsedMilliseconds} ms");

// Stop program process
Console.WriteLine("Optimization completed. Press any key to exit.");
Console.ReadKey();