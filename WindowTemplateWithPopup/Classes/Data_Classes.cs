﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowTemplateWithPopup.Classes
{
    class Data_Classes
    {
        public class JSON_Setting
        {
            public Enums.WindowState maximilize_window { get; set; } = Enums.WindowState.None;
            public List<double> size_window { get; set; } = new List<double>() { 450.0, 800.0  };
            public List<double> position_window { get; set; } = new List<double>() { 0.0, 0.0 };
            public string window_language { get; set; } = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }

        public class JSON_Temp
        {
            public string path { get; set; }
        }
    }
}
