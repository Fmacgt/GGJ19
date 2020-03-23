
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class PollMouseInput : IPollInput
    {
        public float moveInches = 0.05f;

        //==============================================================================

        private TouchInputSet _inputSet;
        private float _sqrMoveThreshold = 0f;

        private const int MaxTouches = 3;

        /////////////////////////////////////////////////////////////////////////////////////

        public override TouchInputSet poll()
        {
            int activeInputCount = 0;

            for (int i = 0; i < MaxTouches; i++) {
                if (_inputSet.active[i]) {
                    var state = _inputSet.states[i];

                    state.prevPosition = state.position;
                    state.position = (Vector2)Input.mousePosition;

                    state.duration = Time.unscaledTime - state.startTime;
                    if (Input.GetMouseButton(i)) {
                        var offset = (state.position - state.prevPosition).sqrMagnitude;
                        state.phase = offset > _sqrMoveThreshold ?
                            TouchPhase.Moved : TouchPhase.Stationary;
                    } else if (Input.GetMouseButtonUp(i)) {
                        state.phase = TouchPhase.Ended;
                    }

                    _inputSet.states[i] = state;
                } else if (Input.GetMouseButtonDown(i)) {
                    _inputSet.active[i] = true;

                    var state = _inputSet.states[i];

                    state.phase = TouchPhase.Began;

                    state.position = (Vector2)Input.mousePosition;
                    state.prevPosition = state.position;
                    state.initialPosition = state.position;

                    state.startTime = Time.unscaledTime;
                    state.duration = 0f;

                    _inputSet.states[i] = state;
                }
            }

            for (int i = 0; i < MaxTouches; i++) {
                activeInputCount += _inputSet.active[i] ? 1 : 0;
            }
            _inputSet.activeCount = activeInputCount;

            return _inputSet;
        }

        public override void resetInputStates()
        {
            for (int i = 0; i < MaxTouches; i++) {
                _inputSet.active[i] = false;
            }

            for (int i = 0; i < MaxTouches; i++) {
                _inputSet.ignored[i] = false;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _inputSet = new TouchInputSet();

            _inputSet.capacity = MaxTouches;
            _inputSet.activeCount = 0;
            _inputSet.active = new bool[MaxTouches];
            _inputSet.ignored = new bool[MaxTouches];
            _inputSet.states = new TouchInputState[MaxTouches];


            float minMoveOffset = Screen.dpi * moveInches;
            _sqrMoveThreshold = minMoveOffset * minMoveOffset;
        }
    }
}
