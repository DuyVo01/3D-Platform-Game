using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyDemBones : MonoBehaviour
{
    public GameObject source;
    public GameObject myBoneRoot;

    Transform[] sourceBone;
    Transform[] myBones;

    private void Start()
    {
        if (source && myBoneRoot)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        sourceBone = source.GetComponentsInChildren<Transform>();
        myBones = myBoneRoot.GetComponentsInChildren<Transform>();

    }

    public void CopyBones()
    {
        for(int i = 0; i < sourceBone.Length; i++)
        {
            myBones[i].localPosition = sourceBone[i].localPosition;
            myBones[i].localRotation = sourceBone[i].localRotation;
            myBones[i].localScale = sourceBone[i].localScale;
        }
    }
}
