using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] float pressDepth = 0.1f;
    [SerializeField] float moveSpeed = 0.15f;

    Vector3 startPos;
    Vector3 targetPos;
    Coroutine moveRoutine;
    public bool wasPressed { get; private set; } = false;
    public PresurePlateManager manager;

    void Awake()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Untagged")) return;

        wasPressed = true;
        targetPos = startPos + Vector3.down * pressDepth;
        StartMove();
        manager.PlatePressed(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Untagged")) return;

        targetPos = startPos;
        StartMove();
    }

    void StartMove()
    {
        if (moveRoutine == null)
            moveRoutine = StartCoroutine(MoveCoroutine());
    }


    IEnumerator MoveCoroutine()
    {
        while ((transform.localPosition - targetPos).sqrMagnitude > 0.0001f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.localPosition = targetPos;
        moveRoutine = null;
    }

    public void ResetPlate()
    {
        StartCoroutine(DisableCollider());
        wasPressed = false;
        targetPos = startPos;
        StartMove();
    }

    private IEnumerator DisableCollider()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(0.75f);
        boxCollider.enabled = true;
    }
}