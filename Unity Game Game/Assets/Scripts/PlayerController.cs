using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class PlayerController : MonoBehaviour {
    [BoxGroup("Player Controls", centerLabel:true)]
    [LabelText("Player Speed")]
    [LabelWidth(100)]
    [Required]
    [SerializeField] float _playerSpeed = 10;
    private Rigidbody2D _playerRigidbody;
    private Vector2 _playerVelocity;
    private Animator _playerAnimator;
    private void Start() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }
 
    private void Update() {
        _playerRigidbody.velocity = _playerVelocity;
    }

    private void OnMove(InputValue value) {
        Vector2 inputAxis = value.Get<Vector2>();
        bool isHorizontal = Mathf.Abs(inputAxis.x) > Mathf.Epsilon;
        bool isVertical = Mathf.Abs(inputAxis.y) > Mathf.Epsilon;
        if (isHorizontal) {
            transform.localScale = new Vector3( Mathf.Sign(inputAxis.x), 1, 1);
            _playerAnimator.SetBool("isWalking", true);
        } else {
            _playerAnimator.SetBool("isWalking", false);
        }
        _playerVelocity = inputAxis * _playerSpeed;
    }
}
