using CommonTools.Log;
using ComponentModel.Common;

namespace ComponentModel.Tests
{
    public class OwnerTestComponent : Component<ITestEntity>, ITestComponent
    {
        private readonly ILogger logger;

        public OwnerTestComponent(ILogger logger)
        {
            this.logger = logger;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            logger.Log("OnAwake()");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            logger.Log("OnDestroy()");
        }
    }
}