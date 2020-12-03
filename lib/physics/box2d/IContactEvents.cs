using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IContactEvents
    {
        void OnBeginContact(Body body);

        void OnEndContact(Body body);
    }
}