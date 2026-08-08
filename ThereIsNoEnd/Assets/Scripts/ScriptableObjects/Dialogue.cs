using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("Character info")]
    public string characterName;
    public Sprite characterSprite;

    [Header("Dialogue info")]
    [TextArea]
    public string[] dialogue;
}
