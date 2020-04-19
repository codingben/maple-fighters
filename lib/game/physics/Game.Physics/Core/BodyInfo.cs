namespace Physics.Box2D.Core
{
    public struct BodyInfo
    {
        public readonly int Id;
        public readonly BodyDefinitionWrapper BodyDefinition;

        public BodyInfo(int id, BodyDefinitionWrapper bodyDefinition)
        {
            Id = id;
            BodyDefinition = bodyDefinition;
        }
    }
}