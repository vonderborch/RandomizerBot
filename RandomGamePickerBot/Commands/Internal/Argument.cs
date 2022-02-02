namespace RandomGamePickerBot.Commands.Internal
{
    public readonly struct Argument
    {
        public Argument(string name, string description, bool isRequired = true)
        {
            Name = name;
            Description = description;
            IsRequired = isRequired;
        }

        public string Name { get; }

        public string Description { get; }

        public bool IsRequired { get; }

        public string Parameter => $"{(IsRequired ? "<" : "[")}{Name}{(IsRequired ? ">" : "]")}";

        public string HelpMessage => $"-- {Name} => {(IsRequired == false ? "(Optional)" : "(Required)")} {Description} ";
    }
}
