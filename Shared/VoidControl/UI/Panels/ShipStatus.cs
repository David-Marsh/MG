using MG.Shared.UI;
using MG.Shared.UI.Controls;
using MG.Shared.VoidControl.Ship;
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
    private readonly StatusBar statusECM;
    private readonly Button buttonAutoGun;
    private readonly Button buttonAutoAim;
    public VoidShipPC Player;
    public ShipStatus(int x, int y, int width, int height) : base(x, y, width, height)
    {
      statusReactor = new(0, 0, 4, 2, "Reactor", Colors);
      statusShield = new(4, 0, 4, 2, "Shield", Colors);
      statusThruster = new(0, 2, 4, 2, "Thruster", Colors);
      statusWeapons = new(4, 2, 4, 2, "Weapons", Colors);
      statusSensors = new(0, 4, 4, 2, "Sensors", Colors);
      statusECM = new(4, 4, 4, 2, "ECM", Colors);
      buttonAutoGun = new(0, 6, 2, 1, "AutoGun", Colors);
      buttonAutoAim = new(2, 6, 2, 1, "AutoAim", Colors);

      Children.Add(statusReactor);
      Children.Add(statusThruster);
      Children.Add(statusShield);
      Children.Add(statusWeapons);
      Children.Add(statusSensors);
      Children.Add(statusECM);
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
      statusECM.PushStatus += new EventHandler<PushStatusEventArgs>(delegate (object o, PushStatusEventArgs a)
      { Player.ECM.Quality = a.Value; });
      buttonAutoGun.Clicked += new EventHandler(delegate (object o, EventArgs a)
      { Player.Weapons.AutoGun = !Player.Weapons.AutoGun; });
      buttonAutoAim.Clicked += new EventHandler(delegate (object o, EventArgs a)
      { Player.Weapons.AutoAim = !Player.Weapons.AutoAim; });
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      statusReactor.Value = Player.Reactor.Quality;
      statusThruster.Value = Player.Thruster.Quality;
      statusShield.Value = Player.Shield.Quality;
      statusWeapons.Value = Player.Weapons.Quality;
      statusSensors.Value = Player.Sensor.Quality;
      statusECM.Value = Player.ECM.Quality;
      buttonAutoGun.Fore(Player.Weapons.AutoGun ? Color.Lime : Color.White);
      buttonAutoAim.Fore(Player.Weapons.AutoAim ? Color.Lime : Color.White);
    }
  }
}
