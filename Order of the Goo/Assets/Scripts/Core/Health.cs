using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour {
    public delegate void DeathAction();

    public event DeathAction OnDeath;
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private float totalHealth = 100f;
    [Header("Healthbar Settings")] 
    [SerializeField] private float fadeOutDelay = 3;
    [SerializeField] private float yOffset;
    [SerializeField] private bool displayHealthBorder;

    [Header("Custom Healthbar")] [SerializeField]
    private bool useCustomSlider;

    private Animator animator;
    private float maxHealth;
    private DisplayState currentState;
    private float startTime;
    private bool isDead;
    private GameObject sliderObject;
    private Slider slider;

    public bool IsDead {
        get => isDead; 
    }

    private enum DisplayState {
        FADED,
        SHOWN,
        IDLE,
        CUSTOM
    }

    private void Start() {
        maxHealth = totalHealth;
        isDead = false;
        startTime = 0;
        sliderObject = Instantiate(sliderPrefab, new Vector2(transform.position.x, transform.position.y + yOffset), Quaternion.identity);
        sliderObject.transform.SetParent(transform);
        if (useCustomSlider) {
            currentState = DisplayState.CUSTOM;
            slider = sliderPrefab.GetComponent<Slider>();
        } else {
            currentState = DisplayState.FADED;
            animator = sliderObject.GetComponent<Animator>();
            slider = sliderObject.GetComponent<Slider>();
            HideDisplay();
        }
        slider.maxValue = maxHealth;
        slider.value = slider.maxValue;
    }

    private void Update() {
        if (isDead)
            return;
        HandleDisplay();
    }

    private void HandleDisplay() {
        switch (currentState) {
            case DisplayState.SHOWN:
                ShowDisplay();
                startTime = Time.time;
                currentState = DisplayState.IDLE;
                break;
            case DisplayState.IDLE:
                if (Time.time - startTime >= fadeOutDelay) {
                    HideDisplay();
                    currentState = DisplayState.FADED;
                } 
                break;
        }
    }

    private void ShowDisplay() {
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true); 
    }

    private void HideDisplay() {
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }
    
    private void HandleDeath() {
        isDead = true;
        if (OnDeath != null)
            OnDeath();
    }

    public void TakeDamage(float amount) {
        totalHealth -= amount;
        if (totalHealth <= 0 && !isDead) {
            HandleDeath();
        }
        slider.value = totalHealth;
        if (currentState != DisplayState.CUSTOM) {
            currentState = DisplayState.SHOWN;
        }
    }

    public void Heal(float amount) {
        totalHealth += amount;
        if (totalHealth > maxHealth) {
            totalHealth = maxHealth;
        }
    }

    public void OnDrawGizmos() {
        if (!displayHealthBorder)
            return;
        Vector2 position = transform.position;
        Gizmos.DrawWireCube(new Vector3(position.x, position.y + yOffset, 0), new Vector3(2, 1f, 1f));
    }
}   