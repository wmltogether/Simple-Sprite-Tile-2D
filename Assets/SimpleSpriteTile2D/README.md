#Simple Sprite Tile 2D

#### Unity Extension Type: 2D & Sprite Management

moogle's simple sprite tile 2D Unity Editor Extension

This extension is Requires Unity 5.x or higher. I didn't test it under lower unity versions.
But you can test it by yourself.

I decided to make a tile-based top-down game with Unity. But unfortunately, Unity's default sprite-tile system are too hard to use in Editor Mode. So ,This extension can auto-generate tile prefabs with one simple texture.You can quickly make tiles that automatically find neighbors around them, and Auto-Update sprites to fit tiles just like the RPG - Maker tile map.

## Features

* Auto snap tiles to grid and adjust child sprites for tile
* Esay Drag & Drop Editor Window for tile prefab creation
* Support JSON template for prefab batch creation.
* Add Custom material for 2D pixel Games.
* Auto update Z-order (sorting order) in scene
* Randomize tiles creation
* Lower Draw call

## HOW TO MAKE A TILE MAP IN Unity
It is very easy to create RPG  Maker - STYLE tiles for Unity.

1) Draw A bitmap like the RPG Maker tilemap.
2) Click [Tools/Simple Sprite Tiles 2D /Tile Prefab Creator] in Unity and drag texture on the editor panel
3) Toggle [Tile ] and Write the tile size and pixel per unit values. Then press [Edit Texture], make sure the sprites are correct.
4) If you want to add custom material , drag your material to the material field.
5) Create Prefab, Add Tile Panel, set the tile-layer name and drag tiles to your game scene.


## Advance Usage
This extension supports tiles auto-generation with json. You could write your own json files (see sample.json) and load it with your texture.

I wrote a sample code to show how to make a json (see : /Scprits/JsonBuilder/TestGenJSON.cs). It‘s a simple class.

Click [Tools/Simple Sprite Tiles 2D /Tile Prefab Creator] ,toggle [Load From JSON] and add your texture & json files.



## About Sorting Order

## Thanks
[Unity Engine](https://unity3d.com/)

[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## License
The MIT License (MIT)

Copyright (c) 2017 moogle / wmltogether

