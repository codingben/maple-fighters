namespace Physics.Box2D
{
    public struct BodyInfo
    {
        public readonly int Id;
        public readonly BodyDefinitionWrapper Body;

        public BodyInfo(int id, BodyDefinitionWrapper body)
        {
            Id = id;
            Body = body;
        }
    }
}