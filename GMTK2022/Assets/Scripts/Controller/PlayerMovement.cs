using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float PlayerSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float controllerDeadZone = 0.1f;
    [SerializeField] private float gamepadRoateSmoothing = -1000f;
    [SerializeField] private float DashSpeed = 1;
    [SerializeField] private float DashDuration = 0.2f;

    private CharacterController controller;
    private bool hasDashInput = false;
    [HideInInspector]
    public bool isDashing = false;
    public Transform debugDistance;

    public Vector2 movement;
    private Vector2 aim;

    // 延45度角移动
    private Quaternion moveQuaternion = Quaternion.AngleAxis(45f, Vector3.up);

    private Vector3 playerVelocity;
    private Coroutine DashCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void HandleInput()
    {
        movement = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        hasDashInput = Input.GetKeyDown(KeyCode.Space);
        //Debug.Log(movement);
    }

    Vector3 DashDir;
    float StartDashTime;
    IEnumerator Dash()
    {
        //@Snoww:开始冲刺
        StartDashTime = Time.time;
        DashDir = movement.magnitude > 0.5f ? moveQuaternion * new Vector3(movement.x, 0, -movement.y) : transform.forward;
        DashDir.Scale(new Vector3(1, 0, 1));
        DashDir.Normalize();
        gameObject.layer = 9;
        isDashing = true;
        yield return new WaitForSeconds(DashDuration);
        //@Snoww:结束冲刺
        isDashing = false;
        gameObject.layer = 6;
    }

    void HandleMovement()
    {
        Vector3 move = moveQuaternion * new Vector3(movement.x, 0, -movement.y);
        if (!isDashing && hasDashInput)
        {
            if (DashCoroutine != null) StopCoroutine(DashCoroutine);
            DashCoroutine = StartCoroutine(Dash());
        }

        if (!isDashing)
            controller.Move(move * Time.deltaTime * PlayerSpeed);
        else
            //@Snoww:冲刺时速度线性衰减
            controller.Move(DashDir * Time.deltaTime * Mathf.Lerp(DashSpeed, 0, (Time.time - StartDashTime) / DashDuration));

    }
    void HandleRoation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
        }
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRoation();
        //Debug.Log("玩家距离debugpos距离"+Vector3.Distance(transform.position, debugDistance.transform.position));
    }
}
