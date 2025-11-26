using Infrastructure.ExternalApi;
using Models.Characters;

namespace Services;

public class GameFactory
{
    private readonly GenshinClient _client = new();

    public async Task<GameCharacter> CreateCharacterAsync(string apiName)
    {
        var details = await _client.GetCharacterAsync(apiName);
        return ApiMapper.ToCharacter(details);
    }

    public async Task EquipRandomWeapon(GameCharacter c)
    {
        var weapons = await _client.GetWeaponsAsync();
        var w = ApiMapper.ToWeapon(weapons.First());
        c.EquipWeapon(w);
    }
}