using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sins.Character
{
    public class HealPlayer : MonoBehaviour
    {
        [SerializeField]
        private int _amount = 30;

        [SerializeField]
        private float _cooldown = 5f;

        [SerializeField]
        private Image _icon;

        private bool _canUse = true;

        private PlayerStats _playerStats;

        private void Awake() => _playerStats = GetComponent<PlayerStats>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (_canUse && _playerStats.Health <= _playerStats.MaxHealth.GetValue())
                {
                    _playerStats.Heal(_amount);

                    _canUse = false;

                    StartCoroutine(StartCooldown());

                    ActivateCooldownUI();
                }
            }
        }

        private void ActivateCooldownUI()
        {
            _icon.fillAmount = 0;

            LeanTween.value(gameObject, 0, 1, _cooldown).setOnUpdate((float value) =>
            {
                _icon.fillAmount = value;
            });
        }

        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(_cooldown);

            _canUse = true;
        }
    }
}