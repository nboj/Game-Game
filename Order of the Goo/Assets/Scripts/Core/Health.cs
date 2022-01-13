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
    [SerializeField] private bool isEnabled = true;

    private Animator animator;
    private float maxHealth;
    private DisplayState currentState;
    private float startTime;
    private bool isDead = false;
    private GameObject sliderObject;
    private Slider slider;
    [SerializeField] private float totalHealth;

    public bool IsDead {
        get => isDead;  
    }

    public bool IsEnabled {
        get => isEnabled;
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
        var modifier = rawModifier != 0 ? rawModifier / 2 : 0;
        var levels = creature.CreatureSO.Level;
        if (totalHealth == 0) {
            SetupHealth(levels, modifier, diceType);
            maxHealth = totalHealth; 
        }
        startTime = Time.time;
        sliderObject = Instantiate(sliderPrefab, new Vector2(transform.position.x, transform.position.y + yOffset), Quaternion.identity);
        sliderObject.transform.SetParent(transform);
        animator = sliderObject.GetComponent<Animator>();
        slider = sliderObject.GetComponent<Slider>();
        if (useCustomSlider) {
            currentState = DisplayState.CUSTOM;
            slider = sliderPrefab.GetComponent<Slider>();
        } else {
            currentState = DisplayState.IDLE;
            HideDisplay();
        }
        if (startHidden) {
            SetHidden(true);
        }
        slider.maxValue = maxHealth;
        slider.value = totalHealth;
    }

    public void Enable() {
        ShowDisplay();
        isEnabled = true;
    }

    public void Disable() {
        HideDisplay(); 
        isEnabled = false;
    } 

    private void SetupHealth(float levels, float modifier, DiceType diceType) {
        for (int i = 0; i < levels; i++) {
            totalHealth += GameController.GetDiceValue(diceType);
        }
        totalHealth += modifier;
        totalHealth *= 5;
        Debug.Log(totalHealth);
    }

    private void Update() {
        if (isDead || !isEnabled)
            return;
        HandleDisplay();
    }

    public void SetHidden(bool hidden) { 
        animator.SetBool("Hidden", hidden);
    }

    private void HandleDisplay() {
        if (IsEnabled) {
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
    }

    public void ShowDisplay() { 
        if (animator != null && isEnabled && animator.isActiveAndEnabled) {
            animator.SetBool("FadeOut", false);
            animator.SetBool("FadeIn", true);
        }
    }

    public void HideDisplay() { 
        if (animator != null && isEnabled && animator.isActiveAndEnabled) {
            animator.SetBool("FadeIn", false);
            animator.SetBool("FadeOut", true);
        }
    }
    
    private void HandleDeath() {
        isDead = true;
        if (OnDeath != null)
            OnDeath.Invoke();
    }

    public void TakeDamage(float amount) {
        if (!isEnabled)
            return;
        totalHealth -= amount;
        if (totalHealth <= 0 && !isDead) {
            totalHealth = -1;
            HandleDeath();
        }
        slider.value = totalHealth;
        if (currentState != DisplayState.CUSTOM) {
            currentState = DisplayState.SHOWN;
        }
    }

    public void Heal(float amount) {
        if (!isEnabled)
            return;
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
        dict["isDead"] = isDead;
        dict["isEnabled"] = isEnabled;
        dict["currentState"] = currentState; 
        return dict;
    }

    void ISaveable.RestoreState(object state) {
        var savedState = (Dictionary<string, object>)state;
        isEnabled = (bool)savedState["isEnabled"];
        isDead = (bool)savedState["isDead"];
        currentState = (DisplayState)savedState["currentState"]; 
        if (isDead) {
            HandleDeath(); 
        } 
        totalHealth = (float)savedState["totalHealth"];
        maxHealth = (float)savedState["maxHealth"];
        if (slider != null) {
            slider.value = totalHealth;
        }
    }
}   