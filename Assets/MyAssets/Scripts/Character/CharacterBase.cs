using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Create New Parts")]
public class CharacterBase : ScriptableObject
{
    [BoxGroup("Box1", ShowLabel = false)]
    [TitleGroup("Box1/CHARACTER BODY PARTS")]
    [SerializeField] 
    string _Name;

    [BoxGroup("Box1", ShowLabel = false)]
    [OnValueChanged("UpdateModel")]
    [SerializeField] 
    public SetBodyParts _characterSuit;

    [BoxGroup("Box1", ShowLabel = false)]
    [ReadOnly, InlineEditor(InlineEditorModes.LargePreview)]
    [SerializeField] 
    public GameObject _PreviewModel;
    
    private void UpdateModel()
    {
        if (_characterSuit != null) { this._PreviewModel = _characterSuit.gameObject; }
        if (_characterSuit == null) { this._PreviewModel = null; }
    }


    //[SerializeField] public GameObject _Face;
    //[SerializeField] public GameObject _Head;
    //[SerializeField] public GameObject _Neck;
    //[SerializeField] public GameObject _Body;
    //[SerializeField] public GameObject _Tail;

    //[SerializeField] public GameObject _LeftShoulder;
    //[SerializeField] public GameObject _RightShoulder;
    //[SerializeField] public GameObject _LeftHand;
    //[SerializeField] public GameObject _RightHand;

    //[SerializeField] public GameObject _LeftHip;
    //[SerializeField] public GameObject _RightHip;
    //[SerializeField] public GameObject _LeftKnee;
    //[SerializeField] public GameObject _RightKnee;
    //[SerializeField] public GameObject _LeftFoot;
    //[SerializeField] public GameObject _RightFoot;

    //public GameObject Face { get { return _Face; } }
    //public GameObject Head { get { return _Head; } }
    //public GameObject Neck { get { return _Neck; } }
    //public GameObject Body { get { return _Body; } }
    //public GameObject Tail { get { return _Tail; } }
    //public GameObject LeftShoulder { get { return _LeftShoulder; } }
    //public GameObject RightShoulder { get { return _RightShoulder; } }
    //public GameObject LeftHand { get { return _LeftHand; } }
    //public GameObject RightHand { get { return _RightHand; } }
    //public GameObject LeftHip { get { return _LeftHip; } }
    //public GameObject RightHip { get { return _RightHip; } }
    //public GameObject LeftKnee { get { return _LeftKnee; } }
    //public GameObject RightKnee { get { return _RightKnee; } }
    //public GameObject LeftFoot { get { return _LeftFoot; } }
    //public GameObject RightFoot { get { return _RightFoot; } }
}
