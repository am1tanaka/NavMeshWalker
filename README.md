# NavMeshWalker
NavMeshAgentの歩き方をCharacterControllerで制御します。

# サンプルの実行方法
ScenesフォルダーのSampleSceneをダブルクリックして開いて、Playします。

マウスでユニティちゃんを移動させたい先の床を指します。クリックは不要です。

# 組み込み方
プロジェクトへの組み込み方です。大雑把には以下のような流れです。

1. NavMeshをベイク
1. キャラクターを制御するための空のオブジェクトを作成してコンポーネントを設定
1. キャラクターの表示用のオブジェクトを設定

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

## 2. キャラクターを制御するための空のオブジェクトを作成してコンポーネントを設定




## 3. キャラクターの表示用のオブジェクトを設定


# ライセンス
本リポジトリーに含まれるSDユニティちゃんのモデルは、ユニティちゃんライセンスで提供されています。

<div><img src="http://unity-chan.com/images/imageLicenseLogo.png" alt="ユニティちゃんライセンス"><p>この作品は<a href="http://unity-chan.com/contents/license_jp/" target="_blank">ユニティちゃんライセンス条項</a>の元に提供されています</p></div>
