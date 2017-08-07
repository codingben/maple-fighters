using System;
using CommonCommunicationInterfaces;
using ExitGames.Client.Photon;
using Scripts.ScriptableObjects;
using UnityEngine;
using PhotonCommonImplementation;

namespace Scripts
{
    public abstract class ServiceBase : MonoBehaviour, IPhotonPeerListener
    {
        protected PhotonPeer PhotonPeer;
        private GameConfiguration gameConfiguration;

        private void Awake()
        {
            gameConfiguration = ScriptableObject.CreateInstance<GameConfiguration>();

            InitializeCommunication();
        }

        private void Update()
        {
            PhotonPeer?.Service();
        }

        private void InitializeCommunication()
        {
            PhotonPeer = new PhotonPeer(this, gameConfiguration.ConnectionProtocol);
        }

        private void OnApplicationQuit()
        {
            PhotonPeer?.Disconnect();
        }

        public void DebugReturn(DebugLevel level, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                {
                    Debug.LogWarning($"ServiceBase::OnStatusChanged -> Status Code: {statusCode}");
                    OnConnected();
                    break;
                }
                case StatusCode.Disconnect:
                case StatusCode.Exception:
                case StatusCode.ExceptionOnConnect:
                case StatusCode.ExceptionOnReceive:
                case StatusCode.TimeoutDisconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.DisconnectByServerLogic:
                {
                    Debug.LogWarning($"ServiceBase::OnStatusChanged -> Status Code: {statusCode}");
                    OnDisconnected();
                    break;
                }
                default:
                {
                    Debug.LogWarning($"ServiceBase::OnStatusChanged -> Status Code: {statusCode}");
                    break;
                }
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
        }

        public void OnEvent(EventData eventData)
        {

        }

        protected void SendOperationRequest<TOperationCode, TParameters>(TOperationCode operationCode, TParameters parameters, bool sendReliable)
            where TOperationCode : struct, IComparable, IConvertible, IFormattable
            where TParameters : struct, IParameters
        {
            // PhotonPeer?.OpCustom(Convert.ToByte(operationCode), parameters.T, sendReliable);
        }

        protected abstract void Connect();

        protected virtual void OnConnected()
        {
            // Left blank intentionally
        }

        protected virtual void OnDisconnected()
        {
            // Left blank intentionally
        }
    }

    public class GameService : ServiceBase
    {
        // Configurate it via scriptable object.

        protected override void Connect()
        {
            PhotonPeer?.Connect("127.0.0.1:5055", "GameApplication");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
        }
    }
}