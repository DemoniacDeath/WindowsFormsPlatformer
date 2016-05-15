using WindowsFormsPlatformer.GameObjects;

namespace WindowsFormsPlatformer
{
    class GameContext
    {
        private World m_world;
        private UI m_ui;
        private Size m_windowSize;
        private bool m_quit = false;

        public World World
        {
            get { return m_world; }
        }

        public UI UI
        {
            get { return m_ui; }
        }

        public Size WindowSize
        {
            get { return m_windowSize; }
            set { m_windowSize = value; }
        }

        public bool IsQuit
        {
            get { return m_quit; }
        }

        public void Quit()
        {
            m_quit = true;
        }

        public GameContext(Size windowSize)
        {
            m_windowSize = windowSize;
            m_world = new World(this, new Rect(0,0,windowSize.Width/2, windowSize.Height/2));
            m_ui = new UI(this, new Rect(Vector.Zero, m_world.Camera.OriginalSize));
        }
    }
}
