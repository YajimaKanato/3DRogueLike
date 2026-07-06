using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
    Animator _controller;
    AnimatorOverrideController _overrideController;
    readonly Dictionary<AnimationStateName, AnimationData> _animDict = new();

    public AnimationManager(Animator controller, AnimationDatas anims)
    {
        _controller = controller;
        _overrideController = new AnimatorOverrideController(_controller.runtimeAnimatorController);

        foreach (var anim in anims.Datas)
        {
            _animDict[anim.State] = anim;
        }
    }

    public void SetAnimationClip(AnimationStateName state)
    {
        if (!_animDict.TryGetValue(state, out var anim)) return;
        _overrideController[state.ToString()] = anim.Clip;
        _controller.SetFloat("AnimationSpeed", anim.Clip != null ? 1 : 1000);
    }
}
