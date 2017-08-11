#if UNITY_WEBGL

namespace ExitGames.Client.Photon
{
    using UnityEngine;


    public class PingHttp : PhotonPing
    {
        private WWW webRequest;

        public override bool StartPing(string address)
        {
            address = "https://" + address + "/photon/m/?ping&r=" + UnityEngine.Random.Range(0, 10000);
            Debug.Log("StartPing: " + address);
            this.webRequest = new WWW(address);
            return true;
        }

        public override bool Done()
        {
            if (this.webRequest.isDone)
            {
                Successful = true;
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            this.webRequest.Dispose();
        }
    }
}
#endif
