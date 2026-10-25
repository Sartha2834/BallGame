
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallLauncher : MonoBehaviour
{
    [Header("Ball Settings")]
    public Rigidbody2D Ball;
    public float launchForce = 30f;
    public float maxDragDistance = 5f;

    [Header("Launch Power")]
    public float minLaunchPower = 15f;
    public float maxLaunchPower = 40f;

    [Header("Trajectory")]
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 50;
    public float timeStep = 0.06f;

    [Header("Trajectory Appearance")]
    [Range(0.05f, 1f)]
    public float trajectoryOpacity = 0.45f;
    public float trajectoryStartWidth = 0.045f;
    public float trajectoryEndWidth = 0.015f;

    [Header("Launch Angle Indicator")]
    public LineRenderer angleLine;
    public float angleLineLength = 1.2f;
    public float angleLineWidth = 0.07f;
    public Color angleColor = Color.yellow;

    [Header("Collision")]
    public float collisionPadding = 0.02f;

    [Header("Slingshot Bands")]
    public LineRenderer bandLeft;
    public LineRenderer bandRight;
    public Transform leftAnchor;
    public Transform rightAnchor;
    public float bandWidth = 0.05f;
    public Color bandColor = new Color(0.3f, 0.12f, 0.05f);

    private bool isDragging;
    private bool hasLaunched;

    private Vector2 startPosition;
    private Collider2D ballCollider;

    void Start()
    {
        if (Ball == null)
        {
            Debug.LogError("BallLauncher: Ball is not assigned!");
            enabled = false;
            return;
        }

        ballCollider = Ball.GetComponent<Collider2D>();
        startPosition = Ball.position;

        Ball.linearVelocity = Vector2.zero;
        Ball.angularVelocity = 0f;
        Ball.bodyType = RigidbodyType2D.Kinematic;

        SetupTrajectory();
        SetupAngleLine();

        SetupBand(bandLeft);
        SetupBand(bandRight);

        UpdateSlingshotBands();
    }

    void SetupTrajectory()
    {
        if (trajectoryLine == null)
            return;

        trajectoryLine.useWorldSpace = true;
        trajectoryLine.positionCount = 0;
        trajectoryLine.startWidth = trajectoryStartWidth;
        trajectoryLine.endWidth = trajectoryEndWidth;

        trajectoryLine.startColor =
            new Color(1f, 1f, 1f, trajectoryOpacity);

        trajectoryLine.endColor =
            new Color(1f, 1f, 1f, 0.03f);

        trajectoryLine.numCapVertices = 5;
        trajectoryLine.numCornerVertices = 5;
        trajectoryLine.sortingOrder = 90;
        trajectoryLine.enabled = true;
    }

    void SetupAngleLine()
    {
        if (angleLine == null)
            return;

        angleLine.useWorldSpace = true;
        angleLine.positionCount = 0;
        angleLine.startWidth = angleLineWidth;
        angleLine.endWidth = angleLineWidth;
        angleLine.startColor = angleColor;
        angleLine.endColor = angleColor;
        angleLine.numCapVertices = 5;
        angleLine.numCornerVertices = 5;
        angleLine.sortingOrder = 100;
        angleLine.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
            return;
        }

        if (Ball == null || hasLaunched || Camera.main == null)
            return;

        Vector2 pointerPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(pointerPosition);

            if (hit != null &&
                (hit.attachedRigidbody == Ball ||
                 hit.gameObject == Ball.gameObject))
            {
                isDragging = true;

                Ball.linearVelocity = Vector2.zero;
                Ball.angularVelocity = 0f;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 offset = pointerPosition - startPosition;

            offset = Vector2.ClampMagnitude(
                offset,
                maxDragDistance
            );

            Vector2 target = startPosition + offset;
            target = PreventWallOverlap(target);

            Ball.position = target;

            ShowTrajectory();
            ShowAngleIndicator();
            UpdateSlingshotBands();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector2 launchDirection = startPosition - Ball.position;
            float dragDistance = launchDirection.magnitude;

            isDragging = false;
            ClearAimLines();

            if (dragDistance > 0.05f)
            {
                float power = Mathf.Clamp01(
                    dragDistance / maxDragDistance
                );

                float finalPower = Mathf.Lerp(
                    minLaunchPower,
                    maxLaunchPower,
                    power
                );

                Ball.bodyType = RigidbodyType2D.Dynamic;
                Ball.linearVelocity = Vector2.zero;
                Ball.angularVelocity = 0f;

                Ball.AddForce(
                    launchDirection.normalized * finalPower,
                    ForceMode2D.Impulse
                );

                hasLaunched = true;

                if (bandLeft != null)
                    bandLeft.positionCount = 0;

                if (bandRight != null)
                    bandRight.positionCount = 0;

                Debug.Log("Ball launched with power: " + finalPower);
            }
            else
            {
                Ball.position = startPosition;
                UpdateSlingshotBands();
            }
        }
    }

    Vector2 PreventWallOverlap(Vector2 target)
    {
        if (ballCollider == null)
            return target;

        Vector2 currentPosition = Ball.position;
        Vector2 movement = target - currentPosition;
        float distance = movement.magnitude;

        if (distance <= 0.0001f)
            return target;

        float radius = Mathf.Max(
            ballCollider.bounds.extents.x,
            ballCollider.bounds.extents.y
        );

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            currentPosition,
            radius,
            movement.normalized,
            distance
        );

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || hit.collider.isTrigger)
                continue;

            if (hit.collider.attachedRigidbody == Ball)
                continue;

            if (Vector2.Dot(movement.normalized, hit.normal) >= 0f)
                continue;

            return currentPosition + movement.normalized *
                Mathf.Max(0f, hit.distance - collisionPadding);
        }

        return target;
    }

    void SetupBand(LineRenderer band)
    {
        if (band == null)
            return;

        band.useWorldSpace = true;
        band.positionCount = 2;
        band.startWidth = bandWidth;
        band.endWidth = bandWidth;
        band.startColor = bandColor;
        band.endColor = bandColor;
        band.numCapVertices = 4;
        band.sortingOrder = 10;
    }

    void UpdateSlingshotBands()
    {
        if (Ball == null || hasLaunched)
            return;

        if (bandLeft != null && leftAnchor != null)
        {
            bandLeft.positionCount = 2;
            bandLeft.SetPosition(0, leftAnchor.position);
            bandLeft.SetPosition(1, Ball.position);
        }

        if (bandRight != null && rightAnchor != null)
        {
            bandRight.positionCount = 2;
            bandRight.SetPosition(0, rightAnchor.position);
            bandRight.SetPosition(1, Ball.position);
        }
    }

    void ShowTrajectory()
    {
        if (trajectoryLine == null || Ball == null)
            return;

        Vector2 launchDirection = startPosition - Ball.position;
        float dragDistance = launchDirection.magnitude;

        if (dragDistance < 0.05f)
        {
            trajectoryLine.positionCount = 0;
            return;
        }

        float power = Mathf.Clamp01(
            dragDistance / maxDragDistance
        );

        float finalPower = Mathf.Lerp(
            minLaunchPower,
            maxLaunchPower,
            power
        );

        Vector2 launchVelocity =
            launchDirection.normalized * finalPower;

        trajectoryLine.positionCount = trajectoryPoints;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;

            Vector2 point =
                (Vector2)Ball.position
                + launchVelocity * t
                + 0.5f * Physics2D.gravity
                * Ball.gravityScale * t * t;

            trajectoryLine.SetPosition(i, point);
        }
    }

    void ShowAngleIndicator()
    {
        if (angleLine == null || Ball == null)
            return;

        Vector2 direction = startPosition - Ball.position;

        if (direction.magnitude < 0.05f)
        {
            angleLine.positionCount = 0;
            return;
        }

        direction.Normalize();

        Vector2 start = Ball.position;
        Vector2 end = start + direction * angleLineLength;

        angleLine.positionCount = 2;
        angleLine.SetPosition(0, start);
        angleLine.SetPosition(1, end);
    }

    void ClearAimLines()
    {
        if (trajectoryLine != null)
            trajectoryLine.positionCount = 0;

        if (angleLine != null)
            angleLine.positionCount = 0;
    }

    void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}
