using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D.Core
{
    public class BodyContactListener : ContactListener
    {
        public void BeginContact(Contact contact)
        {
            if (contact.FixtureA.UserData is IPhysicsCollisionCallback fixtureA)
            {
                fixtureA.OnCollisionEnter(contact.FixtureB.Body);
            }

            if (contact.FixtureB.UserData is IPhysicsCollisionCallback fixtureB)
            {
                fixtureB.OnCollisionEnter(contact.FixtureA.Body);
            }
        }

        public void EndContact(Contact contact)
        {
            if (contact.FixtureA.UserData is IPhysicsCollisionCallback fixtureA)
            {
                fixtureA.OnCollisionExit(contact.FixtureB.Body);
            }

            if (contact.FixtureB.UserData is IPhysicsCollisionCallback fixtureB)
            {
                fixtureB.OnCollisionExit(contact.FixtureA.Body);
            }
        }

        public void PreSolve(Contact contact, Manifold oldManifold)
        {
            // Left blank intentionally
        }

        public void PostSolve(Contact contact, ContactImpulse impulse)
        {
            // Left blank intentionally
        }
    }
}