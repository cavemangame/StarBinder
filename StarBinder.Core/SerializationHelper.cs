using Newtonsoft.Json;

namespace StarBinder.Core
{
    public static class SerializationHelper
    {
        public static string ToJson(this Galaxy galaxy)
        {
            return JsonConvert.SerializeObject(new GalaxyData(galaxy));
        }

        public static Galaxy GalaxyFromJson(string json)
        {
            var data = JsonConvert.DeserializeObject<GalaxyData>(json);
            return Galaxy.Create(data);
        }
    }
}
