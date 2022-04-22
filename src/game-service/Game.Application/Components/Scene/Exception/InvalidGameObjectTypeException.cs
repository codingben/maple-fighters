using System;

namespace Game.Application.Components
{
    public class InvalidGameObjectTypeException : Exception
    {
        public InvalidGameObjectTypeException(ObjectTypes objectType)
            : base($"Unknown object type: {objectType}")
        {
            // Left blank intentionally
        }
    }
}