namespace CharacterManager.UI;

using System;
using System.IO;
using CharacterManager.Models;
using CharacterManager.Services;

public class CommandLineInterface
{
    private CharacterService characterService;

    public CommandLineInterface()
    {
        characterService = new CharacterService();
    }

    public void Start()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();
        
        while (true)
        {
            DisplayMainMenu();
            string choice = Console.ReadLine() ?? "";

            switch (choice.ToLower())
            {
                case "1":
                case "список":
                    DisplayAllCharacters();
                    break;
                case "2":
                case "інформація":
                case "інфо":
                    DisplayCharacterInfo();
                    break;
                case "3":
                case "створити":
                case "новий":
                    CreateNewCharacter();
                    break;
                case "4":
                case "видалити":
                    DeleteCharacter();
                    break;
                case "5":
                case "кеш":
                case "cache":
                    DisplayCacheInfo();
                    break;
                case "6":
                case "вихід":
                case "exit":
                    Exit();
                    return;
                default:
                    Console.WriteLine("\n❌ Невідома команда. Спробуйте ще раз.");
                    PauseMenu();
                    break;
            }
        }
    }

    private void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== КЕРУВАННЯ ПЕРСОНАЖАМИ ===\n");
        Console.WriteLine("1. Показати список персонажів");
        Console.WriteLine("2. Інформація про персонажа");
        Console.WriteLine("3. Створити нового персонажа");
        Console.WriteLine("4. Видалити персонажа");
        Console.WriteLine("5. Інформація про кеш графік");
        Console.WriteLine("6. Вихід");
        Console.Write("\nВиберіть опцію (1-6): ");
    }

    private void DisplayAllCharacters()
    {
        Console.Clear();
        var characters = characterService.GetAllCharacters();

        if (characters.Count == 0)
        {
            Console.WriteLine("Персонажів не знайдено. Створіть нового персонажа!\n");
            PauseMenu();
            return;
        }

        Console.WriteLine("=== СПИСОК ПЕРСОНАЖІВ ===\n");

        foreach (var character in characters)
        {
            Console.WriteLine(character.ToString());
        }

        Console.WriteLine();
        PauseMenu();
    }

    private void DisplayCharacterInfo()
    {
        var characters = characterService.GetAllCharacters();

        if (characters.Count == 0)
        {
            Console.WriteLine("Немає персонажів для відображення.\n");
            PauseMenu();
            return;
        }

        Console.Clear();
        Console.WriteLine("=== ІНФОРМАЦІЯ ПРО ПЕРСОНАЖА ===\n");

        DisplayAllCharacters();
        
        Console.Write("Введіть ID персонажа: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var character = characterService.GetCharacterById(id);
            if (character != null)
            {
                character.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Персонаж з цим ID не знайдений.\n");
            }
        }
        else
        {
            Console.WriteLine("Невалідний ID.\n");
        }

        PauseMenu();
    }

    private void CreateNewCharacter()
    {
        Console.Clear();
        Console.WriteLine("=== СТВОРЕННЯ НОВОГО ПЕРСОНАЖА ===\n");

        var character = new Character();

        Console.Write("Ім'я персонажа: ");
        character.Name = Console.ReadLine() ?? "Безіменний";

        Console.Write("Клас персонажа: ");
        character.Class = Console.ReadLine() ?? "Новачок";

        Console.Write("Рівень: ");
        if (!int.TryParse(Console.ReadLine(), out int level))
        {
            level = 1;
        }
        character.Level = Math.Max(1, level);

        Console.Write("Опис: ");
        character.Description = Console.ReadLine() ?? "Немає опису";

        Console.Write("Посилання на графіку (URL): ");
        character.ImageUrl = Console.ReadLine() ?? "Не вказано";

        characterService.AddCharacter(character);

        Console.WriteLine("\nПерсонаж успішно створений!");
        Console.WriteLine($"Ім'я: {character.Name}");
        Console.WriteLine($"ID: {character.Id}\n");
        
        PauseMenu();
    }

    private void DeleteCharacter()
    {
        var characters = characterService.GetAllCharacters();

        if (characters.Count == 0)
        {
            Console.WriteLine("Немає персонажів для видалення.\n");
            PauseMenu();
            return;
        }

        Console.Clear();
        Console.WriteLine("=== ВИДАЛЕННЯ ПЕРСОНАЖА ===\n");

        DisplayAllCharacters();

        Console.Write("Введіть ID персонажа для видалення: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var character = characterService.GetCharacterById(id);
            if (character != null)
            {
                characterService.DeleteCharacter(id);
                Console.WriteLine($"\nПерсонаж '{character.Name}' видалений.\n");
            }
            else
            {
                Console.WriteLine("\nПерсонаж з цим ID не знайдений.\n");
            }
        }
        else
        {
            Console.WriteLine("\nНевалідний ID.\n");
        }

        PauseMenu();
    }

    private void DisplayCacheInfo()
    {
        Console.Clear();
        var imageService = characterService.GetImageService();
        var cachedImages = imageService.GetCachedImages();
        long cacheSize = imageService.GetCacheSize();

        Console.WriteLine("=== ІНФОРМАЦІЯ ПРО КЕШ ===\n");

        Console.WriteLine($"Закешовано графік: {cachedImages.Count}");
        Console.WriteLine($"Розмір кеша: {FormatFileSize(cacheSize)}\n");
        
        if (cachedImages.Count > 0)
        {
            Console.WriteLine("Список закешованих файлів:");
            foreach (var image in cachedImages)
            {
                var fileInfo = new FileInfo(image);
                Console.WriteLine($"  {fileInfo.Name} ({FormatFileSize(fileInfo.Length)})");
            }
        }

        Console.WriteLine();
        
        while (true)
        {
            Console.WriteLine("Опції:");
            Console.WriteLine("1. Очистити кеш");
            Console.WriteLine("2. Назад\n");
            Console.Write("Виберіть: ");

            string choice = Console.ReadLine() ?? "";
            
            if (choice == "1" || choice.ToLower() == "очистити")
            {
                imageService.ClearCache();
                Console.WriteLine("\nКеш очищено!\n");
                PauseMenu();
                break;
            }
            else if (choice == "2" || choice.ToLower() == "назад")
            {
                break;
            }
            else
            {
                Console.WriteLine("Невідома команда.\n");
            }
        }
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }

    private void Exit()
    {
        Console.Clear();
        Console.WriteLine("До побачення!\n");
    }

    private void PauseMenu()
    {
        Console.Write("Натисніть Enter для продовження...");
        Console.ReadLine();
    }
}
