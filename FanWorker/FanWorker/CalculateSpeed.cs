using FanModelDB;
using FanWorker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FanWorker
{
    internal class CalculateSpeed
    {
        private double _p = 20;
        private double _i = 0.1;

        public CalculateSpeed(double p= 20, double i=0.2)
        {
            _i = i;
            _p = p;
        }

        public FanStatusResponse Calculate(Settings settings, List<SensorsHistory> TSensors, double extTemp) {

            var fs = new FanStatusResponse();

            var list = TSensors.GroupBy(i => i.Date).ToList();

            var speed = calculateSpeed(list, settings.GivenTemperature);

            if(speed > settings.MaxSpeed)
            {
                speed = settings.MaxSpeed;
            }

            fs.Speed = speed;

            var last = list.Last();

            if (extTemp < settings.minExternal)
            {
                fs.ServoPosition = 1;
            }
            else
            {
                fs.ServoPosition = 0;
            }



            fs.Direction = 0;

            if (last.Min(i=>i.Value) < settings.GivenTemperature+settings.minRoom)  //Холодно снизу
            {
                fs.Direction = 1;                               //вытежка
                fs.ServoPosition = 0;                           //верх

            } 

            if(last.Sum(i=>i.Value)/ last.Count() <= settings.GivenTemperature)   //Остудили
            {
                fs.Direction = 1;                               //вытежка
                fs.ServoPosition = 0;                           //верх
                fs.Speed = 2;                                   //минимальная скорость вытяжки
            }

            if (settings.GivenTemperature-settings.maxDeltaExternal <= extTemp)
            {
                fs.Direction = 1;                               //вытежка
                fs.ServoPosition = 0;                           //верх
            }




            return fs;
        }


        private int calculateSpeed(List<IGrouping<DateTime, SensorsHistory>> list, double temp)
        {                                          
            var last =  list.Last();

            var propS = (last.Sum(i => i.Value) / last.Count()) - temp;

            var propS_max = last.Max(i => i.Value) - temp;

            var intS = 0.0;

            var k = 0.1;

            if (list.Count() > 2) {



                for (var i = 1; i < list.Count(); i++)
                {
                    var t = (list[i].Sum(i => i.Value) / list[i].Count());

                    var delta = t - temp;

                    var koef = (delta * ((list[i].Key - list[i-1].Key).TotalMinutes));

                    intS += koef;


                }
                                                
            }


            var p = _p*propS + (_p/2*propS_max) + _i*intS;

            if (p > 100)
            {
                p = 100;
            }

            if (p < 0) {
                p = 0;
            }

            var s = Remap(p, 0, 100, 1, 15);


            return (int)Math.Round(s, MidpointRounding.AwayFromZero);

        }

        private double Remap(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }


    }
}
