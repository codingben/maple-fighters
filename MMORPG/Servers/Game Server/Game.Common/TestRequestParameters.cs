using Shared.Common.Communications;

namespace Shared.Game.Common
{
    public struct TestRequestParameters : IParameters
    {
        [Parameter(Code = 1)]
        public int MagicNumber { get; set; }
    }
}