using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double Global_Red = 64;
        double Global_Green = 128;
        double Global_Blue = 128;
        double Global_Transparency = 255;

        double Global_Hue = 180;
        double Global_Saturation = 50;
        double Global_Value = 50;

        string Global_Hex = "#FFFFFFFF";

        Color Global_Color;

        bool UpdateBlock_ColorPicker = false;
        bool UpdateBlock_TextSliders_RGB = false;
        bool UpdateBlock_TextSliders_HSV = false;

        public MainWindow()
        {
            InitializeComponent();

            Global_Color = new Color
            {
                R = (Byte)Global_Red,
                B = (Byte)Global_Blue,
                G = (Byte)Global_Green,
                A = (Byte)Global_Transparency
            };

            UpdateBlock_TextSliders_RGB = true;
            UpdateBlock_TextSliders_HSV = true;
            UpdateColorPicker();
            UpdateBlock_TextSliders_RGB = false;
            UpdateBlock_TextSliders_HSV = false;
            UpdateBlock_ColorPicker = true;
            UpdateText();
            UpdateBlock_ColorPicker = false;
        }

        private void UpdateColorPicker()
        {
            if (!UpdateBlock_ColorPicker)
            {
                ColorPicker.SelectedColor = Global_Color;
            }
            UpdateWarningLabel();
        }

        private void UpdateText()
        {
            if (!UpdateBlock_TextSliders_RGB)
            {
                Input_Red.Text = Global_Red.ToString();
                Input_Green.Text = Global_Green.ToString();
                Input_Blue.Text = Global_Blue.ToString();

                Input_Transparency.Text = Global_Transparency.ToString();
            }

            if (!UpdateBlock_TextSliders_HSV)
            {
                Input_Hue.Text = Global_Hue.ToString();
                Input_Saturation.Text = Global_Saturation.ToString();
                Input_Value.Text = Global_Value.ToString();
            }
            UpdateWarningLabel();
        }

        private void UpdateColors_RGBtoHSV()
        {
            double RCalc = (Global_Red / 255);
            double GCalc = (Global_Green / 255);
            double BCalc = (Global_Blue / 255);

            double CalcMax = Math.Max(RCalc, Math.Max(GCalc, BCalc));
            double CalcMin = Math.Min(RCalc, Math.Min(GCalc, BCalc));
            double CalcDiff = CalcMax - CalcMin;

            double CalcHue = 0;
            double CalcSat = 0;

            if (CalcMax == CalcMin) Global_Hue = 0;

            else if (CalcMax == RCalc)
                CalcHue = (60 * ((GCalc - BCalc) / CalcDiff) + 360) % 360;

            else if (CalcMax == GCalc)
                CalcHue = (60 * ((BCalc - RCalc) / CalcDiff) + 120) % 360;

            else if (CalcMax == BCalc)
                CalcHue = (60 * ((RCalc - GCalc) / CalcDiff) + 240) % 360;

            if (CalcMax == 0)
                CalcSat = 0;
            else
                CalcSat = (CalcDiff / CalcMax) * 100;

            double CalcVal = CalcMax * 100;

            Global_Hue = (int)CalcHue;
            Global_Saturation = (int)CalcSat;
            Global_Value = (int)CalcVal;

            Int32 HRed = (int)Global_Red;
            Int32 HGreen = (int)Global_Green;
            Int32 HBlue = (int)Global_Blue;

            Global_Hex = "#FF" + HRed.ToString("X") + HGreen.ToString("X") + HBlue.ToString("X");

            Global_Color = new Color
            {
                R = (Byte)Global_Red,
                B = (Byte)Global_Blue,
                G = (Byte)Global_Green,
                A = (Byte)Global_Transparency
            };

            UpdateBlock_TextSliders_RGB = true;
            UpdateColorPicker();
            UpdateBlock_TextSliders_RGB = false;
        }

        private void UpdateColors_HSVtoRGB()
        {
            double CalcSat = Global_Saturation / 100;
            double CalcVal = Global_Value / 100;
            double CalcHue = Global_Hue / 60;

            double CalcC = CalcVal * CalcSat;
            double CalcX = CalcC * (1 - Math.Abs(CalcHue % 2 - 1));
            double CalcM = CalcVal - CalcC;

            double CalcRed = 0;
            double CalcGreen = 0;
            double CalcBlue = 0;

            if (Global_Hue >= 0 && Global_Hue < 60)
            {
                CalcRed = CalcC;
                CalcGreen = CalcX;
                CalcBlue = 0;
            }
            else if (Global_Hue >= 60 && Global_Hue < 120)
            {
                CalcRed = CalcX;
                CalcGreen = CalcC;
                CalcBlue = 0;
            }
            else if (Global_Hue >= 120 && Global_Hue < 180)
            {
                CalcRed = 0;
                CalcGreen = CalcC;
                CalcBlue = CalcX;
            }
            else if (Global_Hue >= 180 && Global_Hue < 240)
            {
                CalcRed = 0;
                CalcGreen = CalcX;
                CalcBlue = CalcC;
            }
            else if (Global_Hue >= 240 && Global_Hue < 300)
            {
                CalcRed = CalcX;
                CalcGreen = 0;
                CalcBlue = CalcC;
            }
            else if (Global_Hue >= 300 && Global_Hue < 360)
            {
                CalcRed = CalcC;
                CalcGreen = 0;
                CalcBlue = CalcX;
            }

            Global_Red = (CalcRed + CalcM) * 255;
            Global_Green = (CalcGreen + CalcM) * 255;
            Global_Blue = (CalcBlue + CalcM) * 255;

            Global_Color = new Color
            {
                R = (Byte)Global_Red,
                B = (Byte)Global_Blue,
                G = (Byte)Global_Green,
                A = (Byte)Global_Transparency
            };

            UpdateBlock_TextSliders_HSV = true;
            UpdateColorPicker();
            UpdateBlock_TextSliders_HSV = false;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ColorPicker.SelectedColor.HasValue)
            {
                Color C = ColorPicker.SelectedColor.Value;
                Global_Red = C.R;
                Global_Green = C.G;
                Global_Blue = C.B;
                Global_Transparency = C.A;

                SolidColorBrush CB = new SolidColorBrush(C);

                ColorPicker.Background = CB;

                UpdateBlock_ColorPicker = true;
                UpdateText();
                UpdateBlock_ColorPicker = false;
            }
        }

        #region RedSwitches
        private void Input_Red_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Red_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Red.Text, "[^0-9]"))
            {
                Lbl_Red_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Red_Warn.Content = "";
                int RedValue = Int32.Parse(Input_Red.Text);
                Slider_Red.Value = RedValue;
                Global_Red = RedValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Red_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int RedValue = (int)Slider_Red.Value;
            Input_Red.Text = RedValue.ToString();
            Global_Red = RedValue;
            UpdateColors_RGBtoHSV();
        }
        #endregion

        #region GreenSwitches
        private void Input_Green_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Green_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Green.Text, "[^0-9]"))
            {
                Lbl_Green_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Green_Warn.Content = "";
                int GreenValue = Int32.Parse(Input_Green.Text);
                Slider_Green.Value = GreenValue;
                Global_Green = GreenValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Green_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int GreenValue = (int)Slider_Green.Value;
            Input_Green.Text = GreenValue.ToString();
            Global_Green = GreenValue;
            UpdateColors_RGBtoHSV();
        }
        #endregion

        #region BlueSwitches
        private void Input_Blue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Blue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Blue.Text, "[^0-9]"))
            {
                Lbl_Blue_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Blue_Warn.Content = "";
                int BlueValue = Int32.Parse(Input_Blue.Text);
                Slider_Blue.Value = BlueValue;
                Global_Blue = BlueValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Blue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int BlueValue = (int)Slider_Blue.Value;
            Input_Blue.Text = BlueValue.ToString();
            Global_Blue = BlueValue;
            UpdateColors_RGBtoHSV();
        }
        #endregion

        #region TransparencySwitches
        private void Input_Transparency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Transparency_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Transparency.Text, "[^0-9]"))
            {
                Lbl_Transparency_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Transparency_Warn.Content = "";
                int TransparencyValue = Int32.Parse(Input_Transparency.Text);
                Slider_Transparency.Value = TransparencyValue;
                Global_Transparency = TransparencyValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Transparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int TransparencyValue = (int)Slider_Transparency.Value;
            Input_Transparency.Text = TransparencyValue.ToString();
            Global_Transparency = TransparencyValue;
            UpdateColors_RGBtoHSV();
        }
        #endregion

        #region HueSwitches
        private void Input_Hue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Hue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Hue.Text, "[^0-9]"))
            {
                Lbl_Hue_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Hue_Warn.Content = "";
                int HueValue = Int32.Parse(Input_Hue.Text);
                Slider_Hue.Value = HueValue;
                Global_Hue = HueValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Hue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int HueValue = (int)Slider_Hue.Value;
            Input_Hue.Text = HueValue.ToString();
            Global_Hue = HueValue;
            UpdateColors_HSVtoRGB();
        }
        #endregion

        #region SaturationSwitches
        private void Input_Saturation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Saturation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Saturation.Text, "[^0-9]"))
            {
                Lbl_Saturation_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Saturation_Warn.Content = "";
                int SaturationValue = Int32.Parse(Input_Saturation.Text);
                Slider_Saturation.Value = SaturationValue;
                Global_Saturation = SaturationValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SaturationValue = (int)Slider_Saturation.Value;
            Input_Saturation.Text = SaturationValue.ToString();
            Global_Saturation = SaturationValue;
            UpdateColors_HSVtoRGB();
        }
        #endregion

        #region ValueSwitches
        private void Input_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        private void Input_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Value.Text, "[^0-9]"))
            {
                Lbl_Value_Warn.Content = "Please use numbers only or it will not calculate.";
            }
            else
            {
                Lbl_Value_Warn.Content = "";
                int ValueValue = Int32.Parse(Input_Value.Text);
                Slider_Value.Value = ValueValue;
                Global_Value = ValueValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int ValueValue = (int)Slider_Value.Value;
            Input_Value.Text = ValueValue.ToString();
            Global_Value = ValueValue;
            UpdateColors_HSVtoRGB();
        }
        #endregion

        private void UpdateWarningLabel()
        {
            if (Global_Saturation == 100)
            {
                if (Global_Value >= 5 && Global_Value <= 25)
                {
                    Lbl_Warn.Content = "Warning: Shade Goo";
                }
            }
            else if (Global_Saturation >= 15 && Global_Saturation <=30)
            {
                if (Global_Value >= 80 && Global_Value <= 95)
                {
                    Lbl_Warn.Content = "Warning: Pastel Goo";
                }
            }
            else if (Global_Saturation < 15 || Global_Value < 5)
            {
                Lbl_Warn.Content = "Warning: Greyscale Goo";
            }
            else
            {
                Lbl_Warn.Content = "";
            }

        }
    }
}