# Die In The Dungeon - Sandbox
This is a Sandbox / Cheat mod for the Steam Game [Die In The Dungeon: Origins](https://store.steampowered.com/app/2428770/Die_in_the_Dungeon_Origins/). It allows you to edit your deck, relics, enemies, skipping floors and more. Perfect for trying different builds, or just skip to the higher floors.

<p align="center">
  <img src="Img/showcase.png" />
</p>

# Installation

The game uses [BepInEx](https://github.com/BepInEx/BepInEx), a plugin framework for Unity games. You can download their latest release [here](https://github.com/BepInEx/BepInEx/releases).

Unpack BepInEx in your game folder (Usually something like `C:\Program Files (x86)\Steam\steamapps\common\Die in the Dungeon ORIGINS`), and run the game. This should create a plugin folder under `BepInEx\plugins`.

Download this mod from the [Releases Page](https://github.com/Leyren/DieInTheDungeonOriginsSandbox/releases) and put the DLLs in the plugins folder. Running the game again should now load the plugin and create a configuration file at `BepInEx\config\DieInTheDungeonsSandbox.cfg`.

**Note**: This mod requires [UniverseLib](https://github.com/sinai-dev/UniverseLib), which for convenience is included in the release as well, but you can also download it separately.

<p align="center">
  <img src="Img/install-1.png" />
</p>

# Configuration

|Name | Description |
|:---:|:---:|
|Unlimited-Deck|Removes the size limit (24) of your deck, allowing you to have more dices than that. The UI cannot display them though.|
|Hotkey|Hotkey with which to open the Sandbox|
|Show-Button|If `true`, displays an 'Open Sandbox' button in the game, as alternative to the hotkey|

# Ingame UI

To open the ingame UI, press the assigned hotkey or click the 'Open Sandbox' icon in the game:
<p align="center">
  <img src="Img/open.png" />
</p>

# Features

Here you find a list of features this mod provides, although most of them should be rather self-explanatory.

## Modify Stats
The `Stats` section allows you to modify different values of the game. Enter the amount you want to change the value by (can be negative) and press `Apply`. On the right side it shows you what the current value is.

<p align="center">
<img src="Img/Stats.png"/>
</p>

#### Max Dices In Hand

Increase/Decrease the limit of how many dices you can hold in your hand.
<p align="center">
<img src="Img/MaxDiceInHand1.png"/>
  <img src="Img/MaxDiceInHand2.png" height="64"/>
</p>

#### Energy
Increase/Decrease your amount of energy for this round.

<p align="center">
  <img src="Img/Energy1.png"/>
  <img src="Img/Energy2.png" height="32"/>
</p>

#### Player Stats

Increase/Decrease your player stats (note that block and health are limited by your max. health)
<p align="center">
  <img src="Img/PlayerStats.png" />
  <img src="Img/PlayerStats2.png" height="90"/>
</p>

#### Enemy Stats

Increase/Decrease the stats of the selected enemy (note that block and health are limited by your max. health)
<p align="center">
  <img src="Img/EnemyStats.png" />
  <img src="Img/EnemyStats2.png" height="90"/>
</p>

## Combat

The `Combat` section has combat & floor related features, to change your progression through the floors.
<p align="center">
<img src="Img/Combat.png"/>
</p>

#### Invulnerable & Force Kill (Toggle)

`Invulnerable` turns you invulnerable, all received damage is set to 0.

`Force Kill` makes every of your attacks deal 9999 damage, instantly killing every enemy.
<p align="center">
<img src="Img/Invulnerable.png" height="128"/>
<img src="Img/ForceKill.png" height="128"/>
</p>

#### Kill selected enemy / all enemies

As the name suggests, click to kill either the selected enemy, or all of them.

<p align="center">
<img src="Img/KillSelected.gif" height="256"/>
<img src="Img/KillAllEnemies.gif" height="256"/>
</p>

#### Skip to Floor

Let's you skip to specific floor (also back to previous one). Enter the target floor and press `Apply`. Shows the current maximum floor on the right side.

**Note**: After floor 20 (Endless mode), the max floor increases in steps of 5, so you can only skip 5 at a time.
<p align="center">
<img src="Img/SkipToFloor.png"/>
</p>

#### Always Win Events

Instantly win events, even without placing any dice. Just click on the event choice and it rolls a 999.
<p align="center">
<img src="Img/AlwaysWinEvents.png" height="64"/>
</p>

## Relics
<p align="center">
<img src="Img/Relics.png"/>
</p>

#### Open Relic Selection
Click to open the relic reward menu, but with **all** available relics. Use Arrow-Buttons on your keyboard to scroll, and click which one you want to obtain.

<p align="center">
<img src="Img/OpenRelicMenu.gif" height="256"/>
</p>

#### Get All Relics

Obtain all relics with one click.
<p align="center">
<img src="Img/GetAllRelics.png" height="64"/>
</p>

## Dices
<p align="center">
<img src="Img/Dices.png"/>
</p>

#### Open Upgrade Menu

Opens your backpack and lets you upgrade any dice, as often as you want.
<p align="center">
<img src="Img/OpenUpgradeMenu.gif" height="256"/>
</p>

#### Upgrade All Dices

Upgrade all dices to maximum level immediately.

<p align="center">
<img src="Img/UpgradeAllDices.gif" height="256"/>
</p>

#### Open Discard Menu

Open the backpack and discard any dice by clicking on it.

<p align="center">
<img src="Img/DiscardDices.gif" height="256"/>
</p>

#### Pick Dice from Backpack

Open the backpack and click a dice to put it in your hand (also bypasses hand size limit)

<p align="center">
<img src="Img/PickDice.gif" height="256"/>
</p>

#### Get Random Dice

Click to obtain a random dice (into your backpack). This can be **any** of the dices existing in the game.

<p align="center">
<img src="Img/GetRandomDice.gif" height="256"/>
</p>

#### Add Dice
Add a specific dice to your backpack. Select which dice you want and press the button

<p align="center">
<img src="Img/AddDice1.png"/><br>
<img src="Img/AddDice2.png" height="192"/>
</p>

#### Add Property
Add any property to your dice. Select which property you want and press the button. Opens the backpack and lets you click on the dice to which to add the property.

<p align="center">
<img src="Img/AddProperty1.png"/><br>
<img src="Img/AddProperty2.png" height="192"/>
</p>