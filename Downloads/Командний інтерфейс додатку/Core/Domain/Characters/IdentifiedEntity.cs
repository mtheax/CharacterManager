namespace RefactoredCommandSystem.Core.Domain.Characters
{
    public abstract class IdentifiedEntity
    {
        protected IdentifiedEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; private set; }

        public void Rename(string name)
        {
            Name = name;
        }
    }
}

