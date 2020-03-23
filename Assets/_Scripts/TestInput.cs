
using System.Collections;
using UnityEngine;

using GGJ2019.UI;


namespace GGJ2019
{
    public sealed class TestInput : MonoBehaviour
    {
        public IPollInput pollInput;

        public RectTransform inputRangeTr;
        public RectTransform inputMinRangeTr;
        public AnimationCurve speedCurve;

        public float minInchToMove = 0.1f;
        public float flickThreshold = 0.3f;
        public float baseComboTimeLimit = 0.5f;
        public float minComboTimeLimit = 0.1f;
        public float comboTimeDecrease = 0.2f;

        public float maxMoveSpeed = 5f;
        public MoveComponent moveComponent;
        public LookTowardsComponent lookComponent;

        public DashEffect dashEffect;
        public float dashSpeed = 50f;

        public SlashAttack slashAttack;
        public SteamTank steamTank;
        public float dashCost = 50f;

        public Camera viewCam;


        public MovementIndicator movementIndicator;
        public ComboDisplay comboDisplay;

        [HideInInspector]
        public int attackCombo = 0;

        //==============================================================================

        private Quaternion _invViewRotate;
        private Quaternion _viewRotate;

        private Rect _inputRangeRect;
        private float _inputRangeRadius = 1f;
        private float _sqrMoveThreshold = 0f;

        private bool _moved = false;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            lookComponent.direction = lookComponent.targetTr.forward;
        }

        private IEnumerator Start()
        {
            var camTr = viewCam.transform;
            _invViewRotate = Quaternion.AngleAxis(camTr.eulerAngles.y, Vector3.up);
            _viewRotate = Quaternion.AngleAxis(-camTr.eulerAngles.y, Vector3.up);

            yield return null;

            _inputRangeRect = inputRangeTr.rect;
            _inputRangeRadius = _inputRangeRect.width * 0.5f;

            _sqrMoveThreshold = inputMinRangeTr.rect.width * 0.5f;
            _sqrMoveThreshold *= _sqrMoveThreshold;
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            var inputSet = pollInput.poll();

            // TODO: detect movement input:
            // 1. drag to move along that direction, with magnitude indicating target speed
            // 2. flick to 'dash' along a direction
            for (int touchIdx = 0; touchIdx < inputSet.activeCount; touchIdx++) {
                if (!inputSet.active[touchIdx] || inputSet.ignored[touchIdx]) {
                    continue;
                }

                // only handle the first active touch, for now
                var state = inputSet.states[touchIdx];
                var offset = state.position - state.initialPosition;
                float speedT = 0f;
                if (offset.sqrMagnitude >= _sqrMoveThreshold) {
                    var direction = new Vector3(offset.x, 0f, offset.y);
                    direction = _invViewRotate * direction;

                    float length = direction.magnitude;
                    speedT = Mathf.Min(length / _inputRangeRadius, 1f);
                    float speedCoeff = speedCurve.Evaluate(speedT);

                    direction.Normalize();

                    moveComponent.direction = direction;
                    moveComponent.speed = maxMoveSpeed * speedCoeff;

                    lookComponent.direction = direction;

                    if (!_moved) {
                        // anchor the movement indicator only when target is about to move
                        inputRangeTr.anchoredPosition = state.position;
                        movementIndicator.indicatorGroup.SetActive(true);
                    }

                    _moved = true;
                } else {
                    moveComponent.speed = 0f;
                    lookComponent.direction = lookComponent.targetTr.forward;
                }


                if (state.phase == TouchPhase.Ended) {
                    lookComponent.direction = lookComponent.targetTr.forward;

                    if (_moved) {
                        bool isFlicking = state.duration <= flickThreshold;
                        bool hasEnoughSteam = steamTank.hasEnoughSteam(dashCost);
                        if (!isFlicking || !hasEnoughSteam) {
                            moveComponent.speed = 0f;
                        } else {
                            steamTank.consume(dashCost);
                            dashEffect.apply(dashSpeed);
                        }
                        
                        movementIndicator.indicatorGroup.SetActive(false);
                    } else {
                        comboDisplay.frameTr.anchoredPosition = state.position;

                        // attack timing:
                        // 1. not while the previous attack is still performing
                        // 2. not too long after the previous attack ends
                        // => increase combo
                        float timeSinceLastAttack = Time.time - slashAttack.previousEndTime;
                        float comboTimeLimit = 
                            Mathf.Max(minComboTimeLimit, 
                                    baseComboTimeLimit - attackCombo * comboTimeDecrease);
                        bool inTimeRange = timeSinceLastAttack <= comboTimeLimit;
                        if (!slashAttack.StillAttacking && inTimeRange) {
                            attackCombo++;
                            comboDisplay.refresh(attackCombo, comboTimeLimit);
                        } else if (attackCombo == 0) {
                            comboDisplay.refresh(attackCombo, comboTimeLimit);
                        } else {
                            attackCombo = 0;
                            comboDisplay.abort();
                        }

                        slashAttack.attack(attackCombo);
                    }

                    _moved = false;
                } else if (_moved) {
                    var direct3D = _viewRotate * moveComponent.direction;
                    movementIndicator.update(new Vector2(direct3D.x, direct3D.z), speedT);
                }
            }

            // reset flags, if necessary
            for (int touchIdx = 0; touchIdx < inputSet.activeCount; touchIdx++) {
                if (inputSet.active[touchIdx] &&
                        inputSet.states[touchIdx].phase == TouchPhase.Ended) {
                    inputSet.active[touchIdx] = false;
                    inputSet.ignored[touchIdx] = false;
                }
            }
        }
    }
}
