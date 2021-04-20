using MG.Shared.Background;
using MG.Shared.Global;
using MG.VoidControl.Ship;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MG.Shared.VoidControl
{
	public class EntityManager : DrawableGameComponent
	{
		private readonly SpriteBatch spriteBatch;
		private static ParticleManager particleManager;
		#region Entitys
		private static readonly List<Entity> entities = new();
		private static readonly List<Bullet> bullets = new();
		private static readonly List<VoidShip> ships = new();
        public static Player Player { get; set; }
		#endregion
        #region UI
        private static bool autoSpawnShipList;
		private static bool clearShipList;
		private static bool spawnShipList;
        #endregion
        #region Spawn
        private static int maskSize;
		private static int mask;
		private static int multiplier;
		private static Point priorUID;
		private static Point UID;
		private static Point point;
		private static Vector2 npPosition;
		private const uint filter = 0x00100100;
		public static int MaskSize
		{
			get => maskSize; set
			{
				maskSize = value;
				multiplier = 1 << maskSize;
				mask = multiplier - 1;
				VoidShip.MaxShipDistanceSquared = (float)Math.Pow(multiplier * maskSize, 2);
			}
		}
        #endregion
		public EntityManager(Game game, GraphicsDeviceManager graphics) : base(game)
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Entity.Initialize(Game.Content);
			Sound.LoadContent(Game.Content);
			Player = new Player();
            graphics.DeviceReset += Player.OnResize;
            Player.OnResize(graphics, null);
            entities.Add(Player);
            MaskSize = 11;
            priorUID = new Point(1, 1);
            PID.Tuning = new(0.02f, 0, 3f);
            autoSpawnShipList = true;
			BackgroundManager.Initialize(graphics);
			particleManager = new();
		}
		#region UI control
		public static int ShipCount() => ships.Count;
        public static void Clear() => clearShipList = true;
		public static void Spawn() => spawnShipList = true;
        public static void ExecuteUI()
        {
			if (clearShipList)
			{
				autoSpawnShipList = clearShipList = false;
				foreach (VoidShip ship in ships)
					ship.IsExpired = true;
				priorUID.X = priorUID.Y = 1;
			}
			if (spawnShipList)
			{
				autoSpawnShipList = spawnShipList = false;
				NonPlayer nonPlayer = new(Player.Position + Vector2.UnitX * 1000);
				PID.Tuning = nonPlayer.PID;
				Add(nonPlayer);
			}
		}
		#endregion
		#region Add Remove Spawn
		public static void Add(Entity entity)
		{
			if (entity is Bullet)
				bullets.Add(entity as Bullet);
			if (entity is VoidShip)
				ships.Add(entity as VoidShip);
			entities.Add(entity);
		}
		private static void RemoveExpired()
		{
			ships.RemoveAll(r => r.OutOfRange);
			bullets.RemoveAll(r => r.IsExpired);
			entities.RemoveAll(r => r.IsExpired);
		}
        private static void Spawn(Vector2 position)
        {
			if (SameGridCell(position)) return;
			UID.X -= multiplier * maskSize;
			UID.Y -= multiplier * maskSize;
			point = UID;

			for (int x = -maskSize; x <= maskSize; x++)
			{
				for (int y = -maskSize; y <= maskSize; y++)
				{
					npPosition = point.ToVector2();
					if((position-npPosition).LengthSquared() <= VoidShip.MaxShipDistanceSquared)
                    {
						if (!ships.Exists(w => w.UID.Equals(point)))
						{
							uint hash = Hash(point);
							if ((hash & filter) == filter)
							{
								Add(new NonPlayer(npPosition, hash, maskSize, mask, point));
							}
						}
					}
					point.Y += multiplier;
				}
				point.X += multiplier;
				point.Y = UID.Y;
			}
		}
		private static bool SameGridCell(Vector2 position)
        {
			UID = position.ToPoint();
			UID.X &= ~mask;
			UID.Y &= ~mask;
			if (priorUID == UID) return true;						// check entry to new grid cell
			priorUID = UID;                                         // note new grid cell
			return false;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static uint Hash(Point point)
        {
			uint hash = 1337;
			hash ^= 1619 * BitConverter.ToUInt32(BitConverter.GetBytes(point.Y), 0);
			hash ^= System.Numerics.BitOperations.RotateRight(hash, 13);
			hash ^= 31337 * BitConverter.ToUInt32(BitConverter.GetBytes(point.X), 0);
			hash ^= System.Numerics.BitOperations.RotateLeft(hash, 23);
			hash = hash * hash * hash * 60493;
			hash = System.Numerics.BitOperations.RotateRight(hash, 13) ^ hash;
			return hash;
		}
		#endregion
		public override void Update(GameTime gameTime)
        {
			Player.Update(gameTime);
			foreach (VoidShip ship in ships)
				ship.Update(gameTime);
			foreach (Bullet bullet in bullets)
				bullet.Update(gameTime);
			ShipCollision();
			BulletCollision();
			ExecuteUI();
			RemoveExpired();
			if (autoSpawnShipList) Spawn(Player.Position);
			BackgroundManager.Update(Player.Position);
			particleManager.Update();
			base.Update(gameTime);
        }
		private static void ShipCollision()
		{
			for (int i = 0; i < ships.Count; i++)
			{
				if ((ships[i].IsExpired) | (!ships[i].TargetDetected)) continue;
				if (Player.IsColliding(ships[i])) Player.HandleCollision(ships[i]);
				for (int j = i + 1; j < ships.Count; j++)
				{
					if (!ships[j].TargetDetected) continue;
					if (ships[j].IsColliding(ships[i])) ships[i].HandleCollision(ships[j]);
				}
			}
		}
		private static void BulletCollision()
		{
			foreach (Bullet bullet in bullets)
            {
				if (bullet.IsExpired) continue;
				if (bullet.IsColliding(Player)) Player.HandleCollision(bullet);
				foreach (VoidShip ship in ships)
                {
					if (ship.IsExpired) continue;
					if (!ship.TargetDetected) continue;
					if (bullet.IsColliding(ship)) ship.HandleCollision(bullet);
					if (ship.IsExpired) particleManager.Add(ship.Position);
				}
			}
		}
		public static Vector2 AvoidShips(Vector2 position)
		{
			Vector2 avoid = Vector2.Zero;
			Vector2 numerator = new(10000);
			foreach (VoidShip ship in ships)
			{
				if (ship.IsExpired) continue;
				if (!ship.TargetDetected) continue;
				if (ship == Player) continue;
				if (position != ship.Position) avoid += numerator / (position - ship.Position);
			}
			return avoid;
		}
		public static VoidShip NearestShip()
		{
			float min = ships.Where(entry => entry.IsExpired == false).Min(entry => entry.TargetRelativePositionSquared);
			return ships.Find(w => w.TargetRelativePositionSquared == min);
		}
		#region Draw
		public override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(transformMatrix: Player.ViewMatrix());			// Map the world space around the player to the full screen
			BackgroundManager.Draw(spriteBatch);								// Draw background below entitys first
			foreach (Entity entity in entities) entity.Draw(spriteBatch);
			particleManager.Draw(spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
		public static void DrawMap(SpriteBatch spriteBatch, Rectangle destrec)	// Draw dots on minimap
		{
			spriteBatch.Draw(Sprite.Pixel, destrec, Color.Lime);
			for (int i = 1; i < ships.Count; i++)
            {
				if (ships[i].IsExpired) continue;
				destrec.Location = ships[i].Position.ToPoint();
				spriteBatch.Draw(Sprite.Pixel, destrec, Color.Red);
			}
        }
		#endregion
	}
}
