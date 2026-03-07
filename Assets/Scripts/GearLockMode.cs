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
    private bool riddleIsSolved = false;

    private void OnEnable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.Enable();
            gearModeInput.action.performed += OnGearModePerformed;
        }
    }

    private void OnDisable()
    {
        if (gearModeInput != null)
        {
            gearModeInput.action.performed -= OnGearModePerformed;
            gearModeInput.action.Disable();
        }
    }

    private void SpawnThisObjectInFronOfPlayer()
    {
        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var cam = playerBehaviour._playerCamera.transform;

        var position = cam.position
                     + cam.forward * distanceFromCamera
                     + cam.up * verticalOffset;

        var rotation = Quaternion.LookRotation(cam.forward) * Quaternion.Euler(-90f, 0f, 0f);

        transform.SetPositionAndRotation(position, rotation);
    }

    private void OnGearModePerformed(InputAction.CallbackContext context)
    {
        ExitGearLockMode();
    }

    public void EnterGearLockMode()
    {
        SpawnThisObjectInFronOfPlayer();
        blurCanvas.gameObject.SetActive(true);
        gameObject.SetActive(true);
        doorBoxCollider.enabled = false;
        playerBehaviour.disableOnlyMovement = true;
        UIManager.Instance.EnableGearModePanel();
    }

    public void ExitGearLockMode()
    {
        if (!riddleIsSolved)
        {
            doorBoxCollider.enabled = true;

            blurCanvas.gameObject.SetActive(false);

            foreach (GameObject gear in gears)
            {
                var item = gear.GetComponent<InteractableItem>();
                if (item != null)
                {
                    item.ResetCurrentGearIndex();
                }
            }

            ResetGears();
            gameObject.SetActive(false);
            playerBehaviour.disableOnlyMovement = false;
            UIManager.Instance.DisableGearModePanel();
        }
        else
        {
            animator.SetTrigger("Open");

            blurCanvas.gameObject.SetActive(false);
            ResetGears();
            gameObject.SetActive(false);
            playerBehaviour.disableOnlyMovement = false;
            UIManager.Instance.DisableGearModePanel();
        }
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

        Debug.Log("Puzzle solved!");

        riddleIsSolved = true;
        ExitGearLockMode();
    }

}
