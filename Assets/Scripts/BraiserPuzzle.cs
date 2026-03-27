using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BraiserPuzzle : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private Camera puzzleCamera;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform cameraPoint0;
    [SerializeField] private Transform cameraPoint1;
    [SerializeField] private Transform cameraPoint2;
    [SerializeField] private Transform cameraPoint3;

    [SerializeField] private Animator animator0;
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    [SerializeField] private Animator animator3;

    [SerializeField] private BoxCollider[] allBoxColliders;

    [SerializeField] private UVScrollFromMovement uVScrollFromMovement0;
    [SerializeField] private UVScrollFromMovement uVScrollFromMovement1;
    [SerializeField] private UVScrollFromMovement uVScrollFromMovement2;
    [SerializeField] private UVScrollFromMovement uVScrollFromMovement3;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator hiddenDoorAnimator;


    private bool puzzle0 = false;
    private bool puzzle1 = false;
    private bool puzzle2 = false;
    private bool puzzle3 = false;

    private void ActiveInput()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.Enable();
            thisModeInput.action.performed += OnModePerformed;
        }

    }

    private void DisableInput()
    {
        if (thisModeInput != null)
        {
            thisModeInput.action.performed -= OnModePerformed;
            thisModeInput.action.Disable();
        }
    }

    private void OnModePerformed(InputAction.CallbackContext context)
    {
        ExitPuzzle();
    }

    public void EnterPuzzle0()
    {
        CursorController.Instance.EnableCursor(puzzleCamera);
        uVScrollFromMovement0.isActive = true;

        var boxCollider = uVScrollFromMovement0.GetComponent<BoxCollider>();
        boxCollider.enabled = true;

        uVScrollFromMovement0.PerformScrollUV();
        uVScrollFromMovement0.ActivateRaycast();
        ToogleAllBoxColliders(false);
        puzzle0 = true;
        ActiveInput();
        SetAndRotateCamera0ToTransform();
        StartCameraMovement();

        animator0.SetTrigger("Open");

        playerBehaviour.disablePlayer = true;
    }

    private void SetAndRotateCamera0ToTransform()
    {
        puzzleCamera.transform.SetPositionAndRotation(cameraPoint0.position, cameraPoint0.rotation);
    }

    public void EnterPuzzle1()
    {
        CursorController.Instance.EnableCursor(puzzleCamera);
        uVScrollFromMovement1.isActive = true;

        var boxCollider1 = uVScrollFromMovement1.GetComponent<BoxCollider>();
        boxCollider1.enabled = true;

        uVScrollFromMovement1.PerformScrollUV();
        uVScrollFromMovement1.ActivateRaycast();
        ToogleAllBoxColliders(false);
        puzzle1 = true;
        ActiveInput();
        SetAndRotateCamera1ToTransform();
        StartCameraMovement();

        animator1.SetTrigger("Open");

        playerBehaviour.disablePlayer = true;

    }

    private void SetAndRotateCamera1ToTransform()
    {
        puzzleCamera.transform.SetPositionAndRotation(cameraPoint1.position, cameraPoint1.rotation);
    }

    public void EnterPuzzle2()
    {
        CursorController.Instance.EnableCursor(puzzleCamera);
        uVScrollFromMovement2.isActive = true;

        var boxCollider2 = uVScrollFromMovement2.GetComponent<BoxCollider>();
        boxCollider2.enabled = true;

        uVScrollFromMovement2.PerformScrollUV();
        uVScrollFromMovement2.ActivateRaycast();
        ToogleAllBoxColliders(false);
        puzzle2 = true;
        ActiveInput();
        SetAndRotateCamera2ToTransform();
        StartCameraMovement();

        animator2.SetTrigger("Open");

        playerBehaviour.disablePlayer = true;

    }

    private void SetAndRotateCamera2ToTransform()
    {
        puzzleCamera.transform.SetPositionAndRotation(cameraPoint2.position, cameraPoint2.rotation);
    }

    public void EnterPuzzle3()
    {
        CursorController.Instance.EnableCursor(puzzleCamera);
        uVScrollFromMovement3.isActive = true;

        var boxCollider3 = uVScrollFromMovement3.GetComponent<BoxCollider>();
        boxCollider3.enabled = true;

        uVScrollFromMovement3.PerformScrollUV();
        uVScrollFromMovement3.ActivateRaycast();
        ToogleAllBoxColliders(false);
        puzzle3 = true;
        ActiveInput();
        SetAndRotateCamera3ToTransform();
        StartCameraMovement();

        animator3.SetTrigger("Open");

        playerBehaviour.disablePlayer = true;

    }

    private void SetAndRotateCamera3ToTransform()
    {
        puzzleCamera.transform.SetPositionAndRotation(cameraPoint3.position, cameraPoint3.rotation);
    }

    private void StartCameraMovement()
    {
        StartCoroutine(CouturineMakeCameraMovement());
    }

    private IEnumerator CouturineMakeCameraMovement()
    {
        Camera cam = playerBehaviour._playerCamera;

        Vector3 startPos = cam.transform.position;
        Quaternion startRot = cam.transform.rotation;

        Vector3 targetPos = puzzleCamera.transform.position;
        Quaternion targetRot = puzzleCamera.transform.rotation;

        float time = 0f;
        float duration = 1.2f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        cam.transform.position = targetPos;
        cam.transform.rotation = targetRot;

        cam.enabled = false;

        yield return new WaitForSeconds(0.1f);

        puzzleCamera.enabled = true;

        yield return new WaitForSeconds(0.1f);

        cam.transform.position = startPos;
        cam.transform.rotation = startRot;
    }

    public void ExitPuzzle()
    {
        DisableInput();

        CursorController.Instance.DisableCursor();

        ToogleAllBoxColliders(true);

        if (puzzle0)
        {
            animator0.SetTrigger("Close");
            uVScrollFromMovement0.isActive = false;
            puzzle0 = false;

        }
        else if (puzzle1)
        {
            animator1.SetTrigger("Close");
            uVScrollFromMovement1.isActive = false;
            puzzle1 = false;

        }
        else if (puzzle2)
        {
            animator2.SetTrigger("Close");
            uVScrollFromMovement2.isActive = false;
            puzzle2 = false;

        }
        else
        {
            animator3.SetTrigger("Close");
            uVScrollFromMovement3.isActive = false;
            puzzle3 = false;

        }

        playerBehaviour.disablePlayer = false;
        playerBehaviour._playerCamera.enabled = true;

        puzzleCamera.enabled = false;
    }

    private void ToogleAllBoxColliders(bool toogle)
    {
        foreach (var boxCollider in allBoxColliders)
        {
            boxCollider.enabled = toogle;
        }
    }

    public void PuzzleWin()
    {
        if (uVScrollFromMovement0.goodSymbolSelected && uVScrollFromMovement1.goodSymbolSelected &&
        uVScrollFromMovement2.goodSymbolSelected && uVScrollFromMovement3.goodSymbolSelected)
        {
            DisableInput();

            CursorController.Instance.DisableCursor();

            if (puzzle0)
            {
                animator0.SetTrigger("Close");
                uVScrollFromMovement0.isActive = false;
                puzzle0 = false;
            }
            else if (puzzle1)
            {
                animator1.SetTrigger("Close");
                uVScrollFromMovement1.isActive = false;
                puzzle1 = false;
            }
            else if (puzzle2)
            {
                animator2.SetTrigger("Close");
                uVScrollFromMovement2.isActive = false;
                puzzle2 = false;
            }
            else
            {
                animator3.SetTrigger("Close");
                uVScrollFromMovement3.isActive = false;
                puzzle3 = false;
            }

            ToogleAllBoxColliders(false);

            uVScrollFromMovement0.enabled = false;
            uVScrollFromMovement1.enabled = false;
            uVScrollFromMovement2.enabled = false;
            uVScrollFromMovement3.enabled = false;

            var boxCollider0 = uVScrollFromMovement0.GetComponent<BoxCollider>();
            boxCollider0.enabled = false;

            var boxCollider1 = uVScrollFromMovement1.GetComponent<BoxCollider>();
            boxCollider1.enabled = false;

            var boxCollider2 = uVScrollFromMovement2.GetComponent<BoxCollider>();
            boxCollider2.enabled = false;

            var boxCollider3 = uVScrollFromMovement3.GetComponent<BoxCollider>();
            boxCollider3.enabled = false;

            animator.SetTrigger("Open");
            hiddenDoorAnimator.SetTrigger("Open");

            playerBehaviour.disablePlayer = false;
            playerBehaviour._playerCamera.enabled = true;

            puzzleCamera.enabled = false;
        }
    }

}
