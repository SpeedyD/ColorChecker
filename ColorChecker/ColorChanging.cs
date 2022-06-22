using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorChecker
{
    public class ColorChanging
    {
        public double Global_Red { get; set; }
        public double Global_Green { get; set; }
        public double Global_Blue { get; set; }
        public double Global_Transparency { get; set; }

        public double Global_Hue { get; set; }
        public double Global_Saturation { get; set; }
        public double Global_Value { get; set; }

        public string Global_Hex { get; set; }

        public Color Global_Color { get; set; }
    }
}
