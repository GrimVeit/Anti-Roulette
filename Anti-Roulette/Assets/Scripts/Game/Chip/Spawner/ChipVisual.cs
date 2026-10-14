using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class ChipVisual : MonoBehaviour
{
    [SerializeField] private Image imageChip;
    [Header("Detection")]
    [SerializeField] private float stopSpeed = 0.1f;
    [SerializeField] private float stopDelay = 0.15f;

    [Header("Optional")]
    [SerializeField] private float minLaunchForce = 0.5f;
    [SerializeField] private float linearDrag = 2f;
    [SerializeField] private float angularDrag = 3f;

    public event System.Action<ChipVisual> Finished;

    private LayerMask sectorMask;
    private Rigidbody2D rb;
    private bool inFlight;
    private bool resolved;

    public bool InFlight => inFlight;
    public bool IsResolved => resolved;

    public void Initialize(LayerMask layerMask, Sprite sprite)
    {
        imageChip.sprite = sprite;
        sectorMask = layerMask;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.drag = linearDrag;          // Unity 2022 и старше
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

        // Сброс, если фишку перезапускают
        //CancelInvoke(nameof(Resolve));
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
            //Invoke(nameof(Resolve), stopDelay);
        }
    }

    //void Resolve()
    //{
    //    inFlight = false;

    //    Collider2D hit = Physics2D.OverlapPoint(rb.position, sectorMask);
    //    if (hit != null && hit.TryGetComponent<ISector>(out var sector))
    //    {
    //        sector.OnChipLanded(this);
    //    }
    //    else if (hit == null)
    //    {
    //        Debug.Log("Chip: остановилась вне секторов");
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"Chip: коллайдер {hit.name} не реализует ISector");
    //    }

    //    Finished?.Invoke(this);
    //}
}
