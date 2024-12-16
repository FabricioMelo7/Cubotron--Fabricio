using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Helper event args, used in various event handlers
    /// </summary>
    public class CustomEventArgs
    {
        public UnityEngine.Vector3 position { get; set; }
        public GameObject obj { get; set; }
        public float power { get; set; }
        public Enum Ability { get; set; }
    }
}