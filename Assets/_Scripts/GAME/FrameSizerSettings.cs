using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrameSizerSettings", menuName = "")]
public class FrameSizerSettings : ScriptableObject
{
    [Serializable]
    public struct AmountMinAir
    {
        public float onePlayerAir;
        public float twoPlayerAir;
        public float treePlayerAir;
        public float fourPlayerAir;
    }

    public AmountMinAir AmountMinAirForPush;
}
