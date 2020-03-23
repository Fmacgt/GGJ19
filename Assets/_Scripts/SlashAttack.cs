
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class SlashAttack : MonoBehaviour
    {
        public Transform armTr;

        public float standAngle = -90f;
        public float attackAngle = 0f;
        public AnimationCurve attackCurve;
        public float baseAttackTime = 0.3f;

        [HideInInspector]
        public float previousEndTime = 0f;

        //==============================================================================

        private Quaternion _standRotate;
        private Quaternion _attackRotate;

        private bool _attacking = false;
        private float _attackTicks = 0f;
        private float _attackRate = 1f;

        private bool _reseting = false;

        /////////////////////////////////////////////////////////////////////////////////////

        public bool StillAttacking { get { return _attacking; } }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _standRotate = Quaternion.AngleAxis(standAngle, Vector3.right);
            _attackRotate = Quaternion.AngleAxis(attackAngle, Vector3.right);

            _attackRate = 1f / baseAttackTime;

            armTr.localRotation = _standRotate;
        }

        /////////////////////////////////////////////////////////////////////////////////////

        public bool attack(int combo)
        {
            // rotate the arm from a standing position to a horizontal position
            if (!_attacking) {
                armTr.localRotation = _standRotate;
                _attackTicks = 0f;
                _attacking = true;

                _reseting = false;

                return true;
            }

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            if (_reseting) {
                _reseting = false;
                armTr.localRotation = _standRotate;
            }


            if (!_attacking) {
                return;
            }

            _attackTicks += Time.deltaTime * _attackRate;
            armTr.localRotation = Quaternion.Lerp(_standRotate, _attackRotate,
                    attackCurve.Evaluate(_attackTicks));

            if (_attackTicks >= 1f) {
                _attacking = false;
                _reseting = true;

                previousEndTime = Time.time;
            }
        }
    }
}
