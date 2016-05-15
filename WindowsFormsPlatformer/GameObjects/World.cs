using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class World : GameObject
    {
        private Camera m_camera;

        public Camera Camera
        {
            get { return m_camera; }
        }

        public World(GameContext context, Rect frame) : base(context, frame)
        {
            m_camera = new Camera(context, frame);
        }

        public void Render(Graphics renderer)
        {
            base.Render(renderer, Frame.Center, Camera.GlobalPosition(), Camera.Frame.Size);
        }

        public override void KeyDown(Keys key)
        {
            base.KeyDown(key);
            if (key == Keys.Q)
            {
                Context.Quit();
            }
        }
    }
}
