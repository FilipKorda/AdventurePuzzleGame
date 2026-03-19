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

        if (puzzle0)
        {
            animator0.SetTrigger("Close");
            puzzle0 = false;
        }
        else if (puzzle1)
        {
            animator1.SetTrigger("Close");
            puzzle1 = false;
        }
        else if (puzzle2)
        {
            animator2.SetTrigger("Close");
            puzzle2 = false;
        }
        else
        {
            animator3.SetTrigger("Close");
            puzzle3 = false;
        }



        playerBehaviour.disablePlayer = false;
        playerBehaviour._playerCamera.enabled = true;

        puzzleCamera.enabled = false;
    }
}
