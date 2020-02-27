using Newtonsoft.Json;
namespace UserService.Models
{
    [JsonObject]
    public class Stats
    {
        public int attack { get; set; }
        public int agility { get; set; }
        public int source { get; set; }
        public int constitution { get; set; }
    }
}
