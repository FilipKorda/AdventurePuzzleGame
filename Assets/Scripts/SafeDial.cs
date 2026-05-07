using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SafeDial : MonoBehaviour
{
    [SerializeField] private InputActionReference safeInputMove;
    [SerializeField] private InputActionReference safeInputReset;

    [SerializeField] private SafePuzzle safePuzzle;

    const float stepAngle = 3.6f;
    int currentValue = 0;

    public bool canRotateDial;
    public float fixedY = 90f;
    public float fixedZ = 90f;
    private float startModelPosition = 54f;

    [System.Serializable]
    public struct DialStep
    {
        public int number;
        public bool clockwise;
    }

    public DialStep[] combination = new DialStep[4]
    {
        new DialStep{number = 13, clockwise = true},
        new DialStep{number = 35, clockwise = false},
        new DialStep{number = 72, clockwise = true},
        new DialStep{number = 5, clockwise = false}
    };

    private int sequenceIndex = 0;

    void Start()
    {
        currentValue = 0;
        float angle = currentValue * stepAngle + startModelPosition;
        transform.rotation = Quaternion.Euler(angle, fixedY, fixedZ);
        // Debug.Log($"Startowa pozycja tarczy: {currentValue}");
    }


    private void OnEnable()
    {
        if (safeInputMove != null)
        {
            safeInputMove.action.Enable();
            safeInputMove.action.performed += OnMoveInput;
        }

        if (safeInputReset != null)
        {
            safeInputReset.action.Enable();
            safeInputReset.action.performed += OnResetInput;
        }
    }

    private void OnDisable()
    {
        if (safeInputMove != null)
        {
            safeInputMove.action.performed -= OnMoveInput;
            safeInputMove.action.Disable();
        }

        if (safeInputReset != null)
        {
            safeInputReset.action.performed -= OnResetInput;
            safeInputReset.action.Disable();
        }
    }


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (canRotateDial)
        {
            float input = context.ReadValue<float>();

            if (input > 0.5f)
                Rotate(1);
            else if (input < 0.5f)
                Rotate(-1);
        }
    }

    private void OnResetInput(InputAction.CallbackContext context)
    {
        if (canRotateDial)
        {
            AnimationResetDial();
        }
    }



    public void InstantResetDial()
    {
        sequenceIndex = 0;
        canRotateDial = false;
        currentValue = 0;
        float angle = currentValue * stepAngle + startModelPosition;
        transform.rotation = Quaternion.Euler(angle, fixedY, fixedZ);
        // Debug.Log($"Startowa pozycja tarczy: {currentValue}");
    }

    void Rotate(int dir)
    {
        currentValue = (currentValue + dir + 100) % 100;
        float angle = currentValue * stepAngle + startModelPosition;
        transform.rotation = Quaternion.Euler(angle, fixedY, fixedZ);

        CheckCombination(dir);

        Services.Audio.PlaySFX("SafeDialClickSound");
    }

    private void AnimationResetDial()
    {
        StartCoroutine(CoroutineAnimationResetDial());
    }

    private IEnumerator CoroutineAnimationResetDial()
    {
        canRotateDial = false;

        float totalRotation = 360f * 3;
        float duration = 1.5f;
        float elapsed = 0f;

        float startX = startModelPosition;
        float endX = startX + totalRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float currentX = Mathf.Lerp(startX, endX, t);
            transform.rotation = Quaternion.Euler(currentX, fixedY, fixedZ);

            yield return null;
        }

        currentValue = 0;
        transform.rotation = Quaternion.Euler(startModelPosition, fixedY, fixedZ);
        canRotateDial = true;
    }


    void CheckCombination(int dir)
    {
        DialStep currentStep = combination[sequenceIndex];

        bool isClockwise = dir > 0;
        if (isClockwise != currentStep.clockwise)
        {
            if (sequenceIndex > 0)
            {
                // Debug.Log("Zły kierunek! Reset sekwencji.");
                sequenceIndex = 0;
            }
            return;
        }

        if (currentValue == currentStep.number)
        {
            sequenceIndex++;
            //  Debug.Log($"Poprawny krok {sequenceIndex}/{combination.Length}");

            if (sequenceIndex >= combination.Length)
            {
                PuzzleWin();
            }
        }
    }

    public void PuzzleWin()
    {
        safePuzzle.WinPuzzle();
        canRotateDial = false;
        //  Debug.Log("win");
    }
}