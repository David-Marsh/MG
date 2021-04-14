using MG.Shared.UI;
using Microsoft.Xna.Framework;
using System;

namespace MG.VoidControl.UI
{
    public class MiniMapPanel : Panel
    {
        private readonly MiniMap MiniMap;
        private readonly Button btnZoomIn;
        private readonly Button btnZoomOut;
        private readonly Label labelZoom;

        public MiniMapPanel(GraphicsDeviceManager graphics, Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 9;
            Collums = 8;
            MiniMap = new(graphics, Color.Lime, Color.Lerp(Color.Black, Color.White, 0.1f), 0, 0, 8, 8);
            MiniMap.CanHover = false;
            labelZoom = new(Color.Gray, "Zoom:50", 2, 8, 4, 1);
            labelZoom.CanHover = false;
            btnZoomOut = new(Colors.Back, Color.White, ((char)0xF8AB).ToString(), 6, 8, 1, 1) { MenuButton = true, Delay = 150 };
            btnZoomIn = new(Colors.Back, Color.White, ((char)0xF8AA).ToString(), 7, 8, 1, 1) { MenuButton = true, Delay = 150 };
            Controls.Add(MiniMap);
            Controls.Add(labelZoom);
            Controls.Add(btnZoomIn);
            Controls.Add(btnZoomOut);
            btnZoomIn.Clicked += new EventHandler(delegate (object o, EventArgs a) { labelZoom.Msg.Text = MiniMap.ZoomIn(); });
            btnZoomOut.Clicked += new EventHandler(delegate (object o, EventArgs a) { labelZoom.Msg.Text = MiniMap.ZoomOut(); });
        }
        public override void Setup(Panel panel)
        {
            base.Setup(panel);
            Padding = (int)Cellsize.X / 8;
            MiniMap.Setup(this);
            labelZoom.Setup(this);
            btnZoomIn.Setup(this);
            btnZoomOut.Setup(this);
        }
    }
}
