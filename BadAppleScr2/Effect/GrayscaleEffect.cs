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

        public static readonly DependencyProperty NegativeProperty =
            DependencyProperty.Register("Negative", typeof(double), typeof(GrayscaleEffect),
                new UIPropertyMetadata(0.5, PixelShaderConstantCallback(1)));
        public double Negative
        {
            get { return (double)GetValue(NegativeProperty); }
            set { SetValue(NegativeProperty, value); }
        }
    }
}
