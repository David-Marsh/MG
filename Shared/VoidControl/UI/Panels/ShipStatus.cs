using MG.Shared.UI;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI.Panels
{
    public class ShipStatus : Panel
    {
        private readonly StatusBar statusReactor;
        private readonly StatusBar statusThruster;
        private readonly StatusBar statusShield;
        private readonly StatusBar statusWeapons;
        private readonly StatusBar statusSensors;
        private readonly StatusBar statusCloak;
        private readonly Button buttonAutoGun;
        private readonly Button buttonAutoAim;
        public ShipStatus(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 7;
            Collums = 8;

            statusReactor = new(Colors.Back, Colors.Fore, "Reactor", 0, 0, 4, 2);
            statusThruster = new(Colors.Back, Colors.Fore, "Thruster", 0, 2, 4, 2);
            statusShield = new(Colors.Back, Colors.Fore, "Shield", 0, 4, 4, 2);
            statusWeapons = new(Colors.Back, Colors.Fore, "Weapons", 4, 0, 4, 2);
            statusSensors = new(Colors.Back, Colors.Fore, "Sensors", 4, 2, 4, 2);
            statusCloak = new(Colors.Back, Colors.Fore, "Cloak", 4, 4, 4, 2);
            buttonAutoGun = new(Colors.Back, Color.White, "AutoGun", 0, 6, 2, 1);
            buttonAutoAim = new(Colors.Back, Color.White, "AutoAim", 2, 6, 2, 1);

            Controls.Add(statusReactor);
            Controls.Add(statusThruster);
            Controls.Add(statusShield);
            Controls.Add(statusWeapons);
            Controls.Add(statusSensors);
            Controls.Add(statusCloak);
            Controls.Add(buttonAutoGun);
            Controls.Add(buttonAutoAim);

            statusReactor.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Reactor.Quality = a.Value; Update(); });
            statusThruster.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Thruster.Quality = a.Value; Update(); });
            statusShield.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Shield.Quality = a.Value; Update(); });
            statusWeapons.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Weapons.Quality = a.Value; Update(); });
            statusSensors.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Sensor.Quality = a.Value; Update(); });
            statusCloak.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a) 
            { EntityManager.Player.Cloak.Quality = a.Value; Update(); });
            buttonAutoGun.Clicked += new EventHandler(delegate (object o, EventArgs a)
            { EntityManager.Player.AutoGun = !EntityManager.Player.AutoGun; Update(); });
            buttonAutoAim.Clicked += new EventHandler(delegate (object o, EventArgs a)
            { EntityManager.Player.AutoAim = !EntityManager.Player.AutoAim; Update(); });
        }
        private void Update()
        {
            statusReactor.Value = EntityManager.Player.Reactor.Quality;
            statusThruster.Value = EntityManager.Player.Thruster.Quality;
            statusShield.Value = EntityManager.Player.Shield.Quality;
            statusWeapons.Value = EntityManager.Player.Weapons.Quality;
            statusSensors.Value = EntityManager.Player.Sensor.Quality;
            statusCloak.Value = EntityManager.Player.Cloak.Quality;
            buttonAutoGun.Fore(EntityManager.Player.AutoGun ? Color.Lime : Color.White);
            buttonAutoAim.Fore(EntityManager.Player.AutoAim ? Color.Lime : Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            buttonAutoGun.Setup(this);
            buttonAutoAim.Setup(this);
            Update();
        }
    }
}
