# NavMeshWalker
NavMeshAgentの歩き方をCharacterControllerで制御します。

# 前提
- Unity2018.1以降で動作確認

# ライセンス
## SDユニティちゃん
本リポジトリーに含まれるSDユニティちゃんのモデルは、ユニティちゃんライセンスで提供されています。

<div><img src="http://unity-chan.com/images/imageLicenseLogo.png" alt="ユニティちゃんライセンス"><p>この作品は<a href="http://unity-chan.com/contents/license_jp/" target="_blank">ユニティちゃんライセンス条項</a>の元に提供されています</p></div>

## その他のスクリプトやデータについて
[MIT License](LICENSE)

Copyright (c) 2018 Yu Tanaka

# サンプルの実行方法
ScenesフォルダーのSampleSceneをダブルクリックして開いて、Playします。

マウスでユニティちゃんを移動させたい先の床を指します。クリックは不要です。

# 組み込み方
プロジェクトへの組み込み方です。おおよそ、以下のような流れです。

1. パッケージのダウンロードと組み込み
1. NavMeshをベイク
1. NavMeshWalkerの設定
1. 指定の場所を設定するスクリプト
1. キャラクターを差し替える
1. アニメーションの設定

組み込みたいプロジェクトを開いた状態で作業をします。

## 1. パッケージのダウンロードと組み込み
歩かせるゲームオブジェクトのプレハブを組み込みます。

- [Releasesページ]()から、**NavMeshWalker.unitypackage**をダウンロードします
- プロジェクトにインポートします

## 2. NavMeshをベイク
移動ルートを作成するためのNavMeshをベイクします。手順は以下の辺りが参考になると思います。

- [Unityマニュアル. NavMesh ビルドコンポーネント
ナビメッシュの作成](https://docs.unity3d.com/jp/current/Manual/nav-BuildingNavMesh.html)
- <a href="http://monolizm.com/sab/pdf/%E7%AC%AC26%E5%9B%9E_%E3%83%97%E3%83%AC%E3%82%BC%E3%83%B3%E8%B3%87%E6%96%99(Unity%E3%81%AF%E3%81%98%E3%82%81%E3%82%8B%E3%82%88%EF%BD%9ENavMesh%E5%9F%BA%E7%A4%8E%EF%BD%9E).pdf">monolizm LLC. Unityはじめるよ
〜NavMesh基礎〜</a>

SDユニティちゃんを使う場合、以下ぐらいの設定が丁度よさそうでした。

![SD Unitychan Setting](Images/img00.png)

ベイク結果は以下のような感じです。

![Bake result](Images/img01.png)

## 3. NavMeshWalkerの設定
歩かせるゲームオブジェクトのプレハブ**NavMeshWalker**をシーンに配置します。

- *Project*ビューで、*NavMeshWalker/Prefabs*フォルダーを開きます
- *NavMeshWalker*プレハブを、シーンかヒエラルキーにドラッグ&ドロップして、配置します

Playを開始すると、マウスで指した場所に、SDユニティちゃんが歩いていくようになります。

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

## 4. 指定の場所を設定するスクリプト
デフォルトの*NavMeshWalker*には、マウスで指した場所に移動を設定するデモスクリプトが設定されています。

![Mouse Click Target](Images/img13.png)

実際のゲームには不要と思いますので、これは*Remove*して構いません。

スクリプトから移動先を設定する方法です。

- *NavMeshWalker*に操作用のスクリプトをアタッチします
- アタッチしたスクリプトに、以下のような*NavController*のインスタンスを取得するコードを追加します

```cs
NavController navCon;

private void Awake()
{
    navCon = GetComponent<NavController>();
}
```

- 座標を設定するのは、`navCon`の`SetDestination()`メソッドに、目指す座標を渡します。

```cs
navCon.SetDestination(target);
```

これで、スクリプトから移動先を制御できます。

## 5. キャラクターを差し替える
キャラクターを差し替える手順です。デフォルトの*NavMeshWalker*オブジェクトは、子供にSDユニティちゃんのプレハブを持っています。

![player object](Images/img05.png)

- 差し替えたいキャラクターを同じように子供にして、*SD_unitychan_humanoid*は削除します

![change model](Images/img06.png)

- モデルの座標を`0, 0, 0`にして、奥を向くように回転を調整します

![Set Transform](Images/img07.png)

- *Animator*コンポーネントをアタッチして、*Controller*にアニメーターデータを設定します。サンプルとして、SDユニティちゃんのアニメデータを利用した簡単なものを**Animations/NavMeshWalker**に用意してあります

![Set Animator](Images/img08.png)

スクリプトで移動を制御する場合、`Apply Root Motion`が悪さをする場合があるので、チェックを外しておくとよいでしょう。問題が起きたらチェックを戻してください。

以上で差し替え完了です。

![Changed Chr](Images/img09.png)

# 6. アニメーションの設定
サンプルのアニメーターは以下のようなシンプルなものです。

![Animator](Images/img04.png)

このうち、サンプルでは**Standing**と**Walking**しか使っていません。

## サンプルのAnimatorを利用する場合
Animatorはそのまま利用して、Animationを他のものに差し替えるには、Animatorの各状態に設定されているAnimationを変更してください。

- Animatorウィンドウで差し替えたい状態を選択します

![Select Status](Images/img10.png)

- *Inspector*の*Motino*欄に、変更したいアニメーションをドラッグ&ドロップします

![Set Animation](Images/img11.png)

これでアニメーションのみを差し替えることができます。

## 独自でAnimatorを作る場合
Animatorを変更したい場合は、`Float`型の`Speed`パラメーターを定義してください。

![Speed](Images/img12.png)

このパラメーターに、スクリプトからキャラクターの移動速度を渡すので、この値を使って*Standing*や*Walking*、必要なら*Running*を切り替えるようにしてください。

# 参考・関連URL
- [Unityマニュアル. ナビゲーションと経路探索](https://docs.unity3d.com/jp/current/Manual/Navigation.html)
- <a href="http://monolizm.com/sab/pdf/%E7%AC%AC26%E5%9B%9E_%E3%83%97%E3%83%AC%E3%82%BC%E3%83%B3%E8%B3%87%E6%96%99(Unity%E3%81%AF%E3%81%98%E3%82%81%E3%82%8B%E3%82%88%EF%BD%9ENavMesh%E5%9F%BA%E7%A4%8E%EF%BD%9E).pdf">monolizm LLC. Unityはじめるよ
〜NavMesh基礎〜</a>
- [UNITY-CHAN! OFFICIAL WEBSITE](http://unity-chan.com/)
