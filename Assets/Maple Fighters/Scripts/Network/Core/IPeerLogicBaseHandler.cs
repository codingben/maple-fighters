using System;

namespace Scripts.Services
{
    public interface IPeerLogicBaseHandler
    {
        void SetPeerLogic<T, TOperationCode, TEventCode>()
            where T : IPeerLogicBase, new()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible;

        T GetPeerLogic<T>()
            where T : IPeerLogicBase;
    }
}