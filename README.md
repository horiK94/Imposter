# Imposter

インポスターは3Dポリゴンから複数枚の画像を作成し、見る角度に応じて画像を差し替えることでバッチ数を減らすことができます。
https://www.youtube.com/watch?v=SGqOL7WkDn8

## 作成方法

下記の方法で画像を作成できます。  
2つの方法があります。

・Atlas画像を作成する方法

1. `ImposterAtlasGenerator`をAddComponentする
2. パラメータを設定後、`Create Atlas`でAtlas画像を作成

・SpriteAtlasを作成する方法

1. `ImposterSpriteAtlasGenerator`をAddComponentする
2. パラメータを設定後、`Create Sprite Atlas`でAtlas画像を作成

## 表示方法

作成方法に応じて、表示方法が異なります。

・Atlas画像を作成した場合

1. `Mesh Rendere`をAddComponentする。`materials`に`Unlit/ImposterShader`をshaderとして設定したmaterialを設定する
2. `ImposterAtlasBillboard`をAddComponentする。作成時と同じパラメータを設定する
3. `Unlit/ImposterShader`の`TextureP`に生成された画像をアタッチ
4. `ImposterAtlasBillboard`に`Mesh Render`をアタッチ

・SpriteAtlasを作成する方法

1. `Sprite Rendere`と`ImposterSpriteAtlasBillboard`をAddComponentする。
2. 作成時と同じパラメータを設定する
3. `ImposterSpriteAtlasBillboard`の`Sprite Atlas`に生成されたSpriteAtlasをアタッチ
