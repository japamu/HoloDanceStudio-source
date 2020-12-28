using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HotkeyLibrary", menuName = "Holostage/Hotkey Library")]
public class HotkeyLibrary : ScriptableObject
{
    // [SerializeField]
    public List<HotkeyData> Numbers = new List<HotkeyData>();

    // [SerializeField]
    public List<HotkeyData> Alpha = new List<HotkeyData>();
}


