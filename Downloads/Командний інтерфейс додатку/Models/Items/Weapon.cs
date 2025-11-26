namespace Models.Items;

public class Weapon
{
    public Weapon(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public string Type { get; }
}
