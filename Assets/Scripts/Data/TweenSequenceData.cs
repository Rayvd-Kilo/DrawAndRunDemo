using System;

using UnityEngine;

namespace DrawAndRun.Data
{
    [Serializable]
    public struct TweenSequenceData
    {
        [field:SerializeField] public TweenData[] SequenceData { get; private set; }
    }
}