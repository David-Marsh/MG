using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MG.Shared.UI.Panels
{
    public class Diagnostics : Panel
    {
        public Label lblDraw;
        public Label lblUpdates;
        public Label lblGarbage;
        private static readonly Stopwatch stopWatch = new();
        private static double tsUpdate;
        private static double tsDraw;

        private const string category = ".NET CLR Memory";
        private const string counter = "% Time in GC";
        private readonly string instance;
        private PerformanceCounter gcPerf;

        public Diagnostics(Color back, Color fore, int col, int row, int colspan, int rowspan) : base(back, fore, col, row, colspan, rowspan)
        {
            Rows = 3;
            Collums = 4;
            lblDraw = new(Colors.Fore, "Draw 0.00%", 0,0,4,1);
            lblUpdates = new(Colors.Fore, "Update 0.00%", 0, 1, 4, 1);
            lblGarbage = new(Colors.Fore, "Garbage 0.00%", 0, 2, 4, 1);
            Controls.Add(lblDraw);
            Controls.Add(lblUpdates);
            Controls.Add(lblGarbage);
            instance = Process.GetCurrentProcess().ProcessName;
            instance = "devenv";
        }
        public static void StartTime() => stopWatch.Start();
        public static void StopUpdateTime()
        {
            stopWatch.Stop();
            tsUpdate = tsUpdate * 0.99f + 0.01f * stopWatch.Elapsed.TotalMilliseconds / 16.66666666666667;
            stopWatch.Reset();
        }
        public static void StopDrawTime()
        {
            stopWatch.Stop();
            tsDraw = tsDraw * 0.99f + 0.01f * stopWatch.Elapsed.TotalMilliseconds / 16.66666666666667;
            stopWatch.Reset();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            base.Draw(spriteBatch);
            foreach (var control in Controls) control.Draw(spriteBatch);
        }
        public override void Setup()
        {
            base.Setup();
            Padding = (int)Cellsize.X / 8;
            lblDraw.Setup(this);
            lblUpdates.Setup(this);
            lblGarbage.Setup(this);
        }
        public override void Update(GameTime gameTime)
        {
            if (!Visible) return;
            base.Update(gameTime);
            lblUpdates.Msg.Text = (tsUpdate).ToString("Update 0.00%");
            lblDraw.Msg.Text = (tsDraw).ToString("Draw 0.00%");
            // make sure the performance counter is available to query
            if (PerformanceCounterCategory.Exists(category) &&
                PerformanceCounterCategory.CounterExists(counter, category) &&
                PerformanceCounterCategory.InstanceExists(instance, category))
            {
                gcPerf = new PerformanceCounter(category, counter, instance);
                lblGarbage.Msg.Text = (gcPerf.NextValue()).ToString("Garbage 0.00%");
            }
        }
    }
}
