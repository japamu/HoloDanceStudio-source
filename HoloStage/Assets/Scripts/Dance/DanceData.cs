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
    public List<Vector2> pointerPositions;
    public SavedFollowData()
    {

        pointerPositions = new List<Vector2>();
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
