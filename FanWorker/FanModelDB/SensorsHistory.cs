using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FanModelDB
{
    public class SensorsHistory
    {
        public int Id { get; set; }

        [ForeignKey("SensorId")]
        public virtual Sensor Sensor { get; set; }
        public int SensorId { set; get; }

        public DateTime Date { set; get; } = DateTime.Now;

        public double Value { set; get; }
    }
}
