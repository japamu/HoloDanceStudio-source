using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationLibrary", menuName = "Holostage/Animation Library")]
public class AnimationLibrary : ScriptableObject
{
    public AnimationData[] eyeAnimation;
    public AnimationData[] mouthAnimation;

    public int GetAnimationIndex( AnimationData p_animData )
    {
        switch( p_animData.m_animationLayer )
        {
            case (int)AnimationType.Eye:
                return Array.FindIndex<AnimationData>( eyeAnimation, a => a == p_animData );
            
            case (int)AnimationType.Mouth:
                return Array.FindIndex<AnimationData>( mouthAnimation, a => a == p_animData );
                
            default:
                return 0;
        }
    }
    public AnimationData GetAnimationData( int p_typeIndex, int p_animIndex )
    {
        switch( p_typeIndex )
        {
            case (int)AnimationType.Eye:
                return eyeAnimation[p_animIndex];
            
            case (int)AnimationType.Mouth:
                return mouthAnimation[p_animIndex];
                
            default:
                Debug.LogError("No such animation");
                return null;
        }
    }
}


public enum AnimationType{
    Eye = 0,
    Mouth,
    Pose,
    Count
}
