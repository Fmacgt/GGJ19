
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace GGJ2019
{
    public sealed class SteamTank : MonoBehaviour
    {
        public float capacity = 100f;
        public float refillRate = 25f;
        public float refillWait = 1f;

        public Text percentLabel;
        public Image gaugeFiller;

        //==============================================================================

        private float _remaining = 0f;

        private bool _refilling = false;
        private float _refillWait = 0f;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            _remaining = capacity;
            _refilling = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////

        public bool hasEnoughSteam(float amount)
        {
            return _remaining >= amount;
        }

        public void consume(float amount)
        {
            _remaining -= amount;

            _refilling = false;
            _refillWait = 0f;

            _showAmount(_remaining);
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            float dt = Time.deltaTime;
            if (_refilling) {
                _remaining = Mathf.Min(capacity, _remaining + refillRate * dt);
                _showAmount(_remaining);
            } else {
                _refillWait += dt;
                _refilling = _refillWait >= refillWait;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////

        private void _showAmount(float remaining)
        {
            percentLabel.text = (Mathf.Round(remaining * 100f) * 0.01f).ToString();
            gaugeFiller.fillAmount = remaining / capacity;
        }
    }
}
