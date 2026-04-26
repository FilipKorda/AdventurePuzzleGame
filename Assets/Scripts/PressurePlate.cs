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

    public bool isPresureDissabled = false;

    private bool stayPressed = false;

    private bool waitForPlayerExitAfterReset = false;


    void Awake()
    {
        isPresureDissabled = false;
        startPos = transform.localPosition;
        targetPos = startPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!manager.puzzleIsActivated) return;
        if (isPresureDissabled) return;
        if (!other.CompareTag("Player")) return;
        if (wasPressed) return;
        if (waitForPlayerExitAfterReset) return;

        wasPressed = true;
        stayPressed = true;
        targetPos = startPos + Vector3.down * pressDepth;
        StartMove();
        manager.PlatePressed(this);
    }



    void OnTriggerExit(Collider other)
    {
        if (!manager.puzzleIsActivated) return;

        if (!other.CompareTag("Player")) return;

        if (waitForPlayerExitAfterReset)
        {
            waitForPlayerExitAfterReset = false;
        }

        if (isPresureDissabled) return;
        if (stayPressed) return;

        targetPos = startPos;
        StartMove();
    }



    void StartMove()
    {
        if (moveRoutine == null)
            moveRoutine = StartCoroutine(MoveCoroutine());

        Services.Audio.PlaySFX("PressurePlate");

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

    public void StandOnResetPlate()
    {
        wasPressed = false;
        stayPressed = false;
        waitForPlayerExitAfterReset = true;

        targetPos = startPos;
        StartMove();
    }

    public void OtherResetPlate()
    {
        wasPressed = false;
        stayPressed = false;
        waitForPlayerExitAfterReset = false;

        targetPos = startPos;
        StartMove();
    }
}