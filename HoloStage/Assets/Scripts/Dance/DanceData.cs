using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DanceData
{
    public float TotalDuration;
    public List<SavedPointerData> PointerDatas;
    public List<SavedAnimationData> AnimationDatas;
    public DanceData()
    {
        TotalDuration = 0;
        PointerDatas = new List<SavedPointerData>();
        AnimationDatas = new List<SavedAnimationData>();
    }
}
[Serializable]
public class SavedTimeStamp
{
       public float timestamp;
}
[Serializable]
public class SavedPointerData: SavedTimeStamp
{
    public List<Vector2> pointerPositions;
    public SavedPointerData()
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
