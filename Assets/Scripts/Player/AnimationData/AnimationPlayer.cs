using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationPlayer : IDisposable
{
    readonly PlayableGraph _graph;
    readonly AnimatorControllerPlayable _controller;
    readonly AnimationLayerMixerPlayable _layerPlayable;
    readonly Dictionary<AnimationStateName, AnimationClipPlayable> _clipPlayeblDict;

    public AnimationPlayer(Animator animator, AnimationDatas animationDatas)
    {
        _graph = PlayableGraph.Create();
        _controller = AnimatorControllerPlayable.Create(_graph, animator.runtimeAnimatorController);
        _layerPlayable = AnimationLayerMixerPlayable.Create(_graph, 2);
        _clipPlayeblDict = new();

        Init(animationDatas);
    }

    void Init(AnimationDatas animationDatas)
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

        // 通常アニメーションをLayer0に登録
        _graph.Connect(_controller, 0, _layerPlayable, 0);

        // 通常アニメーションを100%表示
        _layerPlayable.SetInputWeight(0, 1);
        // 上書きモーションを0%表示
        _layerPlayable.SetInputWeight(1, 0);

        // グラフを再生
        _graph.Play();
    }

    public void Dispose()
    {
        _graph.Destroy();
    }

    public void EnterPlay(AnimationStateName state, float wieght = 1)
    {
        if (!_clipPlayeblDict.TryGetValue(state, out var clip)) return;

    }

    public void UpdatePlay(float wieght = 1)
    {
        _layerPlayable.SetInputWeight(1, wieght);
    }
}
