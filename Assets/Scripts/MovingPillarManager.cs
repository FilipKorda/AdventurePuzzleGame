using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovingPillarManager : MonoBehaviour
{
    public Transform leftPoint;
    public Transform centerPoint;
    public Transform rightPoint;

    [SerializeField] private InputActionReference dragDeltaInput;
    [SerializeField] private InputActionReference dragHoldInput;

    [SerializeField] private float dragSpeed = 0.01f;
    [SerializeField] private float snapSmooth = 0.06f;
    [SerializeField] private float stayTime = 0.3f;

    bool holdingPPM;
    Vector3 velocity;
    Coroutine moveRoutine;
    private int imageIndex = 0;
    bool hasMoved;

    [SerializeField] private Image leftImage;
    [SerializeField] private Image centerImage;
    [SerializeField] private Image rightImage;

    [SerializeField] private Sprite spriteSymbol0;
    [SerializeField] private Sprite spriteSymbol1;
    [SerializeField] private Sprite spriteSymbol2;

    [SerializeField] private Sprite transparentSymbol;

    [SerializeField] private GameObject[] leftSymbols0;
    [SerializeField] private GameObject[] centerSymbols1;
    [SerializeField] private GameObject[] rightSymbols2;

    [SerializeField] private float rotationDuration = 1.2f;
    [SerializeField] private int maxRotations = 4;

    [SerializeField] private GameObject rightRotatingObject;
    [SerializeField] private GameObject centerRotatingObject;
    [SerializeField] private GameObject leftRotatingObject;


    [SerializeField] private ThreeSymbolsPillarManager threeSymbolsPillarManager;



    void OnEnable()
    {
        if (dragDeltaInput != null)
        {
            dragDeltaInput.action.Enable();
            dragDeltaInput.action.performed += OnDragDelta;
        }

        if (dragHoldInput != null)
        {
            dragHoldInput.action.Enable();
            dragHoldInput.action.performed += ctx => SetClickAndDrag(true);
            dragHoldInput.action.canceled += ctx => SetClickAndDrag(false);
        }
    }

    void OnDisable()
    {
        if (dragDeltaInput != null)
        {
            dragDeltaInput.action.performed -= OnDragDelta;
            dragDeltaInput.action.Disable();
        }

        if (dragHoldInput != null)
        {
            dragHoldInput.action.performed -= ctx => SetClickAndDrag(true);
            dragHoldInput.action.canceled -= ctx => SetClickAndDrag(false);
            dragHoldInput.action.Disable();
        }
    }

    public void DisableInput()
    {
        if (dragDeltaInput != null)
        {
            dragDeltaInput.action.performed -= OnDragDelta;
            dragDeltaInput.action.Disable();
        }

        if (dragHoldInput != null)
        {
            dragHoldInput.action.performed -= ctx => SetClickAndDrag(true);
            dragHoldInput.action.canceled -= ctx => SetClickAndDrag(false);
            dragHoldInput.action.Disable();
        }

    }

    public void SetClickAndDrag(bool state)
    {
        holdingPPM = state;

        if (!state)
        {
            Transform target = GetClosestPointPhysical();


            if (moveRoutine != null) StopCoroutine(moveRoutine);

            moveRoutine = StartCoroutine(SnapAndAssignSequence(target, hasMoved));

            hasMoved = false;
        }
        else
        {
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
        }
    }

    void OnDragDelta(InputAction.CallbackContext context)
    {
        if (!holdingPPM) return;
      
        Vector2 delta = context.ReadValue<Vector2>();

        if (delta.sqrMagnitude > 0.001f)
        {
            hasMoved = true;
        }

        Vector3 offset = new(-delta.x * dragSpeed, 0f, 0f);
        transform.position += offset;
    }

    Transform GetClosestPointPhysical()
    {
        float dLeft = Vector3.Distance(transform.position, leftPoint.position);
        float dCenter = Vector3.Distance(transform.position, centerPoint.position);
        float dRight = Vector3.Distance(transform.position, rightPoint.position);

        if (dLeft <= dCenter && dLeft <= dRight) return leftPoint;
        if (dCenter <= dLeft && dCenter <= dRight) return centerPoint;
        return rightPoint;
    }

    IEnumerator SnapAndAssignSequence(Transform snapTarget, bool shouldSwap)
    {
        if (AnyArrayLostActive())
        {
            ResetAll();
            yield break;
        }

        yield return MoveSmooth(snapTarget.position, snapSmooth);

        if (shouldSwap && imageIndex < 3)
        {
            GameObject currentRotObj = null;
            Image currentTargetImg = null;

            int symbolIdx = -1;
            if (snapTarget == leftPoint) symbolIdx = GetActiveIndex(leftSymbols0);
            else if (snapTarget == centerPoint) symbolIdx = GetActiveIndex(centerSymbols1);
            else symbolIdx = GetActiveIndex(rightSymbols2);

            Sprite symbolToShow = GetSpriteByIndex(symbolIdx);

            switch (imageIndex)
            {
                case 0:
                    currentRotObj = leftRotatingObject;
                    currentTargetImg = leftImage;
                    break;
                case 1:
                    currentRotObj = centerRotatingObject;
                    currentTargetImg = centerImage;
                    break;
                case 2:
                    currentRotObj = rightRotatingObject;
                    currentTargetImg = rightImage;
                    break;
            }

            if (currentTargetImg != null)
            {
                yield return RotateAndShow(currentRotObj, currentTargetImg, symbolToShow);
                imageIndex++;
            }

            yield return new WaitForSeconds(stayTime);
        }
    }

    IEnumerator RotateAndShow(GameObject rotatingObject, Image targetImage, Sprite targetSprite)
    {
        targetImage.sprite = transparentSymbol;

        int rotations = Random.Range(1, 5);
        float totalAngle = 360f * rotations;

        float time = 0f;
        float duration = 1.2f;

        while (time < duration)
        {
            float t = time / duration;
            float angle = Mathf.Lerp(0f, totalAngle, t);
            rotatingObject.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        rotatingObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        targetImage.sprite = targetSprite;

        threeSymbolsPillarManager.CheckWinPuzzle();
    }

    IEnumerator MoveSmooth(Vector3 target, float smooth)
    {
        velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target,
                ref velocity,
                smooth
            );

            yield return null;
        }

        transform.position = target;
    }

    bool AnyArrayLostActive()
    {
        return GetActiveIndex(leftSymbols0) == -1
            || GetActiveIndex(centerSymbols1) == -1
            || GetActiveIndex(rightSymbols2) == -1;
    }

    void ResetAll()
    {
        imageIndex = 0;
        hasMoved = false;
        leftImage.sprite = transparentSymbol;
        centerImage.sprite = transparentSymbol;
        rightImage.sprite = transparentSymbol;
        transform.position = centerPoint.position;
    }

    int GetActiveIndex(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].activeSelf)
                return i;
        }

        return -1;
    }

    Sprite GetSpriteByIndex(int index)
    {
        return index switch
        {
            0 => spriteSymbol0,
            1 => spriteSymbol1,
            2 => spriteSymbol2,
            _ => transparentSymbol
        };
    }
}
