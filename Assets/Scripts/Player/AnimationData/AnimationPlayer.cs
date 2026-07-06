using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

/// <summary>アニメーション再生を管理するクラス</summary>
public class AnimationPlayer : IDisposable
{
    readonly PlayableGraph _graph;
    readonly AnimatorControllerPlayable _controller;
    readonly AnimationMixerPlayable _mixerPlayable;
    readonly AnimationLayerMixerPlayable _layerPlayable;
    readonly Dictionary<StateName, AnimationClipPlayable> _clipPlayeblDict;
    AnimationClipPlayable _currentClip;
    AnimationClipPlayable _nextClip;
    float _fadeTimer;
    bool _isFading;

    public AnimationPlayer(Animator animator, StateDatas animationDatas)
    {
        _graph = PlayableGraph.Create();
        _controller = AnimatorControllerPlayable.Create(_graph, animator.runtimeAnimatorController);
        _mixerPlayable = AnimationMixerPlayable.Create(_graph, 2);
        _layerPlayable = AnimationLayerMixerPlayable.Create(_graph, 2);
        _clipPlayeblDict = new();

        Init(animator, animationDatas);
    }

    void Init(Animator animator, StateDatas animationDatas)
    {
        // 辞書作成
        if (animationDatas != null)
        {
            foreach (var data in animationDatas.Datas)
            {
                if (data != null)
                    _clipPlayeblDict[data.State] = AnimationClipPlayable.Create(_graph, data.Clip);
            }
        }

        // 出力ポート
        var output = AnimationPlayableOutput.Create(_graph, "Animation", animator);

        // 上書きアニメーション内のブレンド比を設定
        _mixerPlayable.SetInputWeight(0, 0);
        _mixerPlayable.SetInputWeight(1, 0);

        // 二つのAnimationLayerから出力されたアニメーションをブレンドする
        // 通常アニメーションをLayer0に登録
        _graph.Connect(_controller, 0, _layerPlayable, 0);
        // 上書きアニメーションをLayer1に登録
        _graph.Connect(_mixerPlayable, 0, _layerPlayable, 1);

        // 通常アニメーションを100%表示
        _layerPlayable.SetInputWeight(0, 1);
        // 上書きモーションを0%表示
        _layerPlayable.SetInputWeight(1, 0);

        // Layerによって最終ブレンドされたアニメーションを出力設定
        output.SetSourcePlayable(_layerPlayable);

        // グラフを再生
        _graph.Play();
    }

    public void Dispose()
    {
        _graph.Destroy();
    }

    /// <summary>
    /// アニメーションを再生するメソッド
    /// </summary>
    /// <param name="state">再生するアニメーションが割り当てられているステート</param>
    /// <returns>新たに再生するクリップ</returns>
    public AnimationClipPlayable Play(StateName state)
    {
        if (!_clipPlayeblDict.TryGetValue(state, out var clip)) return default;

        // 次に再生するクリップの設定
        _nextClip = clip;

        // アニメーションのミキサーにクリップを繋げる
        _graph.Connect(_nextClip, 0, _mixerPlayable, 1);

        // 再生比率を設定
        _mixerPlayable.SetInputWeight(1, 0);

        // アニメーションのフェード（ブレンド）設定
        _fadeTimer = 0;
        _isFading = true;

        return _nextClip;
    }

    /// <summary>
    /// フェード（ブレンド）を行うメソッド
    /// </summary>
    /// <param name="duration">遷移時間</param>
    public void Fade(float duration)
    {
        if (_isFading)
        {
            _fadeTimer += Time.deltaTime;

            // 遷移時間を計算
            var t = _fadeTimer / duration;

            // 遷移時間に応じて再生比率を設定
            _mixerPlayable.SetInputWeight(0, 1 - t);
            _mixerPlayable.SetInputWeight(1, t);

            // フェード（ブレンド）終了
            if (t >= 1)
            {
                FadeFinish();
            }
        }
    }

    /// <summary>
    /// フェード（ブレンド）が終わったときに呼ばれるメソッド
    /// </summary>
    void FadeFinish()
    {
        _isFading = false;

        // クリップの更新
        _currentClip = _nextClip;
        _nextClip = default;

        // クリップ接続の上書き
        _graph.Connect(_currentClip, 0, _mixerPlayable, 0);

        // 再生比率を設定
        _mixerPlayable.SetInputWeight(0, 1);

        // クリップ接続の削除
        _graph.Disconnect(_mixerPlayable, 1);

        // 再生比率を設定
        _mixerPlayable.SetInputWeight(1, 0);
    }
}
