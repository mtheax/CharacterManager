namespace Infrastructure.ExternalApi;

public class ApiCharacter
{
    public string Name { get; set; }
}

public class ApiCharacterDetails
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Vision { get; set; }
    public string Weapon { get; set; }
}

public class ApiWeapon
{
    public string Name { get; set; }
    public string Type { get; set; }
}