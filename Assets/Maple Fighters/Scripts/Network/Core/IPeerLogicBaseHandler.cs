using System;

namespace Scripts.Services
{
    public interface IPeerLogicBaseHandler
    {
        void SetPeerLogic<TPeerLogic, TOperationCode, TEventCode>()
            where TPeerLogic : IPeerLogicBase, new()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible;

        TPeerLogic GetPeerLogic<TPeerLogic>()
            where TPeerLogic : IPeerLogicBase;
    }
}