using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveCamera))]
public class PlayerController : MonoBehaviour
{
    const float MaxStamina = 100f;

    [SerializeField]
    private float Speed = 5f;
    [SerializeField]
    private float JumpStrange = 0.32f;
    [SerializeField]
    private float Stamina = MaxStamina;
    [SerializeField]
    private float RateStaminaRemove = 0.05f;
    [SerializeField]
    private float RateSpeedUp = 2f;

    public static event _WastageStamina WastageStamina;
    public delegate void _WastageStamina(float stamina);

    private float gravity = -9.81f;
    private bool isSpeedUp = false;
    private bool isJump = false;
    private float timeJump = 0;
    private CharacterController Controller;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameState.IsPaused)
            return;

        if (Input.GetKey(KeyCode.LeftShift))
            isSpeedUp = true;

        if (Input.GetKey(KeyCode.Space) && !isJump)
        {
            timeJump = 0;
            isJump = true;
        }

        float targetSpeed = Speed;

        if (isSpeedUp)
        {
            targetSpeed *= RateSpeedUp;
            Stamina -= RateStaminaRemove;
            WastageStamina?.Invoke(Stamina);
        }

        float curSpeedV = Input.GetAxis("Vertical") * targetSpeed;
        float curSpeedH = Input.GetAxis("Horizontal") * targetSpeed;

        Vector3 moveDirection = (Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up) * curSpeedV) + (Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up) * curSpeedH);

        if (!isJump || timeJump > JumpStrange)
            moveDirection.y += gravity * 0.68f;
        else
            moveDirection.y -= gravity * 0.65f;

        Controller.Move(moveDirection * Time.deltaTime);
        isSpeedUp = false;

        if ((Stamina + RateStaminaRemove) < MaxStamina)
        {
            Stamina += RateStaminaRemove / (RateSpeedUp*2);
            WastageStamina?.Invoke(Stamina);
        }
        else if (Stamina < MaxStamina)
        {
            Stamina = MaxStamina;
            WastageStamina?.Invoke(Stamina);
        }

        if (isJump) {
            timeJump += Time.deltaTime;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (TypeObjects.isTypeObject(TypeObject.Ground & TypeObject.Rocks & TypeObject.SmallTrees & TypeObject.Foundation, hit.gameObject))
        {
            isJump = false;
        }
    }
}
