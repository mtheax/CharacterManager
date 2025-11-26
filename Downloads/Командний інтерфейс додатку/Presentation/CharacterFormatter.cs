using Models.Characters;
using System.Text;

namespace Presentation;

public static class CharacterFormatter
{
    public static string Format(GameCharacter c)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"=== {c.Name} ===");
        sb.AppendLine($"Елемент: {c.Vision}");
        sb.AppendLine($"Опис: {c.Description}");
        sb.AppendLine();
        sb.AppendLine($"HP: {c.HP}");
        sb.AppendLine($"ATK: {c.Attack}");
        sb.AppendLine($"DEF: {c.Defense}");

        if (c.EquippedWeapon != null)
            sb.AppendLine($"Зброя: {c.EquippedWeapon.Name}");

        return sb.ToString();
    }
}