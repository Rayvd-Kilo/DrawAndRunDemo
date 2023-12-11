using System;

using UnityEngine;

namespace DefaultNamespace.Data
{
    [Serializable]
    public struct TweenSequenceData
    {
        [field:SerializeField] public TweenData[] SequenceData { get; private set; }
    }
}