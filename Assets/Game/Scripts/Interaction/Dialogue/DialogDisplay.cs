using UnityEngine;
using Sins.Character;
using TMPro;

namespace Sins.Interaction
{
    public class DialogDisplay : Interactable
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private GameObject _dialogueScreen;

        [SerializeField]
        private GameObject _speakerLeft;

        [SerializeField]
        private GameObject _speakerRight;

        [SerializeField]
        private Conversation _conversation;

        private SpeakerUI _speakerUILeft;
        private SpeakerUI _speakerUIRight;

        private int _activeLineIndex = 0;

        private bool _isInteracting = false;

        private void Start()
        {
            _speakerUILeft = _speakerLeft.GetComponent<SpeakerUI>();
            _speakerUIRight = _speakerRight.GetComponent<SpeakerUI>();

            _speakerUILeft.Speaker = _conversation.SpeakerLeft;
            _speakerUIRight.Speaker = _conversation.SpeakerRight;
        }

        public override void Update()
        {
            base.Update();

            var distance = Vector3.Distance(_player.transform.position, transform.position);

            if (_isInteracting)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    AdvanceConversation();
                }
            }

            if (distance >= Radius || !_isInteracting)
            {
                _dialogueScreen.SetActive(false);

                _isInteracting = false;
            }
        }

        public override void Interact()
        {
            base.Interact();

            if (_player != null)
            {
                _dialogueScreen.SetActive(true);

                _isInteracting = true;

                _player.GetComponent<PlayerController>().MovementLocked = true;

                AdvanceConversation();
            }
        }

        private void AdvanceConversation()
        {
            if (_activeLineIndex < _conversation.Lines.Length)
            {
                DisplayLine();
                _activeLineIndex += 1;
            }
            else
            {
                _speakerUILeft.Hide();
                _speakerUIRight.Hide();
                _activeLineIndex = 0;
                _isInteracting = false;
                _player.GetComponent<PlayerController>().MovementLocked = false;
            }
        }

        private void DisplayLine()
        {
            var line = _conversation.Lines[_activeLineIndex];
            var character = line.Character;

            if (_speakerUILeft.SpeakerIs(character))
            {
                SetDialog(_speakerUILeft, _speakerUIRight, line.Text);
            }
            else
            {
                SetDialog(_speakerUIRight, _speakerUILeft, line.Text);
            }
        }

        private void SetDialog(SpeakerUI activeSpeakerUI, SpeakerUI inactiveSpeakerUI, string text)
        {
            activeSpeakerUI.Dialog = text;
            activeSpeakerUI.Show();
            inactiveSpeakerUI.Hide();
        }
    }
}