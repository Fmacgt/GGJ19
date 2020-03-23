
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public sealed class PollTouchInput : IPollInput
    {
        [SerializeField]
        private int _maxTouches = 5;

        //==============================================================================

        private TouchInputSet _inputSet;

        /////////////////////////////////////////////////////////////////////////////////////

        public override TouchInputSet poll()
        {
            int activeInputCount = 0;

            for (int i = 0; i < Input.touchCount; i++) {
                var touch = Input.GetTouch(i);

                int touchIdx = touch.fingerId;
                if (touchIdx >= _maxTouches) {
                    // skip exceeding touches
                    continue;
                }

                _inputSet.active[touchIdx] = true;
                _inputSet.states[touchIdx].phase = touch.phase;

                if (_inputSet.ignored[touchIdx]) {
                    continue;
                }

                // active input: it is touching, it is not ignored
                activeInputCount++;

                var state = _inputSet.states[touchIdx];

                state.prevPosition = state.position;
                state.position = touch.position;
                if (state.phase == TouchPhase.Began) {
                    state.prevPosition = state.position;
                    state.initialPosition = state.position;

                    state.startTime = Time.unscaledTime;
                    state.duration = 0f;
                } else {
                    state.duration = Time.unscaledTime - state.startTime;
                }

                _inputSet.states[touchIdx] = state;
            }

            _inputSet.activeCount = activeInputCount;
            return _inputSet;
        }

        public override void resetInputStates()
        {
            for (int i = 0; i < _maxTouches; i++) {
                _inputSet.active[i] = false;
                _inputSet.ignored[i] = false;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _inputSet = new TouchInputSet();

            _inputSet.capacity = _maxTouches;
            _inputSet.activeCount = 0;
            _inputSet.active = new bool[_maxTouches];
            _inputSet.ignored = new bool[_maxTouches];
            _inputSet.states = new TouchInputState[_maxTouches];
        }
    }
}
