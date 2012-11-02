using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BadAppleScr2
{
    public class GrayscaleEffect : ShaderEffect
    {
        private static readonly PixelShader _pixelShader =
            new PixelShader() { UriSource = new Uri("/Effect/GrayscaleEffect.ps", UriKind.Relative) };

        public GrayscaleEffect()
        {
            PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(NegativeProperty);
            UpdateShaderValue(LeaveBlackProperty);
            UpdateShaderValue(ChrominanceProperty);
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty ChrominanceProperty =
            DependencyProperty.Register("Chroma", typeof(double), typeof(GrayscaleEffect),
                new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));
        public double Chrominance
        {
            get { return (double)GetValue(ChrominanceProperty); }
            set { SetValue(ChrominanceProperty, value); }
        }

        public static readonly DependencyProperty LeaveBlackProperty =
            DependencyProperty.Register("Black", typeof(double), typeof(GrayscaleEffect),
                new UIPropertyMetadata(1.0, PixelShaderConstantCallback(2)));
        public bool LeaveBlack
        {
            get { return (double)GetValue(LeaveBlackProperty) > 0.5; }
            set
            {
                if (value)
                    SetValue(LeaveBlackProperty, 1.0);
                else
                    SetValue(LeaveBlackProperty, 0.0);
            }
        }

        public static readonly DependencyProperty NegativeProperty =
            DependencyProperty.Register("Negative", typeof(double), typeof(GrayscaleEffect),
                new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        public double Negative
        {
            get { return (double)GetValue(NegativeProperty); }
            set { SetValue(NegativeProperty, value); }
        }
    }
}
