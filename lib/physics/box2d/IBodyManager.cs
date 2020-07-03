namespace Physics.Box2D
{
    public interface IBodyManager
    {
        void AddBody(NewBodyData newBodyData);

        void RemoveBody(int id);

        bool GetBody(int id, out BodyData bodyData);
    }
}