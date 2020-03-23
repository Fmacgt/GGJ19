
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace GGJ2019.UI
{
    public sealed class ComboDisplay : MonoBehaviour
    {
        public RectTransform frameTr;
        public RectTransform ringTr;

        public Text label;
        public TestInput control;

        public Image timerFiller;

        public AnimationCurve openCurve;

        //==============================================================================

        private bool _countingDown = false;
        private float _countdownRate;
        private float _filler;

        /////////////////////////////////////////////////////////////////////////////////////

        private void Update()
        {
            if (_countingDown) {
                _filler = Mathf.Max(_filler - Time.deltaTime * _countdownRate, 0f);
                timerFiller.fillAmount = _filler;

                if (_filler <= 0f) {
                    abort();
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////

        public void refresh(int combo, float duration)
        {
            label.text = combo.ToString();

            _countingDown = true;
            _countdownRate = 1f / duration;

            _filler = 1f;
            timerFiller.fillAmount = 1f;

            open();
        }

        public void abort()
        {
            _countingDown = false;
            label.text = "0";

            close();
        }

        /////////////////////////////////////////////////////////////////////////////////////

        public void open()
        {
            gameObject.SetActive(true);
            LeanTween.cancel(ringTr.gameObject);

            ringTr.localScale = Vector3.zero;
            LeanTween.scale(ringTr, Vector3.one, 0.2f)
                .setEase(openCurve);
        }

        public void close()
        {
            LeanTween.cancel(ringTr.gameObject);

            LeanTween.scale(ringTr, Vector3.zero, 0.1f)
                .setOnComplete(() => { gameObject.SetActive(false); });
        }
    }
}
