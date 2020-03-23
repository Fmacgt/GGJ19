
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class CameraFollow : MonoBehaviour
    {
        public Transform cameraTr;
        public Transform targetTr;

        [Range(0.1f, 5f)]
        public float distanceCoeff = 1f;

        public bool adjustForAspect = true;

        //==============================================================================

        private Vector3 _offset;
        private float _aspectCoeff = 1f;

        private const float DefaultAspect = 4f / 3f;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _offset = cameraTr.position - targetTr.position;
        }

        private void LateUpdate()
        {
            cameraTr.position = targetTr.position + _offset * distanceCoeff * _aspectCoeff;
        }
    }
}
