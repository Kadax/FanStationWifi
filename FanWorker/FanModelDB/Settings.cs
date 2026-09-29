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

        public double? minDelta { set; get; } = 0;      //отключаем приток
        public double? maxDelta { set; get; } = 3;      //приток на максимум
        public double? maxDeltaExternal { set; get;  } = 5;  //Максимальная температура на улице GivenTemperature-maxDeltaExternal
        public double? minExternal {  set; get; } = 5;      //Минимальная температура для верхнего притока
        public double? topDelteRoom { set; get; } = 3;   //разница для вытяжки
        public double? minRoom { set; get; } = -1;       //минимальная температура в комнате для переключения на вытяжку


        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }

    }
}
