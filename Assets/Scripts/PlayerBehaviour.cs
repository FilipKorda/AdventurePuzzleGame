using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Sensitivity Settings")]
    [SerializeField] private float gamepadSensitivity = 100f;
    [SerializeField] private float mouseSensitivity = 10f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    private float gravity = -9.81f;
    [SerializeField] private float climbSpeed = 3f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float minLookAngle = -60f;
    [SerializeField] private float maxLookAngle = 60f;

    [Header("Raycast Detector")]
    [SerializeField] private float raycastRange = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Center Of Screen Dot")]
    [SerializeField] private GameObject centerOfScreen;
    [SerializeField] private GameObject dot;
    private Vector2 centerOfScreenTargetSize = new(10f, 10f);
    private float centerOfScreenScaleSpeed = 5f;

    private IPickupable lastIpickupable;
    private IBookThrowable lastIBookThrowable;
    private IOpenable lastIOpenable;
    private IReadable lastIReadable;
    private IReadableAndInteractable lastIReadableAndInteractable;
    private IPressable lastIPressable;
    private IPlaceable lastIPlaceable;
    private IRecipePlaceable lastIRecipePlaceable;
    private ILockPick lastILockPick;
    private IFillable lastIFillable;
    private IPickupARenewableItem lastIPickupARenewableItem;
    private IAlchemyStation lastIAlchemyStation;
    private IGetObject lastIGetObject;
    private ICrafting lastICrafting;

    private IPinNumber lastIPinNumber;
    private IPinNumber highlightedPin;

    private IRotate lastIRotate;

    private ICryptex lastICryptex;

    private CharacterController characterController;
    private Vector2 inputMovement;
    private Vector2 inputLook;
    private Vector3 velocity;

    private float cameraVerticalRotation = 0f;
    private Inventory playerInventory;
    private bool isClimbing = false;

    private bool isApplyNiceWaterActive = false;
    private bool isAngryTimeActive = false;

    [Header("Drunk Effect Settings")]
    [Tooltip("Jak bardzo kamera chwieje siê na boki. Powinna byæ to ma³a wartoœæ.")]
    [SerializeField] private float drunkSwayAmount = 0.5f;
    [Tooltip("Jak WOLNO kamera chwieje siê na boki. Niska wartoœæ = wolne bujanie.")]
    [SerializeField] private float drunkSwaySpeed = 0.15f;
    [Tooltip("Jak bardzo ruch gracza jest 'chwiejny'.")]
    [SerializeField] private float drunkMovementStaggerAmount = 0.2f;
    [Tooltip("Jak WOLNO zmienia siê kierunek chwiania siê. Niska wartoœæ = powolne zmiany.")]
    [SerializeField] private float drunkMovementStaggerSpeed = 0.1f;
    [Header("Drunk Sluggishness Settings")]
    [Tooltip("Mno¿nik prêdkoœci w stanie upojenia (np. 0.6 to 60% normalnej prêdkoœci).")]
    [Range(0.1f, 1f)]
    [SerializeField] private float drunkMoveSpeedMultiplier = 0.6f;
    [Tooltip("Jak bardzo opóŸniony/ociê¿a³y jest ruch. Ni¿sze wartoœci = wiêksze opóŸnienie.")]
    [SerializeField] private float drunkMovementSmoothing = 4f;
    [Tooltip("Jak bardzo opóŸnione/ociê¿a³e jest rozgl¹danie siê. Ni¿sze wartoœci = wiêksze opóŸnienie.")]
    [SerializeField] private float drunkLookSmoothing = 3f;
    [Header("Drunk Visual Effects")]
    [SerializeField] private Camera _playerCamera;
    [Tooltip("Normalne pole widzenia kamery.")]
    [SerializeField] private float normalFOV = 60f;
    [Tooltip("Pole widzenia kamery podczas efektu upojenia.")]
    [SerializeField] private float drunkFOV = 50f;
    [Tooltip("Prêdkoœæ, z jak¹ zmienia siê pole widzenia.")]
    [SerializeField] private float fovChangeSpeed = 2f;
    [Header("Drunk Blur Effect")]
    [Tooltip("Przypisz tutaj obiekt 'Post-Process Volume' ze sceny.")]
    [SerializeField] private Volume postProcessVolume;
    [Tooltip("Minimalna odleg³oœæ, na której skupia siê wzrok (w metrach).")]
    [SerializeField] private float minFocusDistance = 0.1f;
    [Tooltip("Maksymalna odleg³oœæ, na której skupia siê wzrok (w metrach).")]
    [SerializeField] private float maxFocusDistance = 10f;
    [Tooltip("Prêdkoœæ, z jak¹ zmienia siê odleg³oœæ ogniskowania (p³ywanie wzroku).")]
    [SerializeField] private float focusChangeSpeed = 0.5f;

    [Header("Acid Visual Effect")]
    [Tooltip("Kolor filtra nak³adanego na ekran podczas efektu kwasu.")]
    [SerializeField] private Color acidColorFilter = new Color(0.1f, 0.9f, 0.2f, 1f);
    [Tooltip("Docelowa wartoœæ nasycenia (Saturation) podczas efektu kwasu.")]
    [SerializeField] private float acidSaturationTarget = 100f;
    [SerializeField] private float acidContrastTarget = -30f;
    [Tooltip("Prêdkoœæ, z jak¹ zmieniaj¹ siê efekty wizualne kwasu.")]
    [SerializeField] private float acidEffectChangeSpeed = 1.5f;

    [Header("Flying Effect")]
    [SerializeField] private float flySpeed = 5f;
    public float playerSpeed = 5.0f;
    public float gravityValue = -9.81f;
    private bool isFlying = false;

    [Header("Teleport Effect")]
    [Tooltip("Punkty, miêdzy którymi gracz bêdzie siê teleportowa³.")]
    [SerializeField] private Transform[] teleportPoints;
    [Tooltip("Odstêp czasowy miêdzy teleportacjami.")]
    [SerializeField] private float teleportInterval = 0.75f;

    [Header("Open Door Effect")]
    [SerializeField] private Renderer hiddenDoorRenderer;
    [SerializeField] private BoxCollider boxColliderHiddenDoor;

    private Color _defaultColorFilter = Color.white;

    private Vector2 _smoothedDrunkMoveInput;
    private Vector2 _smoothedDrunkLookInput;
    private bool isDrunk = false;
    private bool isAcidEffectActive = false;
    private Coroutine activeDrunkCoroutine;
    private Coroutine activeAcidCoroutine;
    private Coroutine activeNiceWaterCoroutine;
    private Coroutine activeMudWaterCoroutine;
    private Coroutine activeLeafsGoodsCoroutine;
    private Coroutine activeAngryTimeCoroutine;
    private Coroutine activeTeleportCoroutine;
    private Coroutine hiddenDoorCoroutine;

    private DepthOfField _depthOfFieldEffect;
    private ColorAdjustments _colorAdjustmentsEffect;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    public bool disablePlayer = false;

    [Header("Light Lamp")]
    [SerializeField] private Light lampLight;
    [SerializeField] private GameObject lampLightGo;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned!");
        }

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGet(out _depthOfFieldEffect);
            postProcessVolume.profile.TryGet(out _colorAdjustmentsEffect);

            postProcessVolume.profile.TryGet(out chromaticAberration);
            postProcessVolume.profile.TryGet(out lensDistortion);
        }
        else
        {
            Debug.LogWarning("Nie przypisano Post-Process Volume do PlayerBehaviour! Efekty wizualne nie bêd¹ dzia³aæ.");
        }
    }

    private void Start()
    {
        if (!TryGetComponent(out playerInventory))
        {
            playerInventory = gameObject.AddComponent<Inventory>();
        }

        if (_depthOfFieldEffect != null) _depthOfFieldEffect.active = false;
        if (_colorAdjustmentsEffect != null) _colorAdjustmentsEffect.active = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void ToggleLamp(InputAction.CallbackContext context)
    {
        if (lampLightGo.activeInHierarchy) return;

        if (lampLight != null)
        {
            lampLight.enabled = !lampLight.enabled;
        }
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        if (disablePlayer) { return; }
        if (lastILockPick != null && lastILockPick.IsLockPicking()) return;
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        if (lastIReadableAndInteractable != null && lastIReadableAndInteractable.IsReadingInteractable()) return;
        inputMovement = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (disablePlayer) { return; }
        if (lastILockPick != null && lastILockPick.IsLockPicking()) return;
        if (lastIReadable != null && lastIReadable.IsReading()) return;
        if (lastIReadableAndInteractable != null && lastIReadableAndInteractable.IsReadingInteractable()) return;

        Vector2 rawInput = context.ReadValue<Vector2>();

        if (context.control.device is Mouse)
        {
            inputLook = MouseSensitivitySettings.MouseSensitivity * mouseSensitivity * Time.deltaTime * rawInput;
        }
        else
        {
            inputLook = MouseSensitivitySettings.MouseSensitivity * gamepadSensitivity * Time.deltaTime * rawInput;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (disablePlayer) { return; }

        if (context.performed)
        {
            lastIpickupable?.OnPickUp();
            lastIBookThrowable?.OnBookThrow();
            lastIPressable?.OnPress();
            lastIPlaceable?.PlaceObject();
            lastIRecipePlaceable?.PlaceRecipeObject();

            lastIAlchemyStation?.AddIngredient();

            lastIFillable?.OnFill();
            lastIPickupARenewableItem?.OnPickupARenewableItem();
            lastIGetObject?.GetObject();
            lastICrafting?.PlaceObjectToCraft();

            lastIPinNumber?.EnterPinNumber();

            lastIRotate?.RotateStatue();

            lastICryptex?.RotateCryptex();

            if (lastIOpenable != null && lastIOpenable.IsOpen())
            {
                lastIOpenable?.CloseObject();
            }
            else
            {
                lastIOpenable?.OpenObject();
            }

            if (lastIReadable != null && lastIReadable.IsReading())
            {
                lastIReadable?.OnStopRead();
            }
            else
            {
                lastIReadable?.OnRead();
            }

            if (lastILockPick != null && lastILockPick.IsLockPicking())
            {
                lastILockPick?.StopLockPicking();
            }
            else
            {
                lastILockPick?.StartLockPick();
            }

            if (lastIReadableAndInteractable != null && lastIReadableAndInteractable.IsReadingInteractable())
            {
                lastIReadableAndInteractable?.OnStopReadInteractable();
            }
            else
            {
                lastIReadableAndInteractable?.OnReadInteractable();
            }


        }
    }

    private void Update()
    {
        HandleFOV();
        HandleRaycast();
        HandleMovement();
        HandleLook();
        ApplyGravity();
        HandleClimbing();
        HandleDrunkBlur();
    }

    private void HandleFOV()
    {
        if (disablePlayer) { return; }
        if (_playerCamera == null && !isDrunk) return;

        float targetFOV = isDrunk ? drunkFOV : normalFOV;

        _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
    }

    private void HandleMovement()
    {
        if (disablePlayer) { return; }
        if (isClimbing) { return; }

        Vector2 finalMoveInput = inputMovement;
        float finalMoveSpeed = moveSpeed;

        if (isDrunk)
        {
            _smoothedDrunkMoveInput = Vector2.Lerp(_smoothedDrunkMoveInput, inputMovement, drunkMovementSmoothing * Time.deltaTime);
            finalMoveInput = _smoothedDrunkMoveInput;

            finalMoveSpeed *= drunkMoveSpeedMultiplier;
        }

        Vector3 moveDirection = transform.right * finalMoveInput.x + transform.forward * finalMoveInput.y;

        if (isDrunk)
        {
            float staggerX = (Mathf.PerlinNoise(Time.time * drunkMovementStaggerSpeed, 0) * 2 - 1) * drunkMovementStaggerAmount;
            float staggerZ = (Mathf.PerlinNoise(0, Time.time * drunkMovementStaggerSpeed) * 2 - 1) * drunkMovementStaggerAmount;
            moveDirection += new Vector3(staggerX, 0, staggerZ);
        }

        characterController.Move(finalMoveSpeed * Time.deltaTime * moveDirection);
    }

    private void HandleLook()
    {
        if (disablePlayer) { return; }
        Vector2 finalLookInput = inputLook;

        if (isDrunk)
        {
            _smoothedDrunkLookInput = Vector2.Lerp(_smoothedDrunkLookInput, inputLook, drunkLookSmoothing * Time.deltaTime);
            finalLookInput = _smoothedDrunkLookInput;

            float swayX = Mathf.Sin(Time.time * drunkSwaySpeed) * drunkSwayAmount;
            float swayY = Mathf.Cos(Time.time * drunkSwaySpeed * 0.7f) * drunkSwayAmount;

            finalLookInput += new Vector2(swayX, swayY) * Time.deltaTime;
        }

        float mouseX = finalLookInput.x;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = finalLookInput.y;
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, minLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (disablePlayer) { return; }
        if (isClimbing || isFlying)
        {
            velocity.y = 0;
            return;
        }

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleRaycast()
    {
        if (disablePlayer) return;

        Ray ray = new(cameraTransform.position, cameraTransform.forward);

        lastIpickupable = null;
        lastIBookThrowable = null;
        lastIOpenable = null;
        lastIReadable = null;
        lastIPressable = null;
        lastIPlaceable = null;
        lastIRecipePlaceable = null;
        lastILockPick = null;
        lastIFillable = null;
        lastIPickupARenewableItem = null;
        lastIAlchemyStation = null;
        lastIReadableAndInteractable = null;
        lastIGetObject = null;
        lastICrafting = null;
        lastIPinNumber = null;
        lastIRotate = null;
        lastICryptex = null;

        if (Physics.Raycast(ray, out RaycastHit hit, raycastRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent<InteractableItem>(out var interactableObject))
            {
                if (highlightedPin != null &&
                    interactableObject.interactableType != InteractableItem.InteractableType.PinNumber)
                {
                    highlightedPin.ResetHighlightButton();
                    highlightedPin = null;
                }

                switch (interactableObject.interactableType)
                {
                    case InteractableItem.InteractableType.Pickupable:
                        lastIpickupable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Throwable:
                        lastIBookThrowable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Openable:
                        lastIOpenable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Readable:
                        lastIReadable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Pressable:
                        lastIPressable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Placeable:
                        lastIPlaceable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.LockPick:
                        lastILockPick = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Fillable:
                        lastIFillable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.PickupARenewableItem:
                        lastIPickupARenewableItem = interactableObject;
                        break;

                    case InteractableItem.InteractableType.AlchemyStation:
                        lastIAlchemyStation = interactableObject;
                        break;

                    case InteractableItem.InteractableType.ReadableAndInteractableItem:
                        lastIReadableAndInteractable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.PlaceRecipe:
                        lastIRecipePlaceable = interactableObject;
                        break;

                    case InteractableItem.InteractableType.GetObject:
                        lastIGetObject = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Crafting:
                        lastICrafting = interactableObject;
                        break;

                    case InteractableItem.InteractableType.PinNumber:
                        if (hit.collider.TryGetComponent<IPinNumber>(out var pin))
                        {
                            if (highlightedPin != pin)
                            {
                                highlightedPin?.ResetHighlightButton();
                                highlightedPin = pin;
                                highlightedPin.HighlightButton();
                            }
                        }

                        lastIPinNumber = interactableObject;
                        break;

                    case InteractableItem.InteractableType.RotateStatue:
                        lastIRotate = interactableObject;
                        break;

                    case InteractableItem.InteractableType.Cryptex:
                        lastICryptex = interactableObject;
                        break;



                }
            }

            UpdateDotVisibility(false);
            centerOfScreenTargetSize = new Vector2(20f, 20f);
        }
        else
        {
            highlightedPin?.ResetHighlightButton();
            highlightedPin = null;

            UpdateDotVisibility(true);
            centerOfScreenTargetSize = new Vector2(10f, 10f);
        }

        LerpCenterOfScreenSize();
    }

    private void HandleClimbing()
    {
        if (disablePlayer) { return; }
        if (!isClimbing) { return; }

        if (characterController.isGrounded && inputMovement.y < -0.1f)
        {
            isClimbing = false;
            return;
        }


        Vector3 climbDirection = new Vector3(0, inputMovement.y, 0);
        characterController.Move(climbSpeed * Time.deltaTime * climbDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disablePlayer) { return; }
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }

    private void UpdateDotVisibility(bool isVisible)
    {
        dot.SetActive(isVisible);
    }

    private void LerpCenterOfScreenSize()
    {
        RectTransform rectTransform = centerOfScreen.GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, centerOfScreenTargetSize, centerOfScreenScaleSpeed * Time.deltaTime);
    }

    #region DrunkEffect
    public void ApplyDrunkEffect(float duration)
    {
        if (activeDrunkCoroutine != null) StopCoroutine(activeDrunkCoroutine);
        activeDrunkCoroutine = StartCoroutine(DrunkCoroutine(duration));
    }

    private IEnumerator DrunkCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        isDrunk = true;
        if (_depthOfFieldEffect != null) _depthOfFieldEffect.active = true;

        Debug.Log("Efekt pijañstwa aktywny na " + duration + " sekund.");
        yield return new WaitForSeconds(duration);

        isDrunk = false;

        if (_depthOfFieldEffect != null) _depthOfFieldEffect.active = false;

        activeDrunkCoroutine = null;
        Debug.Log("Efekt pijañstwa zakoñczony.");
    }
    private void HandleDrunkBlur()
    {
        if (_depthOfFieldEffect == null && !isDrunk) return;

        float sineWave = (Mathf.Sin(Time.time * focusChangeSpeed) + 1) / 2.0f;

        float newFocusDistance = Mathf.Lerp(minFocusDistance, maxFocusDistance, sineWave);

        _depthOfFieldEffect.focusDistance.value = newFocusDistance;
    }

    #endregion

    #region AcidEffect
    public void ApplyAcidEffect(float duration)
    {
        if (activeAcidCoroutine != null) StopCoroutine(activeAcidCoroutine);
        activeAcidCoroutine = StartCoroutine(AcidCoroutine(duration));
    }

    private IEnumerator AcidCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        if (_colorAdjustmentsEffect != null) _colorAdjustmentsEffect.active = true;
        isAcidEffectActive = true;

        float startSaturation = _colorAdjustmentsEffect.saturation.value;
        float startContrast = _colorAdjustmentsEffect.contrast.value;
        Color startColor = _colorAdjustmentsEffect.colorFilter.value;

        float originalGamepadSensitivity = gamepadSensitivity;
        float originalMouseSensitivity = mouseSensitivity;
        float originalMoveSpeed = moveSpeed;

        gamepadSensitivity /= 2f;
        mouseSensitivity /= 2f;
        moveSpeed /= 2f;

        float transitionDuration = acidEffectChangeSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            _colorAdjustmentsEffect.saturation.value = Mathf.Lerp(startSaturation, acidSaturationTarget, t);
            _colorAdjustmentsEffect.contrast.value = Mathf.Lerp(startContrast, acidContrastTarget, t);
            _colorAdjustmentsEffect.colorFilter.value = Color.Lerp(startColor, acidColorFilter, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _colorAdjustmentsEffect.saturation.value = acidSaturationTarget;
        _colorAdjustmentsEffect.contrast.value = acidContrastTarget;
        _colorAdjustmentsEffect.colorFilter.value = acidColorFilter;

        yield return new WaitForSeconds(duration);

        gamepadSensitivity = originalGamepadSensitivity;
        mouseSensitivity = originalMouseSensitivity;
        moveSpeed = originalMoveSpeed;

        elapsedTime = 0f;
        startSaturation = _colorAdjustmentsEffect.saturation.value;
        startContrast = _colorAdjustmentsEffect.contrast.value;
        startColor = _colorAdjustmentsEffect.colorFilter.value;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            _colorAdjustmentsEffect.saturation.value = Mathf.Lerp(startSaturation, 0f, t);
            _colorAdjustmentsEffect.contrast.value = Mathf.Lerp(startContrast, 0f, t);
            _colorAdjustmentsEffect.colorFilter.value = Color.Lerp(startColor, _defaultColorFilter, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _colorAdjustmentsEffect.saturation.value = 0f;
        _colorAdjustmentsEffect.contrast.value = 0f;
        _colorAdjustmentsEffect.colorFilter.value = _defaultColorFilter;
        if (_colorAdjustmentsEffect != null) _colorAdjustmentsEffect.active = false;

        isAcidEffectActive = false;
        activeAcidCoroutine = null;
    }

    #endregion

    #region NiceWaterEffect
    public void ApplyNiceWaterEffect(float duration)
    {
        if (activeNiceWaterCoroutine != null) StopCoroutine(activeNiceWaterCoroutine);
        activeNiceWaterCoroutine = StartCoroutine(NiceWaterCoroutine(duration));
    }
    private IEnumerator NiceWaterCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        if (chromaticAberration != null) chromaticAberration.active = true;
        if (lensDistortion != null) lensDistortion.active = true;

        float originalMoveSpeed = moveSpeed;
        float originalClimbSpeed = climbSpeed;

        climbSpeed *= 2f;
        moveSpeed *= 2f;

        float transitionTime = 1f;
        float timer = 0f;

        while (timer < transitionTime)
        {
            float progress = timer / transitionTime;

            if (chromaticAberration != null) chromaticAberration.intensity.value = Mathf.Lerp(0, 1f, progress);
            if (lensDistortion != null) lensDistortion.intensity.value = Mathf.Lerp(0, -0.4f, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        if (chromaticAberration != null) chromaticAberration.intensity.value = 1f;
        if (lensDistortion != null) lensDistortion.intensity.value = -0.5f;

        yield return new WaitForSeconds(duration - (transitionTime * 2));

        timer = 0f;
        while (timer < transitionTime)
        {
            float progress = timer / transitionTime;

            if (chromaticAberration != null) chromaticAberration.intensity.value = Mathf.Lerp(1f, 0, progress);
            if (lensDistortion != null) lensDistortion.intensity.value = Mathf.Lerp(-0.5f, 0, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        if (chromaticAberration != null) chromaticAberration.intensity.value = 0f;
        if (lensDistortion != null) lensDistortion.intensity.value = 0f;

        climbSpeed = originalClimbSpeed;
        moveSpeed = originalMoveSpeed;

        activeNiceWaterCoroutine = null;

        if (chromaticAberration != null) chromaticAberration.active = false;
        if (lensDistortion != null) lensDistortion.active = false;
    }
    #endregion

    #region No Effect

    public void NoEffect()
    {
        Debug.LogWarning("No Effect");
    }

    #endregion

    #region Mud Water Effect

    public void MudWaterEffect(float duration)
    {
        if (activeMudWaterCoroutine != null) StopCoroutine(activeMudWaterCoroutine);
        activeMudWaterCoroutine = StartCoroutine(MudWaterCoroutine(duration));
    }

    private IEnumerator MudWaterCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform nie jest przypisany!");
            yield break;
        }

        float transitionTime = 1f;

        Vector3 originalScale = playerTransform.localScale;
        Vector3 targetScale = originalScale / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            playerTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.localScale = targetScale;

        yield return new WaitForSeconds(duration);

        elapsedTime = 0f;

        if (playerTransform != null)
        {
            Vector3 currentScale = playerTransform.localScale;

            while (elapsedTime < transitionTime)
            {
                playerTransform.localScale = Vector3.Lerp(currentScale, originalScale, elapsedTime / transitionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTransform.localScale = originalScale;
        }
    }
    #endregion

    #region LeafGoodsEffect
    public void LeafGoods(float duration)
    {
        if (activeLeafsGoodsCoroutine != null) StopCoroutine(activeLeafsGoodsCoroutine);
        activeLeafsGoodsCoroutine = StartCoroutine(LeafGoodCoroutine(duration));
    }

    private IEnumerator LeafGoodCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform nie jest przypisany!");
            yield break;
        }

        float transitionTime = 0.5f;

        Vector3 originalScale = playerTransform.localScale;
        Vector3 targetScale = originalScale * 2f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            playerTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.localScale = targetScale;

        yield return new WaitForSeconds(duration);

        elapsedTime = 0f;

        if (playerTransform != null)
        {
            Vector3 currentScale = playerTransform.localScale;

            while (elapsedTime < transitionTime)
            {
                playerTransform.localScale = Vector3.Lerp(currentScale, originalScale, elapsedTime / transitionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTransform.localScale = originalScale;
        }
    }
    #endregion

    #region Angry Time Effect
    public void ApplyAngryTime(float duration)
    {
        if (activeAngryTimeCoroutine != null) StopCoroutine(activeAngryTimeCoroutine);
        activeAngryTimeCoroutine = StartCoroutine(AngryTimeCoroutine(duration));
    }

    private IEnumerator AngryTimeCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        isFlying = true;
        velocity.y = 0;

        float timer = 0f;

        while (timer < duration)
        {
            Vector3 flyMovement = Vector3.up * flySpeed;
            characterController.Move(flyMovement * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        isFlying = false;
        activeAngryTimeCoroutine = null;
    }
    #endregion

    #region BadMoodEffect

    public void BadMoodTeleport(float duration)
    {
        if (activeTeleportCoroutine != null) StopCoroutine(activeTeleportCoroutine);

        activeTeleportCoroutine = StartCoroutine(TeleportCoroutine(duration));
    }

    private IEnumerator TeleportCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("MagicAfterDrink");
        float timer = 0f;
        int currentPointIndex = 0;

        while (timer < duration)
        {
            Transform targetPoint = teleportPoints[currentPointIndex];

            characterController.enabled = false;
            transform.position = targetPoint.position;
            characterController.enabled = true;

            currentPointIndex = (currentPointIndex + 1) % teleportPoints.Length;

            yield return new WaitForSeconds(teleportInterval);

            timer += teleportInterval;
        }

        activeTeleportCoroutine = null;
    }

    #endregion

    #region Open Hidden Door Effect
    public void OpenHiddenDoor(float duration)
    {
        if (hiddenDoorCoroutine != null) StopCoroutine(hiddenDoorCoroutine);
        hiddenDoorCoroutine = StartCoroutine(OpenHiddenCoroutine(duration));
    }

    private IEnumerator OpenHiddenCoroutine(float duration)
    {
        yield return new WaitForSeconds(2);
        Services.Audio.PlaySFX("OpenHiddenDoor");

        Material mat = hiddenDoorRenderer.material;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 3;
            mat.SetFloat("_Dissolve", t);
            yield return null;
        }

        boxColliderHiddenDoor.enabled = false;

        yield return new WaitForSeconds(duration);

        boxColliderHiddenDoor.enabled = true;

        t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime / 3;
            mat.SetFloat("_Dissolve", t);
            yield return null;
        }
    }
    #endregion
}