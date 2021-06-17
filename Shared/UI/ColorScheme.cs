using Microsoft.Xna.Framework;

namespace MG.Shared.UI
{
    public struct ColorScheme 
    {
        public ColorScheme(Color back) : this(Color.Gray, back)
        {
        }
        public ColorScheme(Color fore, Color back) : this()
        {
            Init(fore, back);
        }
        public void Init(Color fore, Color back, bool enable = true, bool hovering = false)
        {
            Fore = ForeNormal = fore;
            ForeHover = new Color(Fore.PackedValue ^ 0x40404040);     // flip bit 7 in R,G and B to change shade of ARGB color
            ForeDisabled = Color.Lerp(Fore, Color.Gray, 0.5f);        // Shift to grey
            Back = BackNormal = back;
            BackHover = new Color(Back.PackedValue ^ 0x40404040);     // flip bit 7 in R,G and B to change shade of ARGB color
            BackDisabled = Color.Lerp(Back, Color.Gray, 0.5f);        // Shift to grey
            Set(enable, hovering);
        }
        public void Set(bool enable, bool hovering)
        {
            if (!enable)
            {
                Back = BackDisabled;
                Fore = ForeDisabled;
            }
            else if (hovering)
            {
                Back = BackHover;
                Fore = ForeDisabled;
            }
            else
            {
                Back = BackNormal;
                Fore = ForeNormal;
            }
        }

        public Color Fore;
        public Color ForeNormal;
        public Color ForeHover;
        public Color ForeDisabled;
        public Color Back;
        public Color BackNormal;
        public Color BackHover;
        public Color BackDisabled;
    }
}
