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

### Color Customizer

[Roadmap entry for customizing UI](https://hub.abinteractive.net/roadmap/inspect?job=191).

A mod to allow customizing the colors.

Currently implemented:

- [x] Nameplates
- [ ] Mic indicator
- [ ] Menus Background
- [ ] Menus lines
- [ ] Menus text
- [ ] Menus Icons
- [ ] User status indicator

Menu related patches probably won't be added before CVR rolls out their new menu system.
And maybe that'll make this mod not need to implement them if it's natively supported.

### Third person camera

[Roadmap entry for third person camera](https://hub.abinteractive.net/roadmap/inspect?job=198).

Allows you to toggle to a third person mode with a keybind, and zoom in and out with your mouse scrollwheel.

This is quite heavily WIP still, expect jank!

### KeyRebinder

There is a [Roadmap entry for rebindable keys](https://hub.abinteractive.net/roadmap/inspect?job=212), and a [feature request for nametag toggle keybind](https://forums.abinteractive.net/d/183-toggle-nametag-hotkey).

A mod to allow for some desktop mode keyboard rebinds, since the game does not include it as a feature.

Please note that the defaults are what I prefer, and for example have F1-8 rather than standard 1-8 for Gestures&States&Emotes. You can change the keys back easily by changing `F1` to `Alpha1` and so on in the config file.

#### Status

- [x] Mic standard & extra push to talk key
- [x] Prone
- [x] Crouch
- [x] Gesture controls
- [x] State controls
- [x] Emote controls
- [x] Flying controls
- [x] Toggling hud & nameplates
- [ ] Other controls

### SpeedMultiplier

[Feature request](https://forums.abinteractive.net/d/187-flight-speed-multiplier).

A mod that allows changing your flying speed.

## Building

Drag the required DLL's (listed in the .csproj files) into the Libs folder, open in VSCodium press `F1` and run the build tasks. Please note that it working does depend on you having the `dotnet` command available in your environment.

Alternatively you can try to open the folder in Visual Studio, but I cannot provide help for using that.

## Contributing

Contact me on Discord or send me git patches that you want merged.
