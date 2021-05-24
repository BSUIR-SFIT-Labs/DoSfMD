using System.Collections.Generic;

namespace XamarinApp.Models
{
    public class Computer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ProcessorModel { get; set; }
        public int RamSize { get; set; }
        public int SsdSize { get; set; }
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}