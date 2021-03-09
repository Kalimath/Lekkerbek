﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lekkerbek.Web.Models
{
    public class Tijdslot
    {
        public int Id { get; set; }

        private DateTime tijdstip;

        public DateTime Tijdstip
        {
            get { return getTijdslot(); }
            set { tijdstip = value; }
        }

        public bool IsVrij { get; set; } = true;

//String word als volgt gedeclareerd om te werken "7:00"
        public Tijdslot(String tijdslot)
        {
            tijdstip = DateTime.Parse(tijdslot); 
        }
        public Tijdslot()
        {
            
        }
        public DateTime getTijdslot()
        {
            return tijdstip; 
        }

    }
}
