using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationLibrary", menuName = "Holostage/Animation Library")]
public class AnimationLibrary : ScriptableObject
{
    public AnimationData[] eyeAnimation;
    public AnimationData[] mouthAnimation;
}


public enum AnimationType{
    Eye = 0,
    Mouth,
    Pose,
    Count
}
