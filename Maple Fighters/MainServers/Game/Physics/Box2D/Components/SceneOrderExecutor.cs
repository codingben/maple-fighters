using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Physics.Box2D.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace Physics.Box2D.Components
{
    public class SceneOrderExecutor : Component, ISceneOrderExecutor
    {
        private readonly ExternalCoroutinesExecutor preUpdate = new ExternalCoroutinesExecutor();
        private readonly ExternalCoroutinesExecutor update = new ExternalCoroutinesExecutor();
        private readonly ExternalCoroutinesExecutor postUpdate = new ExternalCoroutinesExecutor();

        protected override void OnAwake()
        {
            base.OnAwake();

            var fiber = Server.Components.GetComponent<IFiberStarter>().AssertNotNull();
            IScheduler fiberExecutor = fiber.GetFiberStarter();
            fiberExecutor.ScheduleOnInterval(Update, 0, 10);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            preUpdate.Dispose();
            update.Dispose();
            postUpdate.Dispose();
        }

        private void Update()
        {
            preUpdate.Update();
            update.Update();
            postUpdate.Update();
        }

        public ICoroutinesExecutor GetPreUpdateExecutor()
        {
            return preUpdate;
        }

        public ICoroutinesExecutor GetUpdateExecutor()
        {
            return update;
        }

        public ICoroutinesExecutor GetPostUpdateExecutor()
        {
            return postUpdate;
        }
    }
}