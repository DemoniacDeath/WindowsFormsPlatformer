using System;
using System.Drawing;

namespace WindowsFormsPlatformer
{
    class RenderObject
    {
        private bool m_fullRender;
        private Bitmap m_texture;
        private Rectangle m_renderFrame;
        private bool m_renderFlip;

        public RenderObject(Bitmap texture)
        {
            m_texture = texture;
            m_fullRender = true;
        }

        public RenderObject(Bitmap texture, Rectangle renderFrame) : this(texture)
        {
            m_fullRender = false;
            m_renderFrame = renderFrame;
        }

        public bool FullRender
        {
            get { return m_fullRender; }
        }

        public Bitmap Texture
        {
            get { return m_texture; }
        }

        public Rectangle RenderFrame
        {
            get { return m_renderFrame; }
            set { m_renderFrame = value; }
        }

        public bool RenderFlip
        {
            get { return m_renderFlip; }
            set { m_renderFlip = value; }
        }

        public void Render(Graphics renderer, GameContext context, Vector position, Size size, Vector cameraPosition, Size cameraSize)
        {
            Vector renderPosition = position + new Vector(-size.Width / 2, -size.Height / 2) - cameraPosition - new Vector(-cameraSize.Width / 2, -cameraSize.Height / 2);
            RectangleF targetRect = new RectangleF();
            targetRect.X = (float)(context.WindowSize.Width * (renderPosition.X / cameraSize.Width));
            targetRect.Y = (float)(context.WindowSize.Height * (renderPosition.Y / cameraSize.Height));
            targetRect.Width = (float)(context.WindowSize.Width * (size.Width / cameraSize.Width));
            targetRect.Height = (float)(context.WindowSize.Height * (size.Height / cameraSize.Height));
            renderer.DrawImage(m_texture, targetRect);
        }

        public static RenderObject FromColor(Color color)
        {
            Bitmap texture = new Bitmap(1, 1);
            using (Graphics gfx = Graphics.FromImage(texture))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, 0, 0, 1, 1);
            }
            return new RenderObject(texture);
        }
    }
}
