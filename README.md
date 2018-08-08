# NavMeshWalker
NavMeshAgentの歩き方をCharacterControllerで制御します。

# サンプルの実行方法
ScenesフォルダーのSampleSceneをダブルクリックして開いて、Playします。

マウスでユニティちゃんを移動させたい先の床を指します。クリックは不要です。

# 組み込み方
プロジェクトへの組み込み方です。大雑把には以下のような流れです。

1. NavMeshをベイク
1. NavMeshWalkerプレハブのインポートと動きの調整
1. 自分用のキャラクターに差し替え


## 1. NavMeshをベイク
まずはNavMeshをベイクします。以下の辺りが参考になると思います。

- [Unityマニュアル. NavMesh ビルドコンポーネント
ナビメッシュの作成](https://docs.unity3d.com/jp/current/Manual/nav-BuildingNavMesh.html)
- [monolizm LLC. Unityはじめるよ
〜NavMesh基礎〜](http://monolizm.com/sab/pdf/%E7%AC%AC26%E5%9B%9E_%E3%83%97%E3%83%AC%E3%82%BC%E3%83%B3%E8%B3%87%E6%96%99(Unity%E3%81%AF%E3%81%98%E3%82%81%E3%82%8B%E3%82%88%EF%BD%9ENavMesh%E5%9F%BA%E7%A4%8E%EF%BD%9E).pdf)

SDユニティちゃんを使う場合、以下ぐらいの設定が丁度よさそうでした。

![SD Unitychan Setting](Images/img00.png)

ベイク結果は以下のような感じです。

![Bake result](Images/img01.png)

## 2. NavMeshWalkerプレハブのインポートと動きの調整
歩かせるゲームオブジェクトのプレハブを組み込みます。

- [Releasesページ]()から、**NavMeshWalker**パッケージをダウンロードします
- 自分のプロジェクトにインポートします

これで、マウスで指した場所にSDユニティちゃんが歩いていくようになります。

キャラクターが歩く速度や、旋回速度は、*NavMeshWalker*の以下の項目で調整できます。

![NavMeshWalker Setting](Images/img02.png)

### Nav Controller
|項目|内容|
|:-:|:--|
|Walk Speed|歩く速度。大きくすると速くなります。|
|Angular Speed|旋回する時の角速度です。大きくすると速くなります。|
|Turn Angle|目的地がこの角度よりずれている場合、移動せずにその場で方向転換します。|
|Turn Angular Speed|その場で方向転換する時の回転速度です。大きくすると速くなります。|
|Speed 2 Anim|移動速度とアニメーションの速度を調整します。大きくすると、移動速度に対して、アニメーションが速くなります。|
|Stop Speed|移動がこの速度より遅くなったら、アニメーションを立ちアニメにします。|

### Character Controller
|項目|内容|
|:-:|:--|
|Slope Limit|登れる斜面の角度です。|
|Step Offset|登れる段差の高さです。|
|Skin Width|この大きさ分、他のオブジェクトにめり込むことができます。スムーズにすれ違うための設定です。あまり大きくすると地面にめり込んだりします。|
|Min Move Distance|この距離以内の場合は移動させません。ぶるぶる震える現象を抑える設定です。|
|Center|当たり判定の中心です。Heightをいじったら合わせて調整します。|
|Radius|当たり半径です。NavMeshのRadiusより少し小さくしておくとスムーズに動きます。|
|Height|キャラクターの高さです。NavMeshと同じ高さにするか、少し低くしておくとよいです。|

必要に応じて調整してください。

## 3. キャラクターの表示用のオブジェクトを設定
キャラクターを差し替える手順です。



# ライセンス
本リポジトリーに含まれるSDユニティちゃんのモデルは、ユニティちゃんライセンスで提供されています。

<div><img src="http://unity-chan.com/images/imageLicenseLogo.png" alt="ユニティちゃんライセンス"><p>この作品は<a href="http://unity-chan.com/contents/license_jp/" target="_blank">ユニティちゃんライセンス条項</a>の元に提供されています</p></div>
