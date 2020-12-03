using CommonTools.Log;
using Components.Common.Interfaces;
using Game.Application.GameObjects.Components;
using Game.Application.GameObjects.Components.Interfaces;
using InterestManagement;
using InterestManagement.Components;
using ServerApplication.Common.ApplicationBase;

namespace Game.Application.GameObjects
{
    public class GameObject : SceneObject
    {
        protected readonly IInterestAreaNotifier InterestAreaNotifier;

        protected GameObject(string name, TransformDetails transformDetails) 
            : base(GenerateId(), name, transformDetails)
        {
            InterestAreaNotifier = Components.AddComponent(new InterestAreaNotifier());
        }

        private static int GenerateId()
        {
            var idGenerator = ServerComponents.GetComponent<IIdGenerator>().AssertNotNull();
            var id = idGenerator.GenerateId();
            return id;
        }
    }
}