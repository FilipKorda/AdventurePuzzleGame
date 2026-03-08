using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearLockMode : MonoBehaviour
{
    [SerializeField] private InputActionReference gearModeInput;
    [SerializeField] private BoxCollider doorBoxCollider;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Canvas blurCanvas;
    [SerializeField] private GameObject[] gears;
    [SerializeField] private float distanceFromCamera = 1f;
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private int[] correctGearIndexes;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform point;

    private void OnEnable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.Enable();
            gearModeInput.action.performed += OnGearModePerformed;
        }

        StartCoroutine(DisablePlayerLook());
    }

    private void OnDisable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.performed -= OnGearModePerformed;
            gearModeInput.action.Disable();
        }
    }

    private void MovePlayerToPosition()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        if (playerBehaviour.TryGetComponent<CharacterController>(out var controller))
            controller.enabled = false;

        player.SetPositionAndRotation(point.position, point.rotation);

        if (controller != null)
            controller.enabled = true;
    }

    private IEnumerator DisablePlayerLook()
    {
        yield return null;

        playerBehaviour.disablePlayer = true;

        playerBehaviour.ResetCameraRotation();

        yield return new WaitForSeconds(0.1f);

        playerBehaviour.disablePlayer = false;
    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var position = player.position
                     + player.forward * distanceFromCamera
                     + player.up * verticalOffset;

        transform.SetPositionAndRotation(position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private void OnGearModePerformed(InputAction.CallbackContext context)
    {
        ExitGearLockMode();
    }

    public void EnterGearLockMode()
    {
        MovePlayerToPosition();
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        doorBoxCollider.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableGearModePanel();
    }

    public void ExitGearLockMode()
    {
        doorBoxCollider.enabled = true;

        blurCanvas.gameObject.SetActive(false);

        foreach (GameObject gear in gears)
        {
            if (gear.TryGetComponent<InteractableItem>(out var item))
            {
                item.ResetCurrentGearIndex();
            }
        }

        ResetGears();
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
    }

    private void ResetGears()
    {
        foreach (GameObject gear in gears)
        {
            gear.transform.localRotation = Quaternion.identity;
        }

    }

    public void CheckIfPuzzleSolved()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            int current = gears[i].GetComponent<InteractableItem>().CurrentGearIndex;
            int correct = correctGearIndexes[i];

            Debug.Log("Gear " + i + " current: " + current + " correct: " + correct);

            if (current != correct)
            {
                Debug.Log("Puzzle not solved - wrong gear at index " + i);
                return;
            }
        }

        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        StartCoroutine(CourutineSolvePuzzle());
    }

    private IEnumerator CourutineSolvePuzzle()
    {
        Debug.Log("Puzzle solved!");

        yield return StartCoroutine(RotateGearMechanism());

        Services.Audio.PlaySFX("AfterGearPuzzleSolved");

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("Open");
        blurCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false);
        playerBehaviour.disableOnlyMovement = false;
        UIManager.Instance.DisableGearModePanel();
    }


    private IEnumerator RotateGearMechanism()
    {
        int steps = gears.Length;

        for (int step = 0; step < steps; step++)
        {
            for (int i = 0; i <= step; i++)
            {
                if (gears[i].TryGetComponent<InteractableItem>(out var item))
                {
                    item.RotateGear();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

}
