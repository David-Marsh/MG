using MG.Shared.UI.Controls;
using MG.Shared.VoidControl.Ship;
using Microsoft.Xna.Framework;
using System;

namespace MG.Shared.VoidControl.UI.Panels
{
    public class ShipStatus : UIPanel
    {
        private readonly UIStatusBar statusReactor;
        private readonly UIStatusBar statusThruster;
        private readonly UIStatusBar statusShield;
        private readonly UIStatusBar statusWeapons;
        private readonly UIStatusBar statusSensors;
        private readonly UIStatusBar statusCloak;
        private readonly UIButton buttonAutoGun;
        private readonly UIButton buttonAutoAim;
        public VoidShipPC Player;
        public ShipStatus(int x, int y, int width, int height, Color back = default) : base(x, y, width, height, back)
        {
            statusReactor = new(0, 0, 4, 2, "Reactor", Colors.Back, Colors.Fore);
            statusThruster = new(0, 2, 4, 2, "Thruster", Colors.Back, Colors.Fore);
            statusShield = new(0, 4, 4, 2, "Shield", Colors.Back, Colors.Fore);
            statusWeapons = new(4, 0, 4, 2, "Weapons", Colors.Back, Colors.Fore);
            statusSensors = new(4, 2, 4, 2, "Sensors", Colors.Back, Colors.Fore);
            statusCloak = new(4, 4, 4, 2, "Cloak", Colors.Back, Colors.Fore);
            buttonAutoGun = new(0, 6, 2, 1, "AutoGun", Colors.Back, Colors.Fore);
            buttonAutoAim = new(2, 6, 2, 1, "AutoAim", Colors.Back, Colors.Fore);

            Children.Add(statusReactor);
            Children.Add(statusThruster);
            Children.Add(statusShield);
            Children.Add(statusWeapons);
            Children.Add(statusSensors);
            Children.Add(statusCloak);
            Children.Add(buttonAutoGun);
            Children.Add(buttonAutoAim);

            statusReactor.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Reactor.Quality = a.Value; });
            statusThruster.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Thruster.Quality = a.Value; });
            statusShield.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Shield.Quality = a.Value; });
            statusWeapons.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Weapons.Quality = a.Value; });
            statusSensors.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Sensor.Quality = a.Value; });
            statusCloak.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
            { Player.Cloak.Quality = a.Value; });
            buttonAutoGun.Clicked += new EventHandler(delegate (object o, EventArgs a)
            { Player.Weapons.AutoGun = !Player.Weapons.AutoGun;  });
            buttonAutoAim.Clicked += new EventHandler(delegate (object o, EventArgs a)
            { Player.Weapons.AutoAim = !Player.Weapons.AutoAim;  });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            statusReactor.Value = Player.Reactor.Quality;
            statusThruster.Value = Player.Thruster.Quality;
            statusShield.Value = Player.Shield.Quality;
            statusWeapons.Value = Player.Weapons.Quality;
            statusSensors.Value = Player.Sensor.Quality;
            statusCloak.Value = Player.Cloak.Quality;
            buttonAutoGun.Fore(Player.Weapons.AutoGun ? Color.Lime : Color.White);
            buttonAutoAim.Fore(Player.Weapons.AutoAim ? Color.Lime : Color.White);
        }
    }
}
