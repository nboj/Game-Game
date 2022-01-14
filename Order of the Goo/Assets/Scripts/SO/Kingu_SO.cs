using UnityEngine;

[CreateAssetMenu(menuName = "Kingu", fileName = "New Kingu")]
public class Kingu_SO : Enemy_SO {
    [Header("Kingu")]
    [SerializeField] private float chargeSpeed; 
    [Header("Attack Delays")]
    [SerializeField] private float chargeDelay;
    [SerializeField] private float slimeLauncherDelay;
    [SerializeField] private float swingDelay;

    public float ChargeDelay {
        get => chargeDelay;
    }

    public float SlimeLauncherDelay {
        get => slimeLauncherDelay;
    }

    public float SwingDelay { 
        get => swingDelay;
    }

    public float ChargeSpeed {
        get => chargeSpeed;
    } 
}