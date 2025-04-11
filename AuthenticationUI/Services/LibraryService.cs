using AuthenticationUI.Models;
using System.Buffers.Text;
using System.Net.Http.Json;

public class LibraryService
{
    private readonly HttpClient _httpClient;
    private readonly string Base64UrL = "https://localhost:7213";
    public LibraryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        Console.WriteLine($"LibraryService BaseAddress: {_httpClient.BaseAddress}");
    }

    public async Task<List<Book>> GetBooks()
    {
        return await _httpClient.GetFromJsonAsync<List<Book>>($"{Base64UrL}/api/book");
    }

    public async Task<Book> GetBook(int id)
    {
        return await _httpClient.GetFromJsonAsync<Book>($"{Base64UrL}/api/book/{id}");
    }

    public async Task<List<Book>> SearchBook(string title)
    {
        var res = await _httpClient.GetAsync($"{Base64UrL}/api/book/search?title={title}");
        if (res.IsSuccessStatusCode)
        {
            return await res.Content.ReadFromJsonAsync<List<Book>>() ?? new List<Book>();
        }
        return new List<Book>();
    }

    public async Task<bool> DeleteBook(int id)
    {
        var res = await _httpClient.DeleteAsync($"{Base64UrL}/api/book/{id}");
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBook(int id, Book updatedBook)
    {
        var res = await _httpClient.PutAsJsonAsync($"{Base64UrL}/api/book/{id}", updatedBook);
        return res.IsSuccessStatusCode;
    }

    public async Task<Book?> CreateBook(Book newBook)
    {
        var res = await _httpClient.PostAsJsonAsync($"{Base64UrL}/api/book", newBook);
        if (res.IsSuccessStatusCode)
        {
            return await res.Content.ReadFromJsonAsync<Book>();
        }
        return null;
    }
}
