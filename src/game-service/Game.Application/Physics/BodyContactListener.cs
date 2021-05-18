using Box2DX.Collision;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public class BodyContactListener : ContactListener
    {
        public void BeginContact(Contact contact)
        {
            {
                var userData = contact.FixtureA.UserData;
                var body = contact.FixtureB.Body;

                if (userData is IContactEvents contactEvents)
                {
                    contactEvents.OnBeginContact(body);
                }
            }

            {
                var userData = contact.FixtureB.UserData;
                var body = contact.FixtureA.Body;

                if (userData is IContactEvents contactEvents)
                {
                    contactEvents.OnBeginContact(body);
                }
            }
        }

        public void EndContact(Contact contact)
        {
            {
                var userData = contact.FixtureA.UserData;
                var body = contact.FixtureB.Body;

                if (userData is IContactEvents contactEvents)
                {
                    contactEvents.OnEndContact(body);
                }
            }

            {
                var userData = contact.FixtureB.UserData;
                var body = contact.FixtureA.Body;

                if (userData is IContactEvents contactEvents)
                {
                    contactEvents.OnEndContact(body);
                }
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