using System.Collections.Generic;

namespace WindowsFormsPlatformer
{
    internal class PhysicsState
    {
        private GameObject m_gameObject;
        private Vector m_velocity;
        private bool m_gravity = false;
        private bool m_still = true;
        private double m_gravityForce = 0;
        private IList<GameObject> m_colliders = new List<GameObject>();

        public PhysicsState(GameObject gameObject)
        {
            m_gameObject = gameObject;
        }

        public GameObject GameObject
        {
            get { return m_gameObject; }
        }

        public Vector Velocity
        {
            get { return m_velocity; }
            set { m_velocity = value; }
        }

        public bool Gravity
        {
            get { return m_gravity; }
            set { m_gravity = value; }
        }

        public bool Still
        {
            get { return m_still; }
            set { m_still = value; }
        }

        public double GravityForce
        {
            get { return m_gravityForce; }
            set { m_gravityForce = value; }
        }

        public IList<GameObject> Colliders
        {
            get { return m_colliders; }
        }

        public void Change()
        {

        }

        public void DetectCollision(PhysicsState collider)
        {

        }
    }
}