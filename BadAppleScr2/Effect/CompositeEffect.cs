using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BadAppleScr2
{
    public class CompositeEffect : ShaderEffect
    {
        private static readonly PixelShader _pixelShader =
            new PixelShader() { UriSource = new Uri("/Effect/CompositeEffect.ps", UriKind.Relative) };

        public CompositeEffect()
        {
            PixelShader = _pixelShader;
            PixelShader.InvalidPixelShaderEncountered += new EventHandler(PixelShader_InvalidPixelShaderEncountered);

            UpdateShaderValue(MaskProperty);
            UpdateShaderValue(Tex1Property);
            UpdateShaderValue(Tex2Property);
        }

        void PixelShader_InvalidPixelShaderEncountered(object sender, EventArgs e)
        {
            //Console.WriteLine(e.ToString());
        }

        public static readonly DependencyProperty MaskProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Mask", typeof(CompositeEffect), 0);
        public Brush Mask
        {
            get { return (Brush)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        public static readonly DependencyProperty Tex1Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Tex1", typeof(CompositeEffect), 1);
        public Brush Tex1
        {
            get { return (Brush)GetValue(Tex1Property); }
            set { SetValue(Tex1Property, value); }
        }

        public static readonly DependencyProperty Tex2Property =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Tex2", typeof(CompositeEffect), 2);
        public Brush Tex2
        {
            get { return (Brush)GetValue(Tex2Property); }
            set { SetValue(Tex2Property, value); }
        }
    }
}
