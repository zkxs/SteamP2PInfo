using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Globalization;

namespace SteamP2PInfo.Config
{
    public class OverlayConfig : INotifyPropertyChanged
    {
        public class PingColorRange
        {
            [JsonProperty("threshold")]
            public double Threshold { get; set; }
            [JsonProperty("color")]
            public string Color { get; set; }
        }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("show_steam_id")]
        public bool ShowSteamID { get; set; } = false;

        [JsonProperty("show_connection_quality")]
        public bool ShowConnectionQuality { get; set; } = true;

        [JsonProperty("hotkey")]
        public int Hotkey { get; set; } = 0;

        [JsonProperty("banner_format")]
        public string BannerFormat { get; set; } = "[{time:HH:mm:ss}] SteamP2PInfo - by tremwil";

        [JsonProperty("font")]
        public string Font { get; set; } = "Segoe UI, 20.25pt";

        [JsonProperty("x_offset")]
        public double XOffset { get; set; } = 0.025;

        [JsonProperty("y_offset")]
        public double YOffset { get; set; } = 0.025;

        [JsonProperty("anchor")]
        public OverlayAnchor Anchor { get; set; } = OverlayAnchor.TopRight;

        [JsonProperty("text_color")]
        public string TextColor { get; set; } = "#FFFFFFFF";

        [JsonProperty("stroke_color")]
        public string StrokeColor { get; set; } = "#FF000000";

        [JsonProperty("stroke_width")]
        public double StrokeWidth { get; set; } = 2.0;

        [JsonProperty("ping_colors", ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<PingColorRange> PingColors { get; set; } = new List<PingColorRange>()
        {
            new PingColorRange { Threshold = 0, Color = "#FF00BFFF" },
            new PingColorRange { Threshold = 50, Color = "#FF7CFC00" },
            new PingColorRange { Threshold = 100, Color = "#FFFFFF00" },
            new PingColorRange { Threshold = 200, Color = "#FFCD5C5C" }
        };

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
