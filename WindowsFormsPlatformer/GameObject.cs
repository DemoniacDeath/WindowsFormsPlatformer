using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsPlatformer
{
    class GameObject
    {
        private GameContext m_context;
        private Rect m_frame;
        private IList<GameObject> m_children = new List<GameObject>();
        private GameObject m_parent;
        private RenderObject m_renderObject;
        private PhysicsState m_physics;
        private Animation m_animation;
        private bool m_removed = false;
        private bool m_visible = true;

        public GameObject(GameContext context)
        {
            m_context = context;
        }

        public GameObject(GameContext context, Rect frame) : this(context)
        {
            m_frame = frame;
        }

        protected GameContext Context
        {
            get { return m_context; }
        }

        public Rect Frame
        {
            get { return m_frame; }
            set { m_frame = value; }
        }

        public IList<GameObject> Children
        {
            get { return m_children; }
        }

        public GameObject Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public RenderObject RenderObject
        {
            get { return m_renderObject; }
            set { m_renderObject = value; }
        }

        public PhysicsState Physics
        {
            get { return m_physics; }
            set { m_physics = value; }
        }

        public Animation Animation
        {
            get { return m_animation; }
            set { m_animation = value; }
        }

        public bool IsRemoved
        {
            get { return m_removed; }
        }

        public void Remove()
        {
            m_removed = true;
        }

        public bool IsVisible
        {
            get { return m_visible; }
        }

        public virtual void KeyDown(Keys key)
        {
            foreach (var child in Children)
                child.KeyDown(key);
        }

        public virtual void HandleKeyboardState(IList<Keys> keys)
        {
            foreach (var child in Children)
                child.HandleKeyboardState(keys);
        }

        public void ProcessPhysics()
        {
            if (Physics != null)
                Physics.Change();

            foreach (var child in Children)
                child.ProcessPhysics();
        }

        public void DetectCollisions()
        {
            var allColliders = new List<GameObject>();
            DetectCollisions(allColliders);
            int size = allColliders.Count;
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    allColliders[i].Physics.DetectCollision(allColliders[j].Physics);
                }
            }
        }

        protected void DetectCollisions(IList<GameObject> allColliders)
        {
            if (Physics != null)
                allColliders.Add(this);
            foreach (var child in Children)
            {
                child.DetectCollisions(allColliders);
            }
        }

        public virtual void HandleEnterCollision(Collision collision) { }

        public virtual void HandleExitCollision(GameObject collider) { }

        public virtual void HandleCollision(Collision collision) { }

        public void Animate()
        {

        }

        public virtual void Render(Graphics renderer, Vector localBasis, Vector cameraPosition, Size cameraSize)
        {
            if (IsVisible && RenderObject != null)
            {
                Vector globalPosition = Frame.Center;
                globalPosition += localBasis;
                RenderObject.Render(renderer, Context, globalPosition, Frame.Size, cameraPosition, cameraSize);
            }
            foreach (var child in Children)
            {
                child.Render(renderer, localBasis + Frame.Center, cameraPosition, cameraSize);
            }
        }

        public void AddChild(GameObject child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void Clean()
        {
            var childrenToRemove = from child in Children
                                   where child.IsRemoved
                                   select child;
            foreach (var child in childrenToRemove)
                Children.Remove(child);
        }

        public Vector GlobalPosition()
        {
            if (m_parent == null)
            {
                return m_frame.Center;
            }
            else
            {
                return m_frame.Center + Parent.GlobalPosition();
            }
        }
    }
}
