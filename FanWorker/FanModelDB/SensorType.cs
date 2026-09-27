using System;
using System.Collections.Generic;
using System.Text;

namespace FanModelDB
{
    public class SensorType
    {
        public int Id { set; get; }
        public string Name { set; get; } = "default";

        public string? Description { set; get; }

        public string? Source { set; get; }

        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }

    }
}
