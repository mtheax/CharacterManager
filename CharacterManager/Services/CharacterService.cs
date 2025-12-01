namespace CharacterManager.Services;

using CharacterManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public class CharacterService
{
    private List<Character> characters;
    private ImageService imageService;
    private const string DataFile = "characters.json";
    private int nextId = 1;

    public CharacterService()
    {
        characters = new List<Character>();
        imageService = new ImageService();
        LoadCharacters();
    }

    public void AddCharacter(Character character)
    {
        character.Id = nextId++;
        characters.Add(character);
        SaveCharacters();
        
        Task.Run(() => DownloadImageAsync(character));
    }

    public List<Character> GetAllCharacters()
    {
        return characters;
    }

    public Character? GetCharacterById(int id)
    {
        return characters.FirstOrDefault(c => c.Id == id);
    }

    public void DeleteCharacter(int id)
    {
        var character = GetCharacterById(id);
        if (character != null)
        {
            // Видалити локальну копію графіки
            if (!string.IsNullOrEmpty(character.LocalImagePath))
            {
                imageService.DeleteImage(character.LocalImagePath);
            }
            
            characters.Remove(character);
            SaveCharacters();
        }
    }

    public ImageService GetImageService()
    {
        return imageService;
    }

    public async Task DownloadImageAsync(Character character)
    {
        try
        {
            if (string.IsNullOrEmpty(character.ImageUrl) || character.ImageUrl == "Не вказано")
            {
                return;
            }

            character.IsImageDownloading = true;
            var (success, localPath) = await imageService.DownloadImageAsync(character.ImageUrl, character.Id);

            if (success)
            {
                character.LocalImagePath = localPath;
                character.ImageHash = imageService.GetImageHash(localPath);
                SaveCharacters();
            }

            character.IsImageDownloading = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні графіки для {character.Name}: {ex.Message}");
            character.IsImageDownloading = false;
        }
    }

    public void SaveCharacters()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(characters, options);
            File.WriteAllText(DataFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні даних: {ex.Message}");
        }
    }

    public void LoadCharacters()
    {
        try
        {
            if (File.Exists(DataFile))
            {
                string json = File.ReadAllText(DataFile);
                characters = JsonSerializer.Deserialize<List<Character>>(json) ?? new List<Character>();
                
                if (characters.Count > 0)
                {
                    nextId = characters.Max(c => c.Id) + 1;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні даних: {ex.Message}");
            characters = new List<Character>();
        }
    }
}
