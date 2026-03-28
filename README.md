![Unity Version](https://img.shields.io/badge/dynamic/yaml?url=https://raw.githubusercontent.com/alexzorzella/Jelly-Drift-Second-Gear/main/ProjectSettings/ProjectVersion.txt&query=m_EditorVersion&label=Unity&color=222c37&logo=unity)
![Latest Release](https://img.shields.io/github/v/release/alexzorzella/Jelly-Drift-Second-Gear)
[![Unity Build](https://github.com/alexzorzella/Jelly-Drift/actions/workflows/unity-build.yml/badge.svg)](https://github.com/alexzorzella/Jelly-Drift-Second-Gear/actions/workflows/unity-build.yml)
![Issues](https://img.shields.io/github/issues/alexzorzella/Jelly-Drift-Second-Gear)<br>

## About

Jelly Drift: Second Gear is an open-source mod of [Jelly Drift](https://danidev.itch.io/jelly-drift).<br>
The game is heavily altered from the original:
1. **Updated graphics**: Second Gear has PXN style graphics. These have been sourced from various locations, but all are free to use
2. **Leaderboard**: Second Gear was created to be set up arcade style in a university laboratory. A local leaderboard has been added, which saves user times along with a message. Champions' times and messages are displayed in the main menu and after each race.
3. **PXN Controller Support**: The input system has been upgraded to support PXN steering wheels as well as the original keyboard.
4. **Gear System**: Second Gear introduces a gear system that affects the car's max speed and how much it drifts. All cars have a greater max speed but are more likely to drift at higher gears. Gears are supported both on keyboard and on steering wheel controls.

## Development

Launching unity on linux

```bash
GDK_SCALE=2 GDK_DPI_SCALE=0.5 $HOME/Unity/Hub/Editor/6000.0.58f2/Editor/Unity -projectPath .
```

## Credits

- Original game by [Dani Dev](https://www.youtube.com/@Danidev)
- Original music by [ContextSensitive](http://www.youtube.com/@ContextSensitive)
- Original car models by [Lexyc16](https://sketchfab.com/Lexyc16)
- Original terrain by [Sodiboo](https://github.com/sodiboo)
- Updated car models by [GGBotNet](https://ggbot.itch.io/)
- Decompiled by [Sodiboo](https://github.com/sodiboo)
- Refactored by [Alex](https://github.com/alexzorzella)
- Refactored by [0xott](https://github.com/AlexanderHott)
- Second Gear uses [LeanTween](https://github.com/dentedpixel/LeanTween)
