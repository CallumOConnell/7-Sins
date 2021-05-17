using UnityEngine;
using UnityEngine.UI;

namespace Sins.Interaction
{
    public class SpeakerUI : MonoBehaviour
    {
        [SerializeField]
        private Image _portrait;

        [SerializeField]
        private Text _nameText;

        [SerializeField]
        private Text _dialogueText;

        private Character _speaker;

        public Character Speaker
        {
            get => _speaker;
            set
            {
                _speaker = value;
                _portrait.sprite = _speaker.Portrait;
                _nameText.text = _speaker.Name;
            }
        }

        public string Dialog
        {
            set => _dialogueText.text = value;
        }

        public bool HasSpeaker() => _speaker != null;

        public bool SpeakerIs(Character character) => _speaker == character;

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}