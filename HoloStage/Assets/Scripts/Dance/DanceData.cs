using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DanceData
{
    public List<SavedFollowData> FollowDatas;
    public List<SavedAnimationData> AnimationDatas;
    public DanceData()
    {
        FollowDatas = new List<SavedFollowData>();
        AnimationDatas = new List<SavedAnimationData>();
    }
}
[Serializable]
public class SavedTimeStamp
{
       public float timestamp;
}
[Serializable]
public class SavedFollowData: SavedTimeStamp
{
    public Vector2 pointerPosition;
    public SavedFollowData()
    {
        pointerPosition = Vector2.zero;
    }
}
[Serializable]
public class SavedAnimationData: SavedTimeStamp
{
    public int animType;
    public int animIndex;
    public SavedAnimationData()
    {
        animType = 0;
        animIndex = 0;
    }
}
