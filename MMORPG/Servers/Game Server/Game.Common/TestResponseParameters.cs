using Shared.Common.Communications;

namespace Shared.Game.Common
{
    public struct TestResponseParameters : IParameters
    {
        [Parameter(Code = 1)]
        public int MagicNumber { get; set; }
    }
}