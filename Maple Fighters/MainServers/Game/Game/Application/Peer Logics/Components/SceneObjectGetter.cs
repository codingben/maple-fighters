using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using InterestManagement.Components.Interfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class SceneObjectGetter : Component, ISceneObjectGetter
    {
        private readonly ISceneObject sceneObject;

        public SceneObjectGetter(ISceneObject sceneObject)
        {
            this.sceneObject = sceneObject.AssertNotNull();
        }

        public ISceneObject GetSceneObject()
        {
            return sceneObject;
        }
    }
}