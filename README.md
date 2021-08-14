# CVR-Mods

[![Discord](https://img.shields.io/discord/865118898889031690?label=discord&logo=discord&style=flat)](https://discord.gg/2WR6rGVzht)
[![Latest release](https://img.shields.io/badge/dynamic/json.svg?label=release&url=https://git.ljoonal.xyz/api/v1/repos/ljoonal/CVR-Mods/releases&query=$[0].tag_name&style=flat&logo=gitea)](./releases/latest)
[![Works in 2021r160 EV2](https://img.shields.io/badge/CVR-2021r160%20EV2-brightgreen?style=flat&logo=steam)](https://store.steampowered.com/app/661130/ChilloutVR/)
[![GPL-3](https://img.shields.io/badge/license-GPL--3-black?style=flat)](https://tldrlegal.com/license/gnu-general-public-license-v3-(gpl-3))
![Lines of code](https://img.shields.io/tokei/lines/git.ljoonal.xyz/ljoonal/CVR-Mods?label=code%20lines&style=flat)

This repository contains some of my plugins for [ChilloutVR](https://store.steampowered.com/app/661130/ChilloutVR/) using [BepInEx](https://github.com/BepInEx/BepInEx).

To install, just follow the [BepInEx guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html) for Unity games for windows.
After that just drag'n'drop the DLL's into the `BepInEx/plugins` folder.
If you need help, I encourage you to join my [CVR modding discord corner](https://discord.gg/2WR6rGVzht).

I'd recommend you also get [sinai-dev's BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) so that you can edit your configs in game. You'll want the Mono version for CVR.

## Warning

No warranty is provided for these plugins, and they're provided as-is.
Please have a look at the source code & build from source for maximal safety.
I also recommend mirroring this git repository if you want to make sure you always have access to the source code.

As far as I'm aware, the CVR devs allow modding to some extent now.
This doesn't mean though that they endorse the mods by me or that I'm in any way representing or affiliated with CVR or Alpha Blend Interactive.
It's up to you to make sure that you're following their rules & TOS, as I am only providing the code for my plugins and nothing else.

## Why BepInEx and not MelonLoader

If you're asking: "MelonLoader is the tool used with VRC modding, so why not use it for CVR too?"

While it's a great tool, it requires .NET framework. Which is not available for linux.
I can actually build my plugins on Linux with BepInEx and run them without needing to do wineprefix trickery.
This is also why the build system is with VSC tasks instead of Visual Studio in this repository.

While it's technically possible to build MelonLoader mods on linux, most mods aren't done that way.
And running MelonLoader still requires .NET framework anyway, which BepInEx does not.

MelonLoader also does not have a few features that BepInEx does, and BepInEx just seems way more mature for modding Unity (non-IL2CPP) games.

## Skip intro

A simple plugin to skip the intro scene automatically.

## Plugin list

If you want the feature enough to get the plugin for it, you should probably also go upvote the feature request if there is one.

### Color Customizer

[Roadmap entry for customizing UI](https://hub.abinteractive.net/roadmap/inspect?job=191).

A plugin to allow customizing the colors.

Currently implemented:

- [x] Nameplates
- [ ] Mic indicator
- [ ] Menus Background
- [ ] Menus lines
- [ ] Menus text
- [ ] Menus Icons
- [ ] User status indicator

Menu related patches probably won't be added before CVR rolls out their new menu system.
And maybe that'll make this plugin not need to implement them if it's natively supported.

### Third person camera

[Link to roadmap entry for third person camera](https://hub.abinteractive.net/roadmap/inspect?job=198).

Allows you to toggle to a third person mode with a keybind, and zoom in and out with your mouse scrollwheel.

This is quite heavily WIP still, expect jank!

### KeyRebinder

[Link to thew roadmap entry for rebindable keys](https://hub.abinteractive.net/roadmap/inspect?job=212).

A plugin to allow for some desktop mode keyboard rebinds, since the game does not include it as a feature.

Please note that the defaults are what I prefer, and for example have F1-8 rather than standard 1-8 for Gestures&States&Emotes. You can change the keys back easily by changing `F1` to `Alpha1` and so on in the config file.

#### Status

- [x] Mic standard & extra push to talk key
- [x] Zoom
- [x] Prone
- [x] Crouch
- [x] Gesture controls
- [x] State controls
- [x] Emote controls
- [x] Flying controls
- [x] Toggling hud & nameplates
- [x] Reconnecting keybinds
- [ ] Other controls

### Speed Multiplier

[Link to Feature request](https://forums.abinteractive.net/d/187-flight-speed-multiplier).

A plugin that allows changing your flying speed.

### Player Rotater

A plugin that allows rotating your own avatar whilst in flying mode.

### Rotate It

[Link to Feature request](https://forums.abinteractive.net/d/97-object-rotation-in-desktop).

Ever wanted to rotate items in desktop mode? Well now you can with this plugin!
With configurable rotation speed and keybinds.

### M.A Logger

A plugin for logging things, to help figuring out the cause of Malicious Activity.

An example use case would be to get the last change before you crashed.
Since if a lot of people crashed at once, it's probably a fault of the most recently loaded thing.
Then you'll at least have the ID's of the avatar and the user who changed into it.

Current status:

- [x] Avatar changes
- [x] Prop spawns
- [x] Portal drops
- [ ] Avatar state changes
- [ ] Prop state changes

### Hop Lib

A library plugin for other plugins to use.
Mainly supposed to help making mods more wholesome easily, leading to less code repeat & complex code needing to be implemented only once.

You really should read the source code to see all the available ways to use it, but the basics are as follows:

```csharp
void Awake() {
  HopApi.PortalLoaded += delegate {
    // Do stuff that doesn't require any data with an anonymous delegate.
   };
   // If you want to access the data of the event,
  HopApi.InstanceJoined += OnInstancedJoined;
}

void OnInstancedJoined(object sender, InstanceEventArgs ev) {
  if (ev.GamemodeId != "SocialVR") return;
  // Do stuff
}
```

It's still under heavy development, I'd suggest others don't rely on it yet, as I'm trying to figure out what are the most useful & required abstractions.

## Mods under consideration

- Make the menu follow the local player/camera.
- Add local metadata to other players. Nicknames/notes/profile picture replacements even perhaps?
- Nameplate overdraw, basically draw nameplates over everything else.
- Give the user more details about why a prop was filtered. Maybe avatars too, but lesser priority because the tags are already visible in the menus.
- Teleportation to another player or some other good way to be able to find & get to them in a huge world.

## Building

Ensure that the required DLL's (listed in the `Directory.build.props` file and in the individual `.csproj` files) can be found from standard installation paths (check `Directory.build.props`).
Then use the `dotnet build` command to build.
A few examples include running `dotnet build HopLib/HopLib.csproj` to build HopLib in development mode or `dotnet build -c Release CVR-Plugins.csproj` to build all the plugins in release mode.

Alternatively you can try to open the folder in Visual Studio, but I cannot provide help for using that.
If you do want to improve the situation, do feel free to contribute!

## Contacting & contributing

Contact me [on Discord](https://discord.gg/2WR6rGVzht), [elsewhere](https://ljoonal.xyz/contact), and possibly send me git patches if you've already written any code that you'd like to get merged.

Also if anyone from the CVR team is reading this, do feel free to get in touch! I tried to email you a few times but stopped after never hearing a reply.
