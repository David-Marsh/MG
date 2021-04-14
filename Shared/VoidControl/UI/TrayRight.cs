using MG.Shared.UI;
using MG.Shared.UI.PrefabPanels;
using Microsoft.Xna.Framework;
using System;

namespace MG.VoidControl.UI
{
    public class TrayRight : Panel
    {
        private readonly ApplicationPanel applicationPanel;
        private readonly ShipStatusPanel shipStatusPanel;
        private readonly DiagnosticsPanel diagnosticsPanel;
        private readonly MiniMapPanel miniMapPanel;
        private readonly DebugingPanel debugingPanel;

        public TrayRight(Game game, GraphicsDeviceManager graphics, Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Collums = 8;
            Rows = 36;
            applicationPanel = new(game, graphics, Colors.Back, Colors.Fore, 0, 0, 8, 1);
            miniMapPanel = new(graphics, Colors.Back, Colors.Fore, 0, 1, 8, 9);
            shipStatusPanel = new(Colors.Back, Colors.Fore, 0, 10, 8, 4);
            debugingPanel = new(Colors.Back, Colors.Fore, 0, 14, 8, 7);
            diagnosticsPanel = new(Colors.Back, Color.Lime, 4, 34, 4, 2);
            Controls.Add(applicationPanel);
            Controls.Add(miniMapPanel);
            Controls.Add(shipStatusPanel);
            Controls.Add(debugingPanel);
            Controls.Add(diagnosticsPanel);
            Setup(graphics);
            applicationPanel.btnMenu.Clicked += new EventHandler(delegate (Object o, EventArgs a)
            {
                miniMapPanel.Visible = applicationPanel.MenuVisable;
                shipStatusPanel.Visible = applicationPanel.MenuVisable;
                debugingPanel.Visible = applicationPanel.MenuVisable;
                diagnosticsPanel.Visible = applicationPanel.MenuVisable;
            });
        }

        public void Setup(GraphicsDeviceManager graphics)
        {
            Setup(graphics, 64, 36);
            applicationPanel.Setup(this);
            miniMapPanel.Setup(this);
            shipStatusPanel.Setup(this);
            debugingPanel.Setup(this);
            diagnosticsPanel.Setup(this);
        }
    }
}
