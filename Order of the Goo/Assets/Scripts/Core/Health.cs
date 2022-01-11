using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RPG.Saving;

public class Health : MonoBehaviour, ISaveable { 
    public UnityEvent OnDeath;
    public UnityEvent OnRestore;
    [SerializeField] private GameObject sliderPrefab;
    [Header("Healthbar Settings")] 
    [SerializeField] private float fadeOutDelay = 3;
    [SerializeField] private float yOffset;
    [SerializeField] private bool displayHealthBorder;
    [SerializeField] private bool startHidden = false;
    [Header("Custom Healthbar")] [SerializeField]
    private bool useCustomSlider;

    private Animator animator;
    private float maxHealth;
    private DisplayState currentState;
    private float startTime;
    private bool isDead;
    private GameObject sliderObject;
    private Slider slider;
    [SerializeField] private float totalHealth;

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
        var creature = GetComponent<Creature>();
        var diceType = creature.CreatureSO.DiceType;
        var rawModifier = creature.CreatureSO.Constitution - 10;
        var modifier = rawModifier != 0 ? rawModifier / 2: 0;
        var levels = creature.CreatureSO.Level;
        for (int i = 0; i < levels; i++) {
            totalHealth += GameController.GetDiceValue(diceType);
        }
        totalHealth += modifier;
        totalHealth *= 5;
        Debug.Log(totalHealth);
        maxHealth = totalHealth;
        isDead = false;
        startTime = Time.time;
        sliderObject = Instantiate(sliderPrefab, new Vector2(transform.position.x, transform.position.y + yOffset), Quaternion.identity);
        sliderObject.transform.SetParent(transform);
        if (useCustomSlider) {
            currentState = DisplayState.CUSTOM;
            slider = sliderPrefab.GetComponent<Slider>();
        } else {
            currentState = DisplayState.IDLE;
            animator = sliderObject.GetComponent<Animator>();
            slider = sliderObject.GetComponent<Slider>(); 
            HideDisplay(); 
        }
        if (startHidden) {
            SetHidden(true);
        }
        slider.maxValue = maxHealth;
        slider.value = slider.maxValue;
    }

    private void Update() {
        if (isDead)
            return;
        HandleDisplay();
    }

    public void SetHidden(bool hidden) {
        animator.SetBool("Hidden", hidden);
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

    public void ShowDisplay() {
        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true); 
    }

    public void HideDisplay() {
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }
    
    private void HandleDeath() {
        isDead = true;
        if (OnDeath != null)
            OnDeath.Invoke();
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

    object ISaveable.CaptureState() { 
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict["totalHealth"] = totalHealth;
        dict["maxHealth"] = maxHealth;
        dict["isDead"] = IsDead;
        return dict;
    }

    void ISaveable.RestoreState(object state) {
        var savedState = (Dictionary<string, object>)state;
        if ((bool)savedState["isDead"]) {
            HandleDeath();
        } else {
            OnRestore.Invoke();
        }
        totalHealth = (float)savedState["totalHealth"];
        maxHealth = (float)savedState["maxHealth"];
        slider.value = totalHealth;
    }
}   