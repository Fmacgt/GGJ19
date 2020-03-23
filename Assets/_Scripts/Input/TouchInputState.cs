
using UnityEngine;


namespace GGJ2019
{
    public struct TouchInputState
    {
        public TouchPhase phase;

        public Vector2 position;
        public Vector2 prevPosition;
        public Vector2 initialPosition;

        public float startTime;
        public float duration;
    }
}
