using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsPlatformer
{
    class Animation
    {
        private IList<RenderObject> m_frames;
        private int m_startTick = 0;
        private int m_speed;
        private bool m_turnedLeft = false;

        public Animation(int speed, int startTick) : this(new List<RenderObject>(), speed, startTick)
        {
        }

        public Animation(IList<RenderObject> frames, int speed, int startTick)
        {
            m_frames = frames;
            m_speed = speed;
            m_startTick = startTick;
        }

        public IList<RenderObject> Frames
        {
            get { return m_frames; }
        }

        public int StartTick
        {
            get { return m_startTick; }
            internal set { m_startTick = value; }
        }

        public int Speed
        {
            get { return m_speed; }
        }

        public bool TurnedLeft
        {
            get { return m_turnedLeft; }
            set
            {
                //SDL_RendererFlip flip;
                if (value && !m_turnedLeft)
                {
                    //flip = SDL_FLIP_HORIZONTAL;
                    m_turnedLeft = true;
                }
                else if (!value && m_turnedLeft)
                {
                    //flip = SDL_FLIP_NONE;
                    m_turnedLeft = false;
                }
                else
                    return;

                foreach (var frame in Frames)
                    frame.RenderFlip = m_turnedLeft;
            }
        }

        public RenderObject Animate(int ticks)
        {
            if (ticks - StartTick >= Frames.Count * Speed)
                StartTick = ticks;
            var frameIndex = (ticks - StartTick) / Speed;
            return Frames[frameIndex];
        }

        public static Animation WithSingleRenderObject(int ticks, RenderObject renderObject)
        {
            return new Animation(new List<RenderObject> { renderObject }, 1, ticks);
        }

        public static Animation WithSpeedAndColor(int ticks, int speed, Color color)
        {
            var animation = new Animation(speed, ticks);
            animation.Frames.Add(RenderObject.FromColor(color));
            animation.Frames.Add(RenderObject.FromColor(Color.Black));
            return animation;
        }

        public static Animation WithSpeedAndImage(int ticks, int speed, Bitmap image, int width, int height, int frames)
        {
            var animation = new Animation(speed, ticks);
            Rectangle rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            rect.X = 0;
            rect.Y = 0;
            for (int i = 0; i < frames; i++)
            {
                animation.Frames.Add(new RenderObject(image, rect));
                rect.Y = i * rect.Height;
            }
            return animation;
        }
    }
}