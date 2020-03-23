
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class LookTowardsSystem : MonoBehaviour
    {
        public LookTowardsComponent[] components;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            for (int i = 0; i < components.Length; i++) {
                var entry = components[i];
                entry.targetTr.rotation = Quaternion.LookRotation(entry.direction);
            }
        }
    }
}
