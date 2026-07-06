using System;
using UnityEngine;

[Serializable]
public class AnimationData
{
    [SerializeField] AnimationStateName _state;
    [SerializeField] AnimationClip _clip;

    public AnimationStateName State => _state;
    public AnimationClip Clip => _clip;
}
