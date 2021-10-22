using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace FarmShooter
{
    class DTRenderWindow : RenderWindow
    {
        public Time DeltaTime { get; private set; }
        private Clock DeltaTimeClock;

        public void UpdateDeltaTime() 
        {
            DeltaTime = DeltaTimeClock.Restart();
        }

        public DTRenderWindow(VideoMode vm, string title) : base(vm, title)
        {
            DeltaTime = new Time();
            DeltaTimeClock = new Clock();
        }
    }
}
