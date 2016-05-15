using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class Camera : GameObject
    {
        private Size m_originalSize;

        public Size OriginalSize
        {
            get { return m_originalSize; }
        }

        public Camera(GameContext context, Rect frame) : base(context, frame)
        {
            m_originalSize = new Size(frame.Size.Width / 2, frame.Size.Height / 2);
        }

        public override void HandleKeyboardState(IList<Keys> keys)
        {
            if (keys.Contains(Keys.Z))
            {
                Frame = new Rect(Frame.Center, new Size (OriginalSize.Width * 2, OriginalSize.Height * 2));
            }
            else
            {
                Frame = new Rect(Frame.Center, OriginalSize);
            }
        }
    }
}