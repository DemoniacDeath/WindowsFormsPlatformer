using System.Drawing;

namespace WindowsFormsPlatformer.GameObjects
{
    class UI : UIElement
    {
        public UI(GameContext context, Rect frame) : base(context, frame) {}

        public void Render(Graphics renderer)
        {
            base.Render(renderer, Frame.Center, Vector.Zero, Context.World.Camera.OriginalSize);
        }
    }
}
