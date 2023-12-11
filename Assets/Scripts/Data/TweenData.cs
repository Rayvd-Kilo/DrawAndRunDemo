using System;

using DrawAndRun.Enums;

using UnityEngine;

namespace DrawAndRun.Data
{
    [Serializable]
    public struct TweenData
    {
        [field:SerializeField] public TweenType TweenType { get; private set; }
        
        [field:SerializeField] public Vector3 TransformDirection { get; private set; }
        
        [field:SerializeField] public float TweenTime { get; private set; }
    }
}