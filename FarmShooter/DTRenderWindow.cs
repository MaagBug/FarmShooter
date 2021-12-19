namespace FarmShooter
{
    public class DTRenderWindow : RenderWindow
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
