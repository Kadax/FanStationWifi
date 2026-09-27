using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FanModelDB
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; } = "default";
        public bool IsActive { get; set; } = true;
        public int? Order { get; set; } = 0;

        [ForeignKey("TypeId")]
        public virtual SensorType Type { get; set; }
        public int TypeId { set; get; }

        public string? Source { get; set; }
        public string? SensorExtID { set; get; }

        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }
    }
}
