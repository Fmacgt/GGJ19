
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class MoveSystem : MonoBehaviour
    {
        public MoveComponent[] components;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            float dt = Time.deltaTime;
            for (int i = 0; i < components.Length; i++) {
                var entry = components[i];
                entry.targetTr.position += entry.direction * entry.speed * dt;
            }
        }
    }
}
