using System;
using UnityEngine;
using UnityEngine.UI;

public class ChipVisual : MonoBehaviour
{
    [SerializeField] private Image imageChip;
    [SerializeField] private LayerMask sectorMask;

    [Header("Detection")]
    [SerializeField] private float stopSpeed = 0.1f;
    [SerializeField] private float stopDelay = 0.15f;

    [Header("Launch")]
    [SerializeField] private float minLaunchForce = 0.5f;

    [Header("Braking")]
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float angularDrag = 3f;

    public event System.Action<ChipVisual> Finished;

    private Rigidbody2D rb;
    private bool inFlight;
    private bool resolved;

    public bool InFlight => inFlight;
    public bool IsResolved => resolved;

    public void Initialize(Sprite sprite)
    {
        imageChip.sprite = sprite;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.drag = linearDrag;
        rb.angularDrag = angularDrag;
    }

    public void Launch(Vector2 direction, float force)
    {
        if (direction.sqrMagnitude < 0.0001f)
        {
            Debug.LogWarning("Chip.Launch: нулевое направление");
            return;
        }

        if (force < minLaunchForce)
            force = minLaunchForce;

        CancelInvoke(nameof(Resolve));
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        inFlight = true;
        resolved = false;

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (!inFlight || resolved) return;

        if (rb.velocity.sqrMagnitude < stopSpeed * stopSpeed)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            resolved = true;
            Invoke(nameof(Resolve), stopDelay);
        }
    }

    void Resolve()
    {
        inFlight = false;

        var sector = GetSectorUnderChip();
        if (sector != null)
            sector.SetChip(this);
        else
            OnFail?.Invoke();

        Destroy(rb);

        Finished?.Invoke(this);
    }

    private ISector GetSectorUnderChip()
    {
        Vector2 point = transform.position; // середина фишки (при корректном pivot)
        Collider2D hit = Physics2D.OverlapPoint(point, sectorMask);
        if (hit == null) return null;
        return hit.GetComponentInParent<ISector>();
    }

    #region Output

    public event Action OnFail;

    #endregion
}
