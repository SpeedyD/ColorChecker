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
        double Global_Red = 0;
        double Global_Green = 0;
        double Global_Blue = 0;
        double Global_Transparency = 255;

        double Global_Hue = 0;
        double Global_Saturation = 0;
        double Global_Value = 0;

        string Global_Hex = "#FFFFFFFF";

        bool RGB_Update = false;
        bool HSV_Update = false;

        public MainWindow()
        {
            InitializeComponent();

            UpdateRGBText();
            UpdateHSVText();
            UpdateRGBSliders();
            UpdateHSVSliders();
        }

        private void UpdateColors_RGBtoHSV()
        {
            if (!RGB_Update && !HSV_Update)
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

                Color NewColor = new Color
                {
                    R = (Byte)Global_Red,
                    B = (Byte)Global_Blue,
                    G = (Byte)Global_Green,
                    A = (Byte)Global_Transparency
                };

                HSV_Update = true;
                ColorPicker.SelectedColor = NewColor;
                HSV_Update = false;
            }
        }

        private void UpdateColors_HSVtoRGB()
        {
            if (!RGB_Update && !HSV_Update)
            {

                double CalcC = Global_Value * Global_Saturation;
                double CalcX = CalcC * (1 - (Global_Hue / 60) % 2 - 1);
                double CalcM = Global_Value - CalcC;

                double CalcRed = 0;
                double CalcGreen = 0;
                double CalcBlue = 0;

                if (Global_Hue >= 0 || Global_Hue < 60)
                {
                    CalcRed = CalcC;
                    CalcGreen = CalcX;
                    CalcBlue = 0;
                }
                else if (Global_Hue >= 60 || Global_Hue < 120)
                {
                    CalcRed = CalcX;
                    CalcGreen = CalcC;
                    CalcBlue = 0;
                }
                else if (Global_Hue >= 120 || Global_Hue < 180)
                {
                    CalcRed = 0;
                    CalcGreen = CalcC;
                    CalcBlue = CalcX;
                }
                else if (Global_Hue >= 180 || Global_Hue < 240)
                {
                    CalcRed = 0;
                    CalcGreen = CalcX;
                    CalcBlue = CalcC;
                }
                else if (Global_Hue >= 240 || Global_Hue < 300)
                {
                    CalcRed = CalcX;
                    CalcGreen = 0;
                    CalcBlue = CalcC;
                }
                else if (Global_Hue >= 300 || Global_Hue < 360)
                {
                    CalcRed = CalcC;
                    CalcGreen = 0;
                    CalcBlue = CalcX;
                }

                Global_Red = (CalcRed + CalcM) * 255;
                Global_Green = (CalcGreen + CalcM) * 255;
                Global_Blue = (CalcBlue + CalcM) * 255;

                Color NewColor = new Color
                {
                    R = (Byte)Global_Red,
                    B = (Byte)Global_Blue,
                    G = (Byte)Global_Green,
                    A = (Byte)Global_Transparency
                };

                RGB_Update = true;
                ColorPicker.SelectedColor = NewColor;
                RGB_Update = false;
            }
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

                if (RGB_Update == false)
                {
                    UpdateRGBSliders();
                    UpdateRGBText();
                }
                // if (HSV_Update == false)
                {
                    UpdateHSVSliders();
                    UpdateHSVText();
                }
                UpdateWarningLabel();
            }

        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Red.Text = Input_Red.Text.Remove(Input_Red.Text.Length - 1);
            }
            else
            {
                Global_Red = Int32.Parse(Input_Red.Text);
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Red_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Red = (int)Slider_Red.Value;
            UpdateColors_RGBtoHSV();
        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Green.Text = Input_Green.Text.Remove(Input_Green.Text.Length - 1);
            }
            else
            {
                Global_Green = Int32.Parse(Input_Green.Text);
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Green_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Green = (int)Slider_Green.Value;
            UpdateColors_RGBtoHSV();
        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Blue.Text = Input_Blue.Text.Remove(Input_Blue.Text.Length - 1);
            }
            else
            {
                Global_Blue = Int32.Parse(Input_Blue.Text);
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Blue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Blue = (int)Slider_Blue.Value;
            UpdateColors_RGBtoHSV();
        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Transparency.Text = Input_Transparency.Text.Remove(Input_Transparency.Text.Length - 1);
            }
            else
            {
                Global_Transparency = Int32.Parse(Input_Transparency.Text);
                UpdateColors_RGBtoHSV();
            }
        }

        private void Slider_Transparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Transparency = (int)Slider_Transparency.Value;
            UpdateColors_RGBtoHSV();
        }

        private void Input_Hue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

        }

        /*
        private void Input_Hue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Input_Hue.Text, "[^0-9]"))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Hue.Text = Input_Hue.Text.Remove(Input_Hue.Text.Length - 1);
            }
            else
            {
                Global_Hue = double.Parse(Input_Hue.Text);
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Hue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Hue = (int)Slider_Hue.Value;
            UpdateColors_HSVtoRGB();
        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Saturation.Text = Input_Saturation.Text.Remove(Input_Saturation.Text.Length - 1);
            }
            else
            {
                Global_Saturation = Int32.Parse(Input_Saturation.Text);
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Saturation = (int)Slider_Saturation.Value;
            UpdateColors_HSVtoRGB();
        }

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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter only numbers.");
                Input_Value.Text = Input_Value.Text.Remove(Input_Value.Text.Length - 1);
            }
            else
            {
                Global_Value = Int32.Parse(Input_Value.Text);
                UpdateColors_HSVtoRGB();
            }
        }

        private void Slider_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global_Value = (int)Slider_Value.Value;
            UpdateColors_HSVtoRGB();
        }
        */

        private void UpdateRGBSliders()
        {
            Slider_Red.Value = Global_Red;
            Slider_Green.Value = Global_Green;
            Slider_Blue.Value = Global_Blue;

            Slider_Transparency.Value = Global_Transparency;
        }

        private void UpdateHSVSliders()
        {
            Slider_Hue.Value = Global_Hue;
            Slider_Saturation.Value = Global_Saturation;
            Slider_Value.Value = Global_Value;
        }

        private void UpdateRGBText()
        {
            Input_Red.Text = Global_Red.ToString();
            Input_Green.Text = Global_Green.ToString();
            Input_Blue.Text = Global_Blue.ToString();
            Input_Transparency.Text = Global_Transparency.ToString();
        }

        private void UpdateHSVText()
        {
            Input_Hue.Text = Global_Hue.ToString();
            Input_Saturation.Text = Global_Saturation.ToString();
            Input_Value.Text = Global_Value.ToString();
        }

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

        private void Slider_Hue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // FIX ME //
        }

        private void Slider_Saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // FIX ME //
        }

        private void Slider_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // FIX ME //
        }

        private void Input_Hue_TextChanged(object sender, TextChangedEventArgs e)
        {
            // FIX ME //
        }

        private void Input_Saturation_TextChanged(object sender, TextChangedEventArgs e)
        {
            // FIX ME //
        }

        private void Input_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            // FIX ME //
        }
    }
}
