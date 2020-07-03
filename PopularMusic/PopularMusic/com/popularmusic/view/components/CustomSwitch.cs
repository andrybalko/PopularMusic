using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PopularMusic.com.popularmusic.view.components
{
    public class CustomSwitch : Switch
    {
        public static readonly BindableProperty SwitchOffColorProperty =
          BindableProperty.Create(nameof(SwitchOffColor),
              typeof(Color), typeof(CustomSwitch),
              Color.Default);

        public Color SwitchOffColor
        {
            get { return (Color)GetValue(SwitchOffColorProperty); }
            set { SetValue(SwitchOffColorProperty, value); }
        }

        public static readonly BindableProperty SwitchOnColorProperty =
          BindableProperty.Create(nameof(SwitchOnColor),
              typeof(Color), typeof(CustomSwitch),
              Color.Default);

        public Color SwitchOnColor
        {
            get { return (Color)GetValue(SwitchOnColorProperty); }
            set { SetValue(SwitchOnColorProperty, value); }
        }
    }
}
