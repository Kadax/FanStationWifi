using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FanModelDB
{
    public class Period
    {
        public int Id { set; get; }
        public string Name { set; get; } = "default";

        [ForeignKey("ProfileId")]
        public virtual Profile Profile { set; get; }
        public int ProfileId { set; get; }          


        public int MaxSpeed { set; get; }
        public TimeSpan Start { set; get; }
        public TimeSpan End { set; get; }

        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }
    }
}
