using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlatformer.GameObjects
{
    class Solid : GameObject
    {
        public Solid(GameContext context, Rect frame) : base(context, frame)
        {
            Physics = new PhysicsState(this);
        }

        public override void HandleEnterCollision(Collision collision)
        {
        }

        public override void HandleExitCollision(GameObject collider)
        {
        }
    }
}
