
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class LookTowardsComponent : MonoBehaviour
    {
        public Transform targetTr;
        public Vector3 direction;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            direction = targetTr.forward;
        }
    }
}
