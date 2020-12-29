using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundLibrary", menuName = "Holostage/Background Library")]
public class BackgroundLibrary : ScriptableObject
{
    // [SerializeField]
    public List<BackgroundData> backgroundSet = new List<BackgroundData>();

}

[System.Serializable]
public class BackgroundData
{
    public Sprite m_image;
    public Color m_color;
}