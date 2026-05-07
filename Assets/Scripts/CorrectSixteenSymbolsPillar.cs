using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorrectSixteenSymbolsPillar : MonoBehaviour
{
    [SerializeField] private InputActionReference thisModeInput;
    [SerializeField] private Camera puzzleCamera;
    //[SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private BoxCollider boxCollider;


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

    public void EnterPuzzle()
    {
        boxCollider.enabled = false;

        //LPM to click
        UIManager.Instance.EnableLpmToClickPanel();

        CursorController.Instance.EnableCursor(puzzleCamera);

        ActiveInput();
        SetAndRotateCameraToTransform();
        StartCameraMovement();

        PlayerControlManager.Instance.LockPlayer();
    }


    private void OnModePerformed(InputAction.CallbackContext context)
    {
        ExitPuzzle();
    }

    public void ExitPuzzle()
    {

        DisableInput();
        UIManager.Instance.DisableSharedPanelText();
        CursorController.Instance.DisableCursor();

        SquareSelectionManager.Instance.canSelect = false;

        boxCollider.enabled = true;

        PlayerControlManager.Instance.UnlockPlayer();
        PlayerControlManager.Instance.EnableCamera();


        puzzleCamera.enabled = false;
    }

    public void ExitAfterWin()
    {
        DisableInput();
        UIManager.Instance.DisableSharedPanelText();
        CursorController.Instance.DisableCursor();

        boxCollider.enabled = false;

        PlayerControlManager.Instance.UnlockPlayer();
        PlayerControlManager.Instance.EnableCamera();

        puzzleCamera.enabled = false;
    }

    private void StartCameraMovement()
    {
        StartCoroutine(CouturineMakeCameraMovement());
    }

    private IEnumerator CouturineMakeCameraMovement()
    {
        Camera cam = PlayerControlManager.Instance.GetPlayerCamera();

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

        SquareSelectionManager.Instance.canSelect = true;
    }

    private void SetAndRotateCameraToTransform()
    {
        puzzleCamera.transform.SetPositionAndRotation(cameraPoint.position, cameraPoint.rotation);
    }
}
