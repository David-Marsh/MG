using System;

namespace MG.Shared.VoidControl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new VoidControl();
            game.Run();
        }
    }
}
