using DataSources;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var allAlbums = ListAlbumsData.ListAlbums;

// --- Exercice 1: Display albums with LINQ with specific format
//var albumsWithLinq = from album in allAlbums
//                     select $"Album n°{album.AlbumId} : {album.Title}";
//Console.WriteLine("Albums with LINQ:");
//foreach (var album in albumsWithLinq)
//{
//    Console.WriteLine(album);
//}

// --- Exercice 2 & 3 & 4: Search by string & order by / then by & grouped search
//Console.WriteLine("Enter a search string:");
//var searchString = Console.ReadLine();

//var searchResults = allAlbums
//    .Where(album => album.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
//    .OrderBy(album => album.Title)
//    .ThenByDescending(album => album.AlbumId)
//    .GroupBy(album => album.ArtistId);

//foreach (var artiste in searchResults)
//{
//    Console.WriteLine($"Artist n°{artiste.Key} :");
//    foreach (var album in artiste)
//        Console.WriteLine($"    Album n°{album.AlbumId} : {album.Title}");
//    Console.WriteLine();
//}

// --- Exercice 5: Display result of grouped search by artist with name of the artist
//Console.WriteLine("Enter a search string:");
//var searchString = Console.ReadLine();
//var allArtists = ListArtistsData.ListArtists;
//var searchResults = from album in allAlbums
//                    where album.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)
//                    orderby album.Title, album.AlbumId descending
//                    group album by album.ArtistId into artistGroup
//                    join artist in allArtists on artistGroup.Key equals artist.ArtistId
//                    select new
//                    {
//                        ArtistName = artist.Name,
//                        Albums = artistGroup
//                    };
//foreach (var artist in searchResults)
//{
//    Console.WriteLine($"Artist: {artist.ArtistName} ({artist.Albums.Count()})");
//    foreach (var album in artist.Albums)
//        Console.WriteLine($"    Album n°{album.AlbumId} : {album.Title}");
//    Console.WriteLine();
//}

// --- Exercice 6: Pagination with take & skip
//var albumsPerPage = 20;
//var albumsToDisplay = from album in allAlbums
//                        let affichage = $"    Album n°{album.AlbumId} : {album.Title}"
//                      orderby album.AlbumId
//                      select affichage;
//var totalPages = (int)Math.Ceiling((double)albumsToDisplay.Count() / albumsPerPage);
//var currentPage = 1;
//while (currentPage <= totalPages)
//{
//    var pagedAlbums = albumsToDisplay
//        .Skip((currentPage - 1) * albumsPerPage)
//        .Take(albumsPerPage);

//    Console.WriteLine($"Displaying page {currentPage}:");
//    foreach (var album in pagedAlbums)
//        Console.WriteLine(album);

//    Console.WriteLine(
//        currentPage < totalPages
//        ? "Press Enter to see the next page..."
//        : "End of results. Press Enter to exit."
//    );
//    Console.ReadLine();
//    currentPage++;
//}

// --- Exercice 7: Search into a text file
Console.WriteLine("Enter your search string:");
var searchString = Console.ReadLine();
var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Text", "Albums.txt");
if (!File.Exists(filePath))
{
    Console.WriteLine("File not found.");
    return;
}
var lines = File.ReadAllLines(filePath);
var searchResults =
    from line in lines
    let affichage = $"    {line}"
    where line.Contains(searchString, StringComparison.OrdinalIgnoreCase)
    orderby line
    select affichage;
foreach (var result in searchResults)
    Console.WriteLine(result);