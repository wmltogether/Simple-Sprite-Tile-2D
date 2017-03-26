#Simple Sprite Tile 2D

#### Extension Type: 2D & Sprite Management

moogle's simple sprite tile 2D Unity Editor Extension

本扩展需要Unity版本5.X或更高，更低版本的没做测试，不过你可以自己试试。

Unity自带的sprite系统对于做 2D Tile式的RPG来说真是太难用了。于是这个工具就是用来自动根据sprite 生成prefab。
生成的预设可以自动查找相邻的其他tile，构建tile并自动生成相应的边角样式，类似于RPG Maker的地图编辑。


## 演示图
![Extension Preview](https://github.com/wmltogether/Simple-Sprite-Tile-2D/blob/master/Assets/SimpleSpriteTile2D/preview.gif?raw=true)

## 功能

* 自动在场景中对齐图块
* 简单到飞起的prefab生成工具。
* 支持json+texture的批量生成系统。（需要自己写json配置文件）
* 支持自定义纹理
* 自动更新场景中的渲染层级
* 随机图块生成
* Draw call更低

## 在Unity中如何使用

1) 画一张这样的位图(详见 sample.png)。
2) 在unity中选择 [Tools/Simple Sprite Tiles 2D /Tile Prefab Creator]，把图片素材拖到面板上。
3) 选择 [Tile ] 填写tile size 和pixelperunit(推荐两个值填写相同) 然后点击 [Edit Texture]，确认预览中的tile正确。
4) 把自定义材质拖到面板的材质区域。（不填则为自定义材质）
5) 点击 Create Prefab, 在场景中添加 Tile Panel, 设定panel名称，最后把prefab拖入场景中设计你的场景吧。


## 高级模式
这个扩展脚本支持使用json自动批量构建prefab。 你可以根据 sample.json 的示例 做一个json来指定预设的生成。

在 /Scprits/JsonBuilder/TestGenJSON.cs 中有一个简单的生成示例，很简单的格式。

选择 [Tools/Simple Sprite Tiles 2D /Tile Prefab Creator] 中的 [Load From JSON] 添加图片素材和json文件，点击Create就可以批量操作。



## 关于Sorting Order

## 感谢
[Unity Engine](https://unity3d.com/)

[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## License
The MIT License (MIT)

Copyright (c) 2017 moogle / wmltogether

