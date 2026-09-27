using System;
using System.Collections.Generic;
using System.Text;

namespace FanModelDB
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; } = "default";
        public bool IsActive { get; set; } 
        public int Order {  get; set; } = 0;
        public DateTime createAt { set; get; }
        public DateTime updateAt { set; get; }
    }
}
