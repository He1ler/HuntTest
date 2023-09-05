using System.Collections;
using UnityEngine;
public class LaunchScript : MonoBehaviour
{
    public GameObject hunter;
    [Header("Objects")]
    [SerializeField] GameObject currentGizmo;
    [SerializeField] Camera cam;
    Transform player;
    [Header("Parametrs")]
    [SerializeField] float maxTime = .7f;
    [SerializeField] float h = 5;
    [SerializeField] float gravity = -18;
    [SerializeField] Material gizmoMaterialTrue;
    [SerializeField] Material gizmoMaterialFalse;
    Animator playerAnimator;
    MeshRenderer gizmoMesh;
    Rigidbody ball;
    LineRenderer lineRenderer;
    float time = .5f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        gizmoMesh = currentGizmo.GetComponent<MeshRenderer>();
    }
    private void LateUpdate()
    {
        time -= Time.deltaTime;
        if (currentGizmo != null)
        {
            MoveCurrentObjectToMouse();
            ReleaseIfClicked();
        }
    }
    private void Start()
    {
        RenewAnimal();
    }
    void Launch()
    {
        if (time < 0)
        {
            StartCoroutine(LaunchIE(.2f));
            return;
        }
        CleanPath();
    }
    IEnumerator LaunchIE(float timeToWait)
    {
        playerAnimator.SetTrigger("Win");
        time = maxTime;
        yield return new WaitForSeconds(timeToWait);
        ball = hunter.GetComponent<Rigidbody>();
        ball.isKinematic = false;
        ball.useGravity = true;
        Physics.gravity = Vector3.up * gravity;
        ball.velocity = CalculateLaunchData().initialVelocity;
        lineRenderer.positionCount = 0;
    }
    private void MoveCurrentObjectToMouse()
    {
        if (Input.GetMouseButton(0))
        {
            currentGizmo.SetActive(true);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 250f, 1 << 6))
            {
                MoveGizmo(hitInfo);
                return;
            }
        }
    }
    void MoveGizmo(RaycastHit hitInfo)
    {
        gizmoMesh.sharedMaterial = gizmoMaterialTrue;
        ChangeGizmoPosition(hitInfo.point, currentGizmo.transform);
        DrawPath();
        return;
    }
    void ChangeGizmoPosition(Vector3 point, Transform gizmo)
    {
        gizmo.position = point;
    }
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonUp(0))
        {
            currentGizmo.SetActive(false);
            Launch();
        }
    }
    void CleanPath()
    {
        lineRenderer.positionCount = 0;
    }
    void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();

        int resolution = 100;
        for (int i = 0; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = player.position + displacement;
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, drawPoint);
        }
    }
    public void RenewAnimal()
    {
        player = hunter.transform;
        playerAnimator = hunter.GetComponent<Animator>();
    }
    LaunchData CalculateLaunchData()
    {
        float displacementY = currentGizmo.transform.position.y - player.position.y;
        Vector3 displacementXZ = new Vector3(currentGizmo.transform.position.x - player.position.x, 0, currentGizmo.transform.position.z - player.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}