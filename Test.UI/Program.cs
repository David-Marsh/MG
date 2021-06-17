using MG.Shared.Test.UI;
using System;

namespace Test.UI
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TestUI())
                game.Run();
        }
    }
}
