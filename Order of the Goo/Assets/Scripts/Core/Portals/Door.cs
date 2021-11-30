using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Door : Portal {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();   
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        spriteRenderer.color = Color.yellow;
        animator.SetBool("Open", true);
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        spriteRenderer.color = Color.white;
        animator.SetBool("Open", false);
    }
}
