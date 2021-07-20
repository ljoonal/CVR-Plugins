# CVR-Mods

Join my discord if you're interested in CVR modding: [https://discord.gg/2WR6rGVzht](https://discord.gg/2WR6rGVzht)

This repository contains some of my mods for [ChilloutVR](https://store.steampowered.com/app/661130/ChilloutVR/) using [BepInEx](https://github.com/BepInEx/BepInEx), and also some general BepInEx mods for any Unity games.

## Warning

No warranty is provided for these mods, and they're provided as-is.
Please have a look at the source code & build from source for maximal safety.

As far as I'm aware, the CVR devs allow modding to some extent now.
But if they ever do change their minds, it's up to you to make sure that you're following their TOS.

## Why BepInEx and not MelonLoader

If you're asking: "MelonLoader is the tool used with VRC modding, so why not use it for CVR too?"

While it's a great tool, it requires .NET framework. Which is not available for linux.
I can actually build my mods on Linux with BepInEx unlike MelonLoader.
This is also why the build system is with VSC tasks instead of Visual Studio in this repository.

MelonLoader also does not have a few features that BepInEx does, and BepInEx just seems way more mature for modding Unity (non-IL2CPP) games.

## Mod list

### KeyRebinder

[Roadmap entry](https://hub.abinteractive.net/roadmap/inspect?job=212).

A mod to allow for some desktop mode keyboard rebinds, since the game does not include it as a feature.

#### Status

- [x] Toggling nameplates
- [x] Mic standard & extra push to talk key
- [x] Prone
- [x] Crouch
- [x] Gesture controls
- [x] State controls
- [x] Emote controls
- [x] Flying controls
- [ ] Other controls

### SpeedMultiplier

[Feature request](https://forums.abinteractive.net/d/187-flight-speed-multiplier).

This mod does not work yet, haven't had enough time to properly implement it.

A mod that allows changing your flying speed.


## Building

Drag the required DLL's (listed in the .csproj files) into the Libs folder, open in VSCodium press `F1` and run the build tasks. Please note that it working does depend on you having the `dotnet` command available in your environment.

Alternatively you can try to open the folder in Visual Studio, but I cannot provide help for using that.

## Contributing

Contact me on Discord or send me git patches that you want merged.
