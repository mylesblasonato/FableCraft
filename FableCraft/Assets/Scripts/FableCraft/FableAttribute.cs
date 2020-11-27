using System;
using UnityEngine;

namespace FableCraft
{
    [Serializable]
    public struct FableAttribute
    {
        [SerializeField] string _name;
        public string Name => _name;
        public int Value;
    }
}
