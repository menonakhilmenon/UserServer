using Newtonsoft.Json;

namespace UserService.Models
{
    [JsonObject]
    public class CharacterGameData
    {
        public Stats characterStats { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public float posZ { get; set; }
    }
}