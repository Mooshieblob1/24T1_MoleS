using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBaseSet : MonoBehaviour
{
    [BoxGroup("Box1", ShowLabel = false)]
    [BoxGroup("Box1/Box2", ShowLabel = false)]
    [TitleGroup("Box1/Box2/Character Setup")]
    [OnValueChanged("UpdateModel")]
    [OnValueChanged("LoadModel")]
    [SerializeField] 
    public CharacterBase 
    _BaseSet, 
    _CostumeSet;

    private void UpdateModel() { 
    if (_BaseSet != null || _CostumeSet != null) { this._PreviewModel = this.gameObject; } 
    if (_BaseSet == null && _CostumeSet == null) { this._PreviewModel = null; } }

    [FoldoutGroup("Box1/Body Point", expanded: false)]
    [SerializeField] 
    Transform 
    _FacePoint,
    _HeadPoint,
    _NeckPoint,
    _BodyPoint,
    _TailPoint,
    _LeftShoulderPoint,
    _RightShoulderPoint,
    _LeftHandPoint,
    _RightHandPoint,
    _LeftHipPoint,
    _RightHipPoint,
    _LeftKneePoint,
    _RightKneePoint,
    _LeftFootPoint,
    _RightFootPoint;

    [FoldoutGroup("Box1/Instantiate Check", expanded: false)]
    [SerializeField]
    [TitleGroup("Box1/Instantiate Check/Base Parts")]
    [ReadOnly]
    private Transform 
    _BaseFace,
    _BaseHead,
    _BaseNeck,
    _BaseBody,
    _BaseTail,
    _BaseLeftShoulder,
    _BaseRightShoulder,
    _BaseLeftHand,
    _BaseRightHand,
    _BaseLeftHip,
    _BaseRightHip,
    _BaseLeftKnee,
    _BaseRightKnee,
    _BaseLeftFoot,
    _BaseRightFoot;

    [FoldoutGroup("Box1/Instantiate Check")]
    [SerializeField]
    [TitleGroup("Box1/Instantiate Check/Costume Parts")]
    [ReadOnly]
    private Transform
    _CostumeFace,
    _CostumeHead,
    _CostumeNeck,
    _CostumeBody,
    _CostumeTail,
    _CostumeLeftShoulder,
    _CostumeRightShoulder,
    _CostumeLeftHand,
    _CostumeRightHand,
    _CostumeLeftHip,
    _CostumeRightHip,
    _CostumeLeftKnee,
    _CostumeRightKnee,
    _CostumeLeftFoot,
    _CostumeRightFoot;


    [ReadOnly, InlineEditor(InlineEditorModes.LargePreview)]
    [BoxGroup("Box1")]
    [HideLabel]
    public GameObject _PreviewModel;

    void Start()
    {
        if (_BaseSet != null) { LoadModel(); }
    }

    void LoadModel()
    {
        if (Application.isEditor)
        {
            if (_BaseFace != null) DestroyImmediate(_BaseFace.gameObject);
            if (_BaseHead != null) DestroyImmediate(_BaseHead.gameObject);
            if (_BaseNeck != null) DestroyImmediate(_BaseNeck.gameObject);
            if (_BaseBody != null) DestroyImmediate(_BaseBody.gameObject);
            if (_BaseTail != null) DestroyImmediate(_BaseTail.gameObject);
            if (_BaseLeftShoulder != null) DestroyImmediate(_BaseLeftShoulder.gameObject);
            if (_BaseRightShoulder != null) DestroyImmediate(_BaseRightShoulder.gameObject);
            if (_BaseLeftHand != null) DestroyImmediate(_BaseLeftHand.gameObject);
            if (_BaseRightHand != null) DestroyImmediate(_BaseRightHand.gameObject);
            if (_BaseLeftHip != null) DestroyImmediate(_BaseLeftHip.gameObject);
            if (_BaseRightHip != null) DestroyImmediate(_BaseRightHip.gameObject);
            if (_BaseLeftKnee != null) DestroyImmediate(_BaseLeftKnee.gameObject);
            if (_BaseRightKnee != null) DestroyImmediate(_BaseRightKnee.gameObject);
            if (_BaseLeftFoot != null) DestroyImmediate(_BaseLeftFoot.gameObject);
            if (_BaseRightFoot != null) DestroyImmediate(_BaseRightFoot.gameObject);
        }
        else
        {
            if (_BaseFace != null) Destroy(_BaseFace.gameObject);
            if (_BaseHead != null) Destroy(_BaseHead.gameObject);
            if (_BaseNeck != null) Destroy(_BaseNeck.gameObject);
            if (_BaseBody != null) Destroy(_BaseBody.gameObject);
            if (_BaseTail != null) Destroy(_BaseTail.gameObject);
            if (_BaseLeftShoulder != null) Destroy(_BaseLeftShoulder.gameObject);
            if (_BaseRightShoulder != null) Destroy(_BaseRightShoulder.gameObject);
            if (_BaseLeftHand != null) Destroy(_BaseLeftHand.gameObject);
            if (_BaseRightHand != null) Destroy(_BaseRightHand.gameObject);
            if (_BaseLeftHip != null) Destroy(_BaseLeftHip.gameObject);
            if (_BaseRightHip != null) Destroy(_BaseRightHip.gameObject);
            if (_BaseLeftKnee != null) Destroy(_BaseLeftKnee.gameObject);
            if (_BaseRightKnee != null) Destroy(_BaseRightKnee.gameObject);
            if (_BaseLeftFoot != null) Destroy(_BaseLeftFoot.gameObject);
            if (_BaseRightFoot != null) Destroy(_BaseRightFoot.gameObject);
        }

        if (_BaseSet != null)
        {
            if (_FacePoint != null && _BaseSet.Face != null) { SetPreview(ref _BaseFace, _BaseSet.Face, _FacePoint); }
            if (_HeadPoint != null && _BaseSet.Head != null) { SetPreview(ref _BaseHead, _BaseSet.Head, _HeadPoint); }
            if (_NeckPoint != null && _BaseSet.Neck != null) { SetPreview(ref _BaseNeck, _BaseSet.Neck, _NeckPoint); }
            if (_BodyPoint != null && _BaseSet.Body != null) { SetPreview(ref _BaseBody, _BaseSet.Body, _BodyPoint); }
            if (_TailPoint != null && _BaseSet.Tail != null) { SetPreview(ref _BaseTail, _BaseSet.Tail, _TailPoint); }
            if (_LeftShoulderPoint != null && _BaseSet.LeftShoulder != null) { SetPreview(ref _BaseLeftShoulder, _BaseSet.LeftShoulder, _LeftShoulderPoint); }
            if (_RightShoulderPoint != null && _BaseSet.RightShoulder != null) { SetPreview(ref _BaseRightShoulder, _BaseSet.RightShoulder, _RightShoulderPoint); }
            if (_LeftHandPoint != null && _BaseSet.LeftHand != null) { SetPreview(ref _BaseLeftHand, _BaseSet.LeftHand, _LeftHandPoint); }
            if (_RightHandPoint != null && _BaseSet.RightHand != null) { SetPreview(ref _BaseRightHand, _BaseSet.RightHand, _RightHandPoint); }
            if (_LeftHipPoint != null && _BaseSet.LeftHip != null) { SetPreview(ref _BaseLeftHip, _BaseSet.LeftHip, _LeftHipPoint); }
            if (_RightHipPoint != null && _BaseSet.RightHip != null) { SetPreview(ref _BaseRightHip, _BaseSet.RightHip, _RightHipPoint); }
            if (_LeftKneePoint != null && _BaseSet.LeftKnee != null) { SetPreview(ref _BaseLeftKnee, _BaseSet.LeftKnee, _LeftKneePoint); }
            if (_RightKneePoint != null && _BaseSet.RightKnee != null) { SetPreview(ref _BaseRightKnee, _BaseSet.RightKnee, _RightKneePoint); }
            if (_LeftFootPoint != null && _BaseSet._LeftFoot != null) { SetPreview(ref _BaseLeftFoot, _BaseSet.LeftFoot, _LeftFootPoint); }
            if (_RightFootPoint != null && _BaseSet._RightFoot != null) { SetPreview(ref _BaseRightFoot, _BaseSet.RightFoot, _RightFootPoint); }
        }
    }

    void SetPreview(ref Transform instancePreview, GameObject characterBasePart, Transform transformPoint)
    {
        instancePreview = Instantiate(characterBasePart).transform;
        instancePreview.transform.SetParent(transformPoint);
        instancePreview.transform.localPosition = Vector3.zero;
        instancePreview.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
