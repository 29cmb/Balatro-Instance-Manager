namespace Balatro_Instance_Manager;

public class Types
{
    public class Profile
    {
        public bool Enabled { get; set; }
    }

    public class ModData
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }
}