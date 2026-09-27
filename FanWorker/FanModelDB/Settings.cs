using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FanModelDB
{
    public class Settings
    {
        public int Id { get; set; }

        [ForeignKey("ProfileId")]
        public virtual Profile Profile { set; get; }
        public int? ProfileId { set; get; }

        public string Name { get; set; } = "default";
        public double GivenTemperature { get; set; }
        public int MaxSpeed { set; get; }
        public int PeriodUpdete { set; get; }

        public bool isAuto { set; get; } = false;

        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }

    }
}
