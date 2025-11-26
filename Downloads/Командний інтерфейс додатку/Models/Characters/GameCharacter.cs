namespace Models.Characters;

public class GameCharacter
{
    public GameCharacter(string name, string vision, string description, int attack, int defense, int hp)
    {
        Name = name;
        Vision = vision;
        Description = description;
        Attack = attack;
        Defense = defense;
        HP = hp;
    }

    public string Name { get; }
    public string Vision { get; }
    public string Description { get; }
    public int Attack { get; }
    public int Defense { get; }
    public int HP { get; }

    public Models.Items.Weapon? EquippedWeapon { get; private set; }

    public void EquipWeapon(Models.Items.Weapon w) => EquippedWeapon = w;
}
