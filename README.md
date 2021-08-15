# CVR-Mods<!-- omit in toc -->

[![Discord](https://img.shields.io/discord/865118898889031690?label=discord&logo=discord&style=flat)](https://discord.gg/2WR6rGVzht)
[![Latest release](https://img.shields.io/badge/dynamic/json.svg?label=release&url=https://git.ljoonal.xyz/api/v1/repos/ljoonal/CVR-Mods/releases&query=$[0].tag_name&style=flat&logo=gitea)](https://cvr.ljoonal.xyz/releases)
[![Works in 2021r160 EV2](https://img.shields.io/badge/CVR-2021r160%20EV2-brightgreen?style=flat&logo=steam)](https://store.steampowered.com/app/661130/ChilloutVR/)
[![GPL-3](https://img.shields.io/badge/license-GPL--3-black?style=flat&logo=open-source-initiative)](https://tldrlegal.com/license/gnu-general-public-license-v3-(gpl-3))
[![Lines of code](https://img.shields.io/tokei/lines/git.ljoonal.xyz/ljoonal/CVR-Mods?label=lines&style=flat&logo=C-Sharp)](.)

This repository contains some of my plugins for [ChilloutVR](https://store.steampowered.com/app/661130/ChilloutVR/) using [BepInEx](https://github.com/BepInEx/BepInEx).

To install, just follow the [BepInEx guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html) for Unity games for windows.
After that just drag'n'drop the DLL's into the `BepInEx/plugins` folder.
If you need help, I encourage you to join my [CVR modding discord corner](https://discord.gg/2WR6rGVzht).

I'd recommend you also get [sinai-dev's BepInExConfigManager](https://github.com/sinai-dev/BepInExConfigManager) so that you can edit your configs in game. You'll want the Mono version for CVR.

## Warning<!-- omit in toc -->

No warranty is provided for these plugins, and they're provided as-is.
Please have a look at the source code & build from source for maximal safety.
I also recommend mirroring this git repository if you want to make sure you always have access to the source code.

As far as I'm aware, the CVR devs allow modding to some extent now.
This doesn't mean though that they endorse the mods by me or that I'm in any way representing or affiliated with CVR or Alpha Blend Interactive.
It's up to you to make sure that you're following their rules & TOS, as I am only providing the code for my plugins and nothing else.

## Plugin list<!-- omit in toc -->

If you want the feature enough to get the plugin for it, you should probably also go upvote the feature request if there is one.

- [Skip intro](#skip-intro)
- [Color Customizer](#color-customizer)
- [Third person camera](#third-person-camera)
- [KeyRebinder](#keyrebinder)
- [Speed Multiplier](#speed-multiplier)
- [Player Rotater](#player-rotater)
- [Rotate It](#rotate-it)
- [M.A Logger](#ma-logger)
- [More Filters](#more-filters)
- [Hop Lib](#hop-lib)
- [Building](#building)
- [Why BepInEx and not MelonLoader](#why-bepinex-and-not-melonloader)
- [Contacting & contributing](#contacting--contributing)

### Skip intro

A simple plugin to skip the intro scene automatically.

### Color Customizer

[![Roadmap entry][RoadmapBadge]](https://hub.abinteractive.net/roadmap/inspect?job=191)

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

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)
[![Roadmap entry][RoadmapBadge]](https://hub.abinteractive.net/roadmap/inspect?job=198)

Allows you to toggle to a third person mode with a keybind, and zoom in and out with your mouse scroll wheel.

### KeyRebinder

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)
[![Roadmap entry][RoadmapBadge]](https://hub.abinteractive.net/roadmap/inspect?job=212)

A plugin to allow for some desktop mode keyboard rebinds, since the game does not include it as a feature.

Please note that the defaults are what I prefer, and for example have F1-8 rather than standard 1-8 for Gestures&States&Emotes. You can change the keys back easily by changing `F1` to `Alpha1` and so on in the config file.

The currently implemented keys are:

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

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)
[![Feature request][FeatureRequestBadge]](https://forums.abinteractive.net/d/187-flight-speed-multiplier)

A plugin that allows changing your flying speed.

### Player Rotater

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)

A plugin that allows rotating your own avatar whilst in flying mode.

### Rotate It

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)
[![Feature request][FeatureRequestBadge]](https://forums.abinteractive.net/d/97-object-rotation-in-desktop)

Ever wanted to rotate items in desktop mode? Well now you can with this plugin!
With configurable rotation speed and keybinds.

### M.A Logger

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)

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

### More Filters

[![Requires HopLib][HopLibBadge]](https://cvr.ljoonal.xyz/releases)

Adds more features related to content filtering.
Currently it only has an option to feature spawn audio.

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

## Mods under consideration<!-- omit in toc -->

- Make the menu follow the local player/camera.
- Add local metadata to other players. Nicknames/notes/profile picture replacements even perhaps?
- Nameplate overdraw, basically draw nameplates over everything else.
- Give the user more details about why a prop was filtered. Maybe avatars too, but lesser priority because the tags are already visible in the menus.
- Teleportation to another player or some other good way to be able to find & get to them in a huge world.
- Better Content filtering

## For developers<!-- omit in toc -->

### Building

Ensure that the required DLL's (listed in the `Directory.build.props` file and in the individual `.csproj` files) can be found from standard installation paths (check `Directory.build.props`).
Then use the `dotnet build` command to build.
A few examples include running `dotnet build HopLib/HopLib.csproj` to build HopLib in development mode or `dotnet build -c Release CVR-Plugins.csproj` to build all the plugins in release mode.

Alternatively you can try to open the folder in Visual Studio, but I cannot provide help for using that.
If you do want to improve the situation, do feel free to contribute!

### Why BepInEx and not MelonLoader

If you're asking: "MelonLoader is the tool used with VRC modding, so why not use it for CVR too?"

While it's a great tool, it requires .NET framework. Which is not available for linux.
I can actually build my plugins on Linux with BepInEx and run them without needing to do wineprefix trickery.
This is also why the build system is with VSC tasks instead of Visual Studio in this repository.

While it's technically possible to build MelonLoader mods on linux, most mods aren't done that way.
And running MelonLoader still requires .NET framework anyway, which BepInEx does not.

MelonLoader also does not have a few features that BepInEx does, and BepInEx just seems way more mature for modding Unity (non-IL2CPP) games.

### Contacting & contributing

Contact me [on Discord](https://discord.gg/2WR6rGVzht), [elsewhere](https://ljoonal.xyz/contact), and possibly send me git patches if you've already written any code that you'd like to get merged.

Also if anyone from the CVR team is reading this, do feel free to get in touch! I tried to email you a few times but stopped after never hearing a reply.

[HopLibBadge]: https://img.shields.io/badge/HopLib-Required-informational?style=flat
[FeatureRequestBadge]: https://img.shields.io/badge/upstream-feature%20request-pink?style=flat&logo=data:image/png;base64,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
[RoadmapBadge]: https://img.shields.io/badge/upstream-roadmap%20entry-pink?style=flat&logo=data:image/png;base64,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
