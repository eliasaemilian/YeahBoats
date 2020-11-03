using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Tutorial/TutorialInstructionSet")]
public class TutorialSet : ScriptableObject
{
    public List<TutorialInstruction> Instructions;
}


[Serializable]
public class TutorialInstruction
{
    public string Instruction;
    public Vector2 ScreenPos;
    public Vector2 PopUpSize;
    public ConfirmType Confirmation;
    public GameObject ConfirmationGO;
    public bool IsConfirmed;
}

public enum ConfirmType
{
    TapOnGO,
    DoubleTapOnGO,
    TapAnywhere

}