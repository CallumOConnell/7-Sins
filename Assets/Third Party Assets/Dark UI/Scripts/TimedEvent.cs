using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Dark
{
    public class TimedEvent : MonoBehaviour
    {
        [SerializeField]
        private float _timer = 4;

        [SerializeField]
        private bool _enableAtStart;

        [SerializeField]
        private UnityEvent _timerAction;

        private void Start()
        {
            if (_enableAtStart)
            {
                StartCoroutine("TimedEventStart");
            }
        }

        private IEnumerator TimedEventStart()
        {
            yield return new WaitForSeconds(_timer);

            _timerAction.Invoke();
        }

        public void StartIEnumerator()
        {
            StartCoroutine("TimedEventStart");
        }

        public void StopIEnumerator()
        {
            StopCoroutine("TimedEventStart");
        }
    }
}