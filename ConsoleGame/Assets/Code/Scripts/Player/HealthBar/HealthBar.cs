using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPerson;
using UnityEngine.InputSystem;
using UnityEngine.UI;

    public class HealthBar : MonoBehaviour
    {
        public GameObject healthBarParent;
        public GameObject ammoBar;
        public Image healthBar;
        public float healthCurrentAmount;
        public float healthMaxAmount = 100f;
        private GameObject _player;
        private PlayerInput _playerInput;
        private PlayerControls _playerControls;

        public void Start()
        {
            healthCurrentAmount = healthMaxAmount;
            healthBarParent.SetActive(false);
            ammoBar.SetActive(false);

            _player = GameObject.FindGameObjectWithTag("Player");
            _playerInput = _player.GetComponent<PlayerInput>();
            _playerControls = _player.GetComponent<PlayerControls>();
        }
        public void Update()
        {
            if (healthCurrentAmount < healthMaxAmount)
            {
                healthBarParent.SetActive(true);
            }
            if (_playerControls.interact)
            {
                TakeDamge(20);
                _playerControls.interact = false;
            }

        

    }
        public void TakeDamge(float damage)
        {
            healthCurrentAmount -= damage;
            healthBar.fillAmount = Mathf.Min(healthCurrentAmount, 0f);
            UpdateHealthBar();

            if (healthCurrentAmount <= 0)
            {
                // Hide the health bar if health drops to or below 0
                healthBarParent.SetActive(false);
            }
        }
        void UpdateHealthBar()
        {
            healthBar.fillAmount = healthCurrentAmount / healthMaxAmount;
        }
    }
