using System;
using System.Collections.Generic;
using Steamworks;

using SteamP2PInfo.Config;

namespace SteamP2PInfo
{
    /// <summary>
    /// Repr
    /// </summary>
    abstract class SteamPeerBase : IDisposable
    {
        /// <summary>
        /// Steam ID of the peer.
        /// </summary>
        public CSteamID SteamID { get; protected set; }

        // The following field is probably useless now that Steam appears to have disabled the IsPlayingSharedGame request

        /// <summary>
        /// Main Steam ID of the peer, if playing on an alternate account. 
        /// </summary>
        public CSteamID MainSteamID { get; protected set; }

        /// <summary>
        /// Steam persona name of the peer.
        /// </summary>
        public virtual string Name { get { return SteamFriends.GetFriendPersonaName(SteamID); } }

        /// <summary>
        /// True if the peer is connected via the deprected api, ISteamNetworking.
        /// </summary>
        public abstract bool IsOldAPI { get; }

        /// <summary>
        /// Ping to peer in milliseconds.
        /// </summary>
        public abstract double Ping { get; }

        /// <summary>
        /// Subjective measure of connection quality to the remote peer where 0 = horrible and 1 = perfect.
        /// May not be very accurate if using the old API. When using the new API, should be directly related to packet loss.
        /// </summary>
        public abstract double ConnectionQuality { get; }

        private List<double> pings = new List<double>();
        private List<double> connectionQualities = new List<double>();

        /// <summary>
        /// ARGB hexadecimal color code used to fill the ping text.
        /// </summary>
        public string PingColor
        {
            get
            {
                OverlayConfig.PingColorRange range = new OverlayConfig.PingColorRange()
                {
                    Threshold = double.NegativeInfinity,
                    Color = GameConfig.Current.OverlayConfig.TextColor
                };

                foreach (OverlayConfig.PingColorRange r in GameConfig.Current.OverlayConfig.PingColors)
                {
                    if (r.Threshold <= Ping && r.Threshold > range.Threshold)
                        range = r;
                }

                return range.Color;
            }
        }

        protected SteamPeerBase(CSteamID steamID)
        {
            SteamID = steamID;
            //RequestMainSteamID();
        }

        /// <summary>
        /// Update peer info that may not be known at instance creation time.
        /// Should return true if the peer is still connected and false otherwise.
        /// </summary>
        public abstract bool UpdatePeerInfo();

        protected void RecordPeerInfo(double ping, double connectionQuality)
        {
            if (ping >= 0)
                pings.Add(ping);
            if (connectionQuality >= 0)
                connectionQualities.Add(connectionQuality);
        }

        public string GetConnectionSummary()
        {
            if (pings.Count == 0 && connectionQualities.Count == 0)
            {
                return "No ping/connection data available.";
            }

            // ping
            double minPing = double.PositiveInfinity;
            double maxPing = 0;
            double totalPing = 0;
            foreach (double ping in pings)
            {
                if (ping < minPing)
                {
                    minPing = ping;
                }
                if (ping > maxPing)
                {
                    maxPing = ping;
                }
                totalPing += ping;
            }
            double averagePing = totalPing / pings.Count;

            // connection quality
            double minConnectionQuality = double.PositiveInfinity;
            double maxConnectionQuality = 0;
            double totalConnectionQuality = 0;
            foreach (double connectionQuality in connectionQualities)
            {
                if (connectionQuality < minConnectionQuality)
                {
                    minConnectionQuality = connectionQuality;
                }
                if (connectionQuality > maxConnectionQuality)
                {
                    maxConnectionQuality = connectionQuality;
                }
                totalConnectionQuality += connectionQuality;
            }
            double averageConnectionQuality = totalConnectionQuality / connectionQualities.Count;

            return $"PING min={minPing:.0} avg={averagePing:.0} max={maxPing:.0}; QUALITY min={minConnectionQuality:.0} avg={averageConnectionQuality:.0} max={maxConnectionQuality:.0}";
        }

        public virtual void Dispose() { }
    }
}
