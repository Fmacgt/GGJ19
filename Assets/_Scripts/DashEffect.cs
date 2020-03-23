
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class DashEffect : MonoBehaviour
    {
        public float duration = 0.5f;
        public AnimationCurve speedCurve;

        public MoveComponent moveComponent;

        public ParticleSystem[] dashEffects;

        //==============================================================================

        private bool _dashing = false;
        private float _maxSpeed = 0f;
        private float _speedTicks = 0f;
        private float _declineRate = 1f;

        /////////////////////////////////////////////////////////////////////////////////////

        public void apply(float maxSpeed)
        {
            _maxSpeed = maxSpeed;
            moveComponent.speed = _maxSpeed;
            _speedTicks = 0f;
            _dashing = true;

            dashEffects[0].Play();
            dashEffects[1].Play();
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _declineRate = 1f / duration;
        }

        private void Update()
        {
            if (!_dashing) {
                return;
            }

            _speedTicks += Time.deltaTime * _declineRate;
            moveComponent.speed = Mathf.Lerp(_maxSpeed, 0f, speedCurve.Evaluate(_speedTicks));

            if (_speedTicks >= 1f) {
                _dashing = false;
            }
        }
    }
}
