using System.Diagnostics;

namespace SteamP2PInfo
{
    internal class SteamPeerInfo
    {
        internal SteamPeerBase steamPeerBase;
        internal Stopwatch timeSinceLastPacketSent;
        internal bool disconnect;

        internal SteamPeerInfo(SteamPeerBase steamPeerBase)
        {
            this.steamPeerBase = steamPeerBase;
            timeSinceLastPacketSent = Stopwatch.StartNew();
            disconnect = false;
        }

        internal void OnPacketSent()
        {
            timeSinceLastPacketSent.Restart();
        }

        internal long LastSentPacketElapsedMilliseconds()
        {
            return timeSinceLastPacketSent.ElapsedMilliseconds;
        }
    }
}
