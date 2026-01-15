# MornUGUI

## 概要

UIパーツ管理・制御システム。モジュール設計で柔軟なカスタマイズが可能なライブラリ。

## 依存関係

| 種別 | 名前 |
|------|------|
| 外部パッケージ | Unity Input System |
| Mornライブラリ | MornLib, MornSound（オプション）, MornLocalize（オプション） |

## 使い方

### カスタムUIの作成

```csharp
public class MyButton : MornUGUIBase
{
    protected override List<MornUGUIModuleBase> CreateModules()
    {
        return new List<MornUGUIModuleBase>
        {
            new MornUGUIColorModule(),
            new MornUGUISoundModule(),
            new MornUGUIScaleModule(),
        };
    }
}
```

### 利用可能なモジュール

| モジュール | 機能 |
|-----------|------|
| MornUGUIColorModule | 色変化 |
| MornUGUISoundModule | サウンド再生 |
| MornUGUIScaleModule | スケール変化 |
| MornUGUIArrowModule | UIナビゲーション |

### 対応UI要素

- Button
- Slider
- Scrollbar
- ScrollRect
- Selector
- ShowHide（フェード/移動アニメーション対応）
