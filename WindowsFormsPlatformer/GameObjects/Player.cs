using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class Player : GameObject
    {
        private double m_speed;
        private double m_jumpSpeed;
        private int m_power;
        private bool m_jumped;
        private Size m_originalSize;
        private bool m_crouched;
        private int m_health = 100;
        private bool m_dead;
        private bool m_won;
        private Animation m_idleAnimation;
        private Animation m_moveAnimation;
        private Animation m_jumpAnimation;
        private Animation m_crouchAnimation;
        private Animation m_crouchMoveAnimation;

        public double Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        public double JumpSpeed
        {
            get { return m_jumpSpeed; }
            set { m_jumpSpeed = value; }
        }

        public Size OriginalSize
        {
            get { return m_originalSize; }
        }

        public Animation IdleAnimation
        {
            get { return m_idleAnimation; }
            set { m_idleAnimation = value; }
        }

        public Animation MoveAnimation
        {
            get { return m_moveAnimation; }
            set { m_moveAnimation = value; }
        }

        public Animation JumpAnimation
        {
            get { return m_jumpAnimation; }
            set { m_jumpAnimation = value; }
        }

        public Animation CrouchAnimation
        {
            get { return m_crouchAnimation; }
            set { m_crouchAnimation = value; }
        }

        public Animation CrouchMoveAnimation
        {
            get { return m_crouchMoveAnimation; }
            set { m_crouchMoveAnimation = value; }
        }

        public bool Crouched
        {
            get { return m_crouched; }
            set
            {
                if (value && !m_crouched)
                {
                    m_crouched = true;
                    Frame = new Rect(
                        Frame.Center.X,
                        Frame.Center.Y + OriginalSize.Height / 4,
                        Frame.Size.Width,
                        OriginalSize.Height / 2);
                }
                else if (!value && m_crouched)
                {
                    m_crouched = false;
                    Frame = new Rect(
                        Frame.Center.X,
                        Frame.Center.Y - OriginalSize.Height / 4,
                        Frame.Size.Width,
                        OriginalSize.Height);
                }
            }
        }

        public Player(GameContext context, Rect frame) : base(context, frame)
        {
            Physics = new PhysicsState(this);
            Physics.Gravity = true;
            Physics.Still = false;
            m_originalSize = frame.Size;
        }

        public override void KeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.G:
                    Physics.Gravity = !Physics.Gravity;
                    if (!Physics.Gravity)
                    {
                        m_jumped = true;
                        Physics.Velocity *= 0;
                    }
                    break;
            }
            base.KeyDown(key);
        }

        public override void HandleKeyboardState(IList<Keys> keys)
        {
            if (!m_dead)
            {
                bool sitDown = false;
                bool moveLeft = false;
                bool moveRight = false;
                Vector moveVector = new Vector();
                if (keys.Contains(Keys.Left) || keys.Contains(Keys.A))
                {
                    moveVector += new Vector(-m_speed, 0);
                    moveLeft = true;
                }
                if (keys.Contains(Keys.Right) || keys.Contains(Keys.D))
                {
                    moveVector += new Vector(m_speed, 0);
                    moveRight = true;
                }
                if (keys.Contains(Keys.Up) || keys.Contains(Keys.W) || keys.Contains(Keys.Space))
                {
                    if (!Physics.Gravity)
                    {
                        moveVector += new Vector(0, -m_speed);
                    }
                    else
                    {
                        if (!m_jumped)
                        {
                            Physics.Velocity += new Vector(0, -m_jumpSpeed);
                            m_jumped = true;
                        }
                    }
                }
                if (keys.Contains(Keys.Down) || keys.Contains(Keys.S) || keys.Contains(Keys.ControlKey))
                {
                    if (!Physics.Gravity)
                    {
                        moveVector += new Vector(0, m_speed);
                    }
                    else
                    {
                        sitDown = true;
                    }
                }
                Crouched = sitDown;

                //if (moveLeft && !moveRight)
                //{
                //    MoveAnimation.turnLeft(true);
                //    CrouchAnimation.turnLeft(true);
                //    CrouchMoveAnimation.turnLeft(true);
                //}
                //if (moveRight && !moveLeft)
                //{
                //    MoveAnimation.turnLeft(false);
                //    CrouchAnimation.turnLeft(false);
                //    CrouchMoveAnimation.turnLeft(false);
                //}

                if (!moveLeft && !moveRight && !m_jumped && !m_crouched)
                    Animation = IdleAnimation;
                if (!moveLeft && !moveRight && !m_jumped && m_crouched)
                    Animation = CrouchAnimation;
                if ((moveLeft || moveRight) && !m_jumped && !m_crouched)
                    Animation = MoveAnimation;
                if ((moveLeft || moveRight) && !m_jumped && m_crouched)
                    Animation = CrouchMoveAnimation;
                if (m_jumped && m_crouched)
                    Animation = CrouchAnimation;
                if (m_jumped && !m_crouched)
                    Animation = JumpAnimation;

                Frame = new Rect(Frame.Center + moveVector, Frame.Size);
            }
            base.HandleKeyboardState(keys);
        }

        public override void HandleEnterCollision(Collision collision)
        {
            var consumable = collision.Collider as Consumable;
            if (consumable != null)
            {
                m_power += 1;
                //Context.UI.PowerBar.Value = m_power;
                consumable.Remove();
                m_speed += 0.01;
                m_jumpSpeed += 0.01;
                if (m_power > 99)
                {
                    Win();
                }
            }
        }

        public override void HandleExitCollision(GameObject collider)
        {
            if (Physics.Colliders.Count == 0)
            {
                m_jumped = true;
            }
        }

        public override void HandleCollision(Collision collision)
        {
            if (Math.Abs(collision.CollisionVector.X) > Math.Abs(collision.CollisionVector.Y))
            {
                if (collision.CollisionVector.Y > 0 && m_jumped && Physics.Gravity)
                {
                    m_jumped = false;
                }
            }
        }

        public void DealDamage(int damage)
        {
            if (!m_won)
            {
                m_health -= damage;
                //Context.UI.HealthBar.Value = m_health;
                if (m_health < 0)
                {
                    Die();
                }
            }
        }

        public void Die()
        {
            m_dead = true;
            //Context.UI.DeathText.Visible = true;
        }

        public void Win()
        {
            m_won = true;
            //Context.UI.WinText.Visible = true;
        }
    }
}
