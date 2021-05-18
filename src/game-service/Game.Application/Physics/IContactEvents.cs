using Box2DX.Dynamics;

namespace Game.Physics
{
    public interface IContactEvents
    {
        void OnBeginContact(Body body);

        void OnEndContact(Body body);
    }
}