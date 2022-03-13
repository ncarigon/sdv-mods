<table align="center"><tr><td align="center" width="9999">

<!-- LOGO, TITLE, DESCRIPTION -->

# Immersive Fish Ponds

<br/>

<!-- TABLE OF CONTENTS -->
<details open="open" align="left">
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#features">Features</a></li>
    <li><a href="#compatibility">Compatbility</a></li>
    <li><a href="#installation">Installation</a></li>
    <li><a href="#special-thanks">Special Thanks</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

</td></tr></table>

## Features

This mod makes Fish Ponds useful and immersive by implementing the following three features:
1. Fish Ponds preserve the quality of fish placed inside. The quality of newly spawned fish will be inerited from a randomly chosen parent. Fishing from a pond always removes the lowest-quality fish first.
2. The quality of produced roe/ink is based on the average quality of the fish inside the pond. Each fish in the pond has a chance to add one roe/ink to the produce stack. However, this chance depends on the value of the fish (higher value fish have **lower** production chance). This means that cheap fish will produce tons of cheap roe, while more expensive fish will produce less but more valuable roe.
3. After 3 days, empty pond will spontaneously grow algae/seaweed. New algae/seaweed spawn every 2 days, and produce a random amount of algae/seaweed depending on population. Note that seaweed, green algae and white algae will all grow simultaneously in the same pond.
    
## Compatibility

This mod makes heavy use of Harmony to patch the behavior of Fish Ponds and adjacent objects. Any SMAPI mods that also patch Fish Pond behavior might be incompatible. Content Patcher packs that edit Fish Ponds or FishPondData are compatible, however.

- Compatible with (and meant to be paired with) [Walk Of Life](https://www.nexusmods.com/stardewvalley/mods/8111).
- Compatible with [Pond Painter](https://www.nexusmods.com/stardewvalley/mods/4703).
- **Not** compatible with [Anything Pond](https://www.nexusmods.com/stardewvalley/mods/4702) or [Quality Fish Ponds](https://www.nexusmods.com/stardewvalley/mods/11021).

Should be fully compatible with multiplayer.

## Installation

Install like any other mod, by extracting the content of the downloaded zip file to your mods folder and starting the game via SMAPI.

To update, first delete the old version and then install the new one. You can optionally keep your configs.json in case you have personalized settings.

To uninstall simply delete the mod from your mods folder. This mod is safe to uninstall at any point.

## Special Thanks

- [MouseyPounds](https://www.nexusmods.com/stardewvalley/users/3604264), author of Anything Ponds, for the idea of spontaneous algae growth.
- **ConcernedApe** for StardewValley.
- [JetBrains](https://jb.gg/OpenSource) for providing a free license to their tools.

<table>
  <tr>
    <td><img width="64" src="https://smapi.io/Content/images/pufferchick.png" alt="Pufferchick"></td>
    <td><img width="80" src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg" alt="JetBrains logo."></td>
  </tr>
</table>

## License

See [LICENSE](../LICENSE) for more information.