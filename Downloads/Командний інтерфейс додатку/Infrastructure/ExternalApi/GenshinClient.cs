using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ExternalApi;

public class GenshinClient
{
    private readonly HttpClient _http = new();

    private const string BaseUrl = "https://genshin.jmp.blue/";

    public async Task<List<ApiCharacter>> GetCharactersAsync()
    {
        var json = await _http.GetStringAsync($"{BaseUrl}/characters");
        return JsonSerializer.Deserialize<List<ApiCharacter>>(json)!;
    }

    public async Task<ApiCharacterDetails> GetCharacterAsync(string name)
    {
        var json = await _http.GetStringAsync($"{BaseUrl}/characters/{name}");
        return JsonSerializer.Deserialize<ApiCharacterDetails>(json)!;
    }

    public async Task<List<ApiWeapon>> GetWeaponsAsync()
    {
        var json = await _http.GetStringAsync($"{BaseUrl}/weapons");
        return JsonSerializer.Deserialize<List<ApiWeapon>>(json)!;
    }
}