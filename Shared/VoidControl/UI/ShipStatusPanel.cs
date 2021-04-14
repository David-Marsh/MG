using MG.Shared.UI;
using MG.Shared.VoidControl;
using Microsoft.Xna.Framework;
using System;

namespace MG.VoidControl.UI
{
    public class ShipStatusPanel : Panel
    {
        private readonly StatusBar statusReactor;
        private readonly StatusBar statusThruster;
        private readonly StatusBar statusShield;
        private readonly StatusBar statusWeapons;
        private readonly StatusBar statusSensors;
        private readonly StatusBar statusCloak;

        public ShipStatusPanel(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 3;
            Collums = 8;

            statusReactor = new(Colors.Back, Colors.Fore, "Reactor", 0, 0, 4, 1);
            statusThruster = new(Colors.Back, Colors.Fore, "Thruster", 0, 1, 4, 1);
            statusShield = new(Colors.Back, Colors.Fore, "Shield", 0, 2, 4, 1);
            statusWeapons = new(Colors.Back, Colors.Fore, "Weapons", 4, 0, 4, 1);
            statusSensors = new(Colors.Back, Colors.Fore, "Sensors", 4, 1, 4, 1);
            statusCloak = new(Colors.Back, Colors.Fore, "Cloak", 4, 2, 4, 1);

            Controls.Add(statusReactor);
            Controls.Add(statusThruster);
            Controls.Add(statusShield);
            Controls.Add(statusWeapons);
            Controls.Add(statusSensors);
            Controls.Add(statusCloak);

            statusReactor.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Reactor.Quality = a.Value; });
            statusThruster.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Thruster.Quality = a.Value; });
            statusShield.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Shield.Quality = a.Value; });
            statusWeapons.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Weapons.Quality = a.Value; });
            statusSensors.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Sensor.Quality = a.Value; });
            statusCloak.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) { EntityManager.Player.Cloak.Quality = a.Value; });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            statusReactor.Value = EntityManager.Player.Reactor.Quality;
            statusThruster.Value = EntityManager.Player.Thruster.Quality;
            statusShield.Value = EntityManager.Player.Shield.Quality;
            statusWeapons.Value = EntityManager.Player.Weapons.Quality;
            statusSensors.Value = EntityManager.Player.Sensor.Quality;
            statusCloak.Value = EntityManager.Player.Cloak.Quality;
        }
        public override void Setup(Panel panel)
        {
            base.Setup(panel);
            Padding = (int)Cellsize.X / 8;
            statusReactor.Setup(this);
            statusThruster.Setup(this);
            statusShield.Setup(this);
            statusWeapons.Setup(this);
            statusSensors.Setup(this);
            statusCloak.Setup(this);
        }
    }
}
