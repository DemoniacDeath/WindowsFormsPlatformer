namespace WindowsFormsPlatformer.GameObjects
{
    class Frame : GameObject
    {
        private double m_width = 0;
        private Solid m_floor;
        private Solid m_wallLeft;
        private Solid m_wallRight;
        private Solid m_ceiling;

        public double Width
        {
            get { return m_width; }
        }

        public Solid Floor
        {
            get { return m_floor; }
        }

        public Solid WallLeft
        {
            get { return m_wallLeft; }
        }

        public Solid WallRight
        {
            get { return m_wallRight; }
        }

        public Solid Ceiling
        {
            get { return m_ceiling; }
        }

        public Frame(GameContext context, Rect frame, double width) : base(context, frame)
        {
            m_width = width;
            m_ceiling = new Solid(context, new Rect(
                0,
                -Frame.Size.Height / 2 + width / 2,
                Frame.Size.Width,
                width));
            AddChild(m_ceiling);
            m_wallLeft = new Solid(context, new Rect(
                -Frame.Size.Width / 2 + width / 2,
                0,
                width,
                Frame.Size.Height - width * 2));
            AddChild(m_wallLeft);
            m_wallRight = new Solid(context, new Rect(
                Frame.Size.Width / 2 - width / 2,
                0,
                width,
                Frame.Size.Height - width * 2));
            AddChild(m_wallRight);
            m_floor = new Solid(context, new Rect(
                0,
                Frame.Size.Height / 2 - width / 2,
                Frame.Size.Width,
                width));
            AddChild(m_floor);
        }
    }
}
