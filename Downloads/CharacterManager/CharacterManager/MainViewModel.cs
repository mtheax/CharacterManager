namespace CharacterManager
{
    using CharacterManager.Models;
    using CharacterManager.Services;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CharacterService _characterService;

        public ObservableCollection<Character> Characters { get; set; }

        private Character? _selectedCharacter;
        public Character? SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                if (_selectedCharacter != value)
                {
                    _selectedCharacter = value;
                    OnPropertyChanged();
                    ((DelegateCommand)OpenImageCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand OpenImageCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            _characterService = new CharacterService();

            Characters = new ObservableCollection<Character>(_characterService.GetAllCharacters().ToList());

            foreach (var character in Characters)
            {
                Task.Run(() => _characterService.DownloadImageAsync(character));
            }

            OpenImageCommand = new DelegateCommand(OpenImage, CanOpenImage);
            AddCommand = new DelegateCommand(AddCharacter);
            DeleteCommand = new DelegateCommand(DeleteCharacter, CanDeleteCharacter);
        }

        private bool CanOpenImage(object? _) => !string.IsNullOrEmpty(SelectedCharacter?.LocalImagePath);

        private void OpenImage(object? _)
        {
            try
            {
                var path = SelectedCharacter?.LocalImagePath ?? SelectedCharacter?.ImageUrl;
                if (string.IsNullOrEmpty(path)) return;

                var psi = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to open image: {ex}");
            }
        }

        private void AddCharacter(object? _)
        {
            var character = new Character
            {
                Name = "Новий персонаж",
                Class = "Новачок",
                Level = 1,
                Description = "",
                ImageUrl = "Не вказано"
            };

            _characterService.AddCharacter(character);
            Characters.Add(character);
            SelectedCharacter = character;
        }

        private bool CanDeleteCharacter(object? _) => SelectedCharacter != null;

        private void DeleteCharacter(object? _)
        {
            if (SelectedCharacter == null) return;

            var id = SelectedCharacter.Id;
            _characterService.DeleteCharacter(id);
            Characters.Remove(SelectedCharacter);
            SelectedCharacter = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    internal class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}