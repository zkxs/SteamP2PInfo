using System.Diagnostics;

namespace SteamP2PInfo
{
    internal class SteamPeerInfo
    {
        internal SteamPeerBase steamPeerBase;
        internal Stopwatch timeSinceLastSessionStarted;
        internal DisconnectReason disconnectReason;

        internal SteamPeerInfo(SteamPeerBase steamPeerBase)
        {
            this.steamPeerBase = steamPeerBase;
            timeSinceLastSessionStarted = Stopwatch.StartNew();
            disconnectReason = DisconnectReason.NONE;
        }

        internal void OnSessionStart()
        {
            timeSinceLastSessionStarted.Restart();
        }

        internal long LastPeerActivityMilliseconds()
        {
            return timeSinceLastSessionStarted.ElapsedMilliseconds;
        }

        internal enum DisconnectReason
        {
            NONE,
            AUTH_SESSION_ENDED,
            PEER_DISCONNECTED,
            TIMED_OUT,
        }
    }
}
