# SteamP2PInfo - zkxs Edition

This is a fork of [tremwil's original SteamP2PInfo](https://github.com/tremwil/SteamP2PInfo)

Simple C# application displaying active Steam P2P connection info, namely SteamID / ping / connection quality. This was specifically made with Armored Core 6 in mind, but it should work for any game that groups peers in the same session using SteamMatchmaking lobbies.

**It also supports adding peers to the Steam recent players list, if the game does not support this.**
## [Releases](https://github.com/zkxs/SteamP2PInfo/releases/)

![](gui.PNG)

# How to Use
Download the lastest release from the Releases tab and extract the ZIP file in any folder on your computer. Once the game is running, start `SteamP2PInfo.exe` and click on "Attach Game". Select the appropriate game window in the dialog. If this game has never been opened before, you will be prompted to enter the game's **Steam AppId**. This can be queried on websites like [steamdb](https://steamdb.info/). The Steam console will then open. **You must enter the following command in the console for the tool to work:** `log_ipc "BeginAuthSession,EndAuthSession"`. The program should now be ready! You can then go in the "Config" tab to customize game-specific settings. 

# Differences from Original SteamP2PInfo

- Updated to build in Visual Studio 2022 (credit to [@AronDavis](https://github.com/AronDavis) for [fixing one of the build issues](https://github.com/tremwil/SteamP2PInfo/pull/34))
- Various fixes to work consistently in Armored Core 6
- Rewrote to use a different `log_ipc` filter. The original SteamP2P info would fail for some games if you were the lobby creator, which in 1v1 AC6 matches is 50% of the time.
- Removed the overlay feature. Even with the feature disabled via config, the SteamP2PInfo overlay was somehow causing application instability and crashing for me when I'd open the Steam in-game overlay. I recommend putting the SteamP2PInfo on your second monitor. If you lack a second monitor, I apologize for impacting your experience. (Note that for backwards compatibility I've left the overlay configuration intact in the JSON file, but it's unused.)
- Added additional entries to SteamP2PInfo's log file, including some simple statistics of the overall connection performance when a peer disconnects. If you're interested in being able to look back at a record of your games to see what the connection was like, consider turning on the logging config option.

I have only tested this on Armored Core 6: while it's likely it will work on other games as well, it depends heavily on exactly how the game uses the Steamworks API.

# FAQ
### Why does it require administrator privileges?
While the `SteamNetworkingMessages` API provides detailed connection information, the old API `SteamNetworking` does not do this. Hence in this the pings are computed by monitoring STUN packets that are sent to and recieved from the players' IPs. To capture these packets I use Event Tracing for Windows (ETW), which requires administrator privileges for "kernel" events like networking.

### Why do I have to use the Steam console / IPC logging? Isn't there an cleaner way to monitor lobbies?
Sadly, this is the only way I found to reliably detect lobby joining and creation when running two processes using the same game ID. I cannot use Steam callbacks, because if the game "consumes" them my tool's callbacks will not be called, and vice versa. I also do not want to rely on reading game memory or injecting code into the game in order to support anti-cheat protected games. In the future, I plan to move from IPC log file parsing to an internal `steam.exe` hook to make peer detection 100% reliable. Since some VAC games might not like this, an option will be available to use the legacy system if needed. This new method will take quite a bit of work to implement, however.

### How is the "Connection Quality" computed?
When a peer is connected using the `SteamNetworkingMessages` API, this roughly corresponds to `1 - packet_loss`. When connected using the deprecated API `SteamNetworking`, the value is computed using the formula `1 / (0.01 * jitter + 1)`, where `jitter` is the standard deviation of the last 10 ping values. This is done instead of showing `jitter` directly so that the value is on the same scale as the `SteamNetworkingMessages` one.

### Why does the program close with the game?
Since the tool is loaded with the game's Steam AppId, letting the program run after the game closes would make Steam think the game is still running. Calling `SteamAPI_Shutdown` does not seem to fix the problem, so we have to close the process.

### I found a bug / I have something to say about the tool
Feel free to open an issue on this Github repo.
