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
        public ColorChanging SwapClass;

        bool UpdateBlock_ColorPicker = false;
        bool UpdateBlock_TextSliders_RGB = false;
        bool UpdateBlock_TextSliders_HSV = false;

        public MainWindow()
        {
            InitializeComponent();

            SwapClass = new ColorChanging()
            {
                Global_Red = 180,
                Global_Blue = 64,
                Global_Green = 64,

                Global_Transparency = 255,

                Global_Hue = 0,
                Global_Saturation = 64,
                Global_Value = 71,

                Global_Hex = "#FFB44040",
            };

            SwapClass.Global_Color = new Color
            {
                R = (Byte)SwapClass.Global_Red,
                B = (Byte)SwapClass.Global_Blue,
                G = (Byte)SwapClass.Global_Green,
                A = (Byte)SwapClass.Global_Transparency
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

        public void ColorImport(Color ImportColor)
        {
            SwapClass.Global_Color = ImportColor;
            UpdateColorPicker();
        }

        private void UpdateColorPicker()
        {
            if (!UpdateBlock_ColorPicker)
            {
                ColorPicker.SelectedColor = SwapClass.Global_Color;
            }
        }

        private void UpdateText()
        {
            if (!UpdateBlock_TextSliders_RGB)
            {
                Input_Red.Text = SwapClass.Global_Red.ToString();
                Input_Green.Text = SwapClass.Global_Green.ToString();
                Input_Blue.Text = SwapClass.Global_Blue.ToString();

                Input_Transparency.Text = SwapClass.Global_Transparency.ToString();
            }

            if (!UpdateBlock_TextSliders_HSV)
            {
                Input_Hue.Text = SwapClass.Global_Hue.ToString();
                Input_Saturation.Text = SwapClass.Global_Saturation.ToString();
                Input_Value.Text = SwapClass.Global_Value.ToString();
            }
        }

        private void UpdateColors_RGBtoHSV()
        {
            double RCalc = (SwapClass.Global_Red / 255);
            double GCalc = (SwapClass.Global_Green / 255);
            double BCalc = (SwapClass.Global_Blue / 255);

            double CalcMax = Math.Max(RCalc, Math.Max(GCalc, BCalc));
            double CalcMin = Math.Min(RCalc, Math.Min(GCalc, BCalc));
            double CalcDiff = CalcMax - CalcMin;

            double CalcHue = 0;
            double CalcSat = 0;

            if (CalcMax == CalcMin) SwapClass.Global_Hue = 0;

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

            SwapClass.Global_Hue = (int)CalcHue;
            SwapClass.Global_Saturation = (int)CalcSat;
            SwapClass.Global_Value = (int)CalcVal;

            Int32 HRed = (int)SwapClass.Global_Red;
            Int32 HGreen = (int)SwapClass.Global_Green;
            Int32 HBlue = (int)SwapClass.Global_Blue;

            SwapClass.Global_Hex = "#FF" + HRed.ToString("X") + HGreen.ToString("X") + HBlue.ToString("X");

            SwapClass.Global_Color = new Color
            {
                R = (Byte)SwapClass.Global_Red,
                B = (Byte)SwapClass.Global_Blue,
                G = (Byte)SwapClass.Global_Green,
                A = (Byte)SwapClass.Global_Transparency
            };

            UpdateBlock_TextSliders_RGB = true;
            UpdateColorPicker();
            UpdateWarningLabel();
            UpdateBlock_TextSliders_RGB = false;
        }

        private void UpdateColors_HSVtoRGB()
        {
            double CalcSat = SwapClass.Global_Saturation / 100;
            double CalcVal = SwapClass.Global_Value / 100;
            double CalcHue = SwapClass.Global_Hue / 60;

            double CalcC = CalcVal * CalcSat;
            double CalcX = CalcC * (1 - Math.Abs(CalcHue % 2 - 1));
            double CalcM = CalcVal - CalcC;

            double CalcRed = 0;
            double CalcGreen = 0;
            double CalcBlue = 0;

            if (SwapClass.Global_Hue >= 0 && SwapClass.Global_Hue < 60)
            {
                CalcRed = CalcC;
                CalcGreen = CalcX;
                CalcBlue = 0;
            }
            else if (SwapClass.Global_Hue >= 60 && SwapClass.Global_Hue < 120)
            {
                CalcRed = CalcX;
                CalcGreen = CalcC;
                CalcBlue = 0;
            }
            else if (SwapClass.Global_Hue >= 120 && SwapClass.Global_Hue < 180)
            {
                CalcRed = 0;
                CalcGreen = CalcC;
                CalcBlue = CalcX;
            }
            else if (SwapClass.Global_Hue >= 180 && SwapClass.Global_Hue < 240)
            {
                CalcRed = 0;
                CalcGreen = CalcX;
                CalcBlue = CalcC;
            }
            else if (SwapClass.Global_Hue >= 240 && SwapClass.Global_Hue < 300)
            {
                CalcRed = CalcX;
                CalcGreen = 0;
                CalcBlue = CalcC;
            }
            else if (SwapClass.Global_Hue >= 300 && SwapClass.Global_Hue < 360)
            {
                CalcRed = CalcC;
                CalcGreen = 0;
                CalcBlue = CalcX;
            }

            SwapClass.Global_Red = (CalcRed + CalcM) * 255;
            SwapClass.Global_Green = (CalcGreen + CalcM) * 255;
            SwapClass.Global_Blue = (CalcBlue + CalcM) * 255;

            SwapClass.Global_Color = new Color
            {
                R = (Byte)SwapClass.Global_Red,
                B = (Byte)SwapClass.Global_Blue,
                G = (Byte)SwapClass.Global_Green,
                A = (Byte)SwapClass.Global_Transparency
            };

            UpdateBlock_TextSliders_HSV = true;
            UpdateColorPicker();
            UpdateWarningLabel();
            UpdateBlock_TextSliders_HSV = false;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ColorPicker.SelectedColor.HasValue)
            {
                Color C = ColorPicker.SelectedColor.Value;
                SwapClass.Global_Red = C.R;
                SwapClass.Global_Green = C.G;
                SwapClass.Global_Blue = C.B;
                SwapClass.Global_Transparency = C.A;

                SolidColorBrush CB = new SolidColorBrush(C);

                ColorPicker.Background = CB;

                UpdateBlock_ColorPicker = true;
                UpdateText();
                UpdateWarningLabel();
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
                SwapClass.Global_Red = RedValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Red_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int RedValue = (int)Slider_Red.Value;
            Input_Red.Text = RedValue.ToString();
            SwapClass.Global_Red = RedValue;
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
                SwapClass.Global_Green = GreenValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Green_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int GreenValue = (int)Slider_Green.Value;
            Input_Green.Text = GreenValue.ToString();
            SwapClass.Global_Green = GreenValue;
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
                SwapClass.Global_Blue = BlueValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Blue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int BlueValue = (int)Slider_Blue.Value;
            Input_Blue.Text = BlueValue.ToString();
            SwapClass.Global_Blue = BlueValue;
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
                SwapClass.Global_Transparency = TransparencyValue;
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Transparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int TransparencyValue = (int)Slider_Transparency.Value;
            Input_Transparency.Text = TransparencyValue.ToString();
            SwapClass.Global_Transparency = TransparencyValue;
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
                SwapClass.Global_Hue = HueValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Hue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int HueValue = (int)Slider_Hue.Value;
            Input_Hue.Text = HueValue.ToString();
            SwapClass.Global_Hue = HueValue;
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
                SwapClass.Global_Saturation = SaturationValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SaturationValue = (int)Slider_Saturation.Value;
            Input_Saturation.Text = SaturationValue.ToString();
            SwapClass.Global_Saturation = SaturationValue;
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
                SwapClass.Global_Value = ValueValue;
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int ValueValue = (int)Slider_Value.Value;
            Input_Value.Text = ValueValue.ToString();
            SwapClass.Global_Value = ValueValue;
            UpdateColors_HSVtoRGB();
        }
        #endregion

        private void UpdateWarningLabel()
        {
            bool ContentSet = false;

            if (SwapClass.Global_Saturation == 100)
            {
                if (SwapClass.Global_Value >= 5 && SwapClass.Global_Value <= 25)
                {
                    Lbl_Warn.Content = "Warning: Shade Goo";
                    ContentSet = true;
                }
            }
            if (SwapClass.Global_Saturation >= 15 && SwapClass.Global_Saturation <=30)
            {
                if (SwapClass.Global_Value >= 80 && SwapClass.Global_Value <= 95)
                {
                    Lbl_Warn.Content = "Warning: Pastel Goo";
                    ContentSet = true;
                }
            }
            if (SwapClass.Global_Saturation < 15 || SwapClass.Global_Value < 5)
            {
                Lbl_Warn.Content = "Warning: Greyscale Goo";
                ContentSet = true;
            }

            if (!ContentSet)
            {
                Lbl_Warn.Content = "";
            }
        }
    }
}