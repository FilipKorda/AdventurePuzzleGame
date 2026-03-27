using UnityEngine;

public class InspectSymbolObject : MonoBehaviour
{
    [Header("Statues")]
    [SerializeField] private GameObject shrineObject;
    [SerializeField] private GameObject pillarObject;
    [SerializeField] private GameObject graveObject;
    [SerializeField] private GameObject brokenPillarObject;
    [SerializeField] private GameObject woodenSwordObject;
    [Header("Frame Puzzle")]
    [SerializeField] private GameObject swordPieceObject;
    [SerializeField] private GameObject clubPieceObject;
    [SerializeField] private GameObject skeletonSwordPieceObject;
    [SerializeField] private GameObject skeletonWarAxePieceObject;
    [SerializeField] private GameObject skeletonHelmetPieceObject;
    [SerializeField] private GameObject skeletonFullHelmetPieceObject;

    [SerializeField] private PlayerBehaviour playerBehaviour;

    private GameObject currentObject;

    public void HideCurrentObject()
    {
        if (currentObject == null) return;
        ActivePlayer();
        Destroy(currentObject);
        currentObject = null;
    }

    public void ShowShrineObject()
    {
        Spawn(shrineObject);
    }

    public void ShowPillarObject()
    {
        Spawn(pillarObject);
    }

    public void ShowGraveObject()
    {
        Spawn(graveObject);
    }

    public void ShowBrokenPillarObject()
    {
        Spawn(brokenPillarObject);
    }

    public void ShowWoodenSwordObject()
    {
        Spawn(woodenSwordObject);
    }


    public void ShowSwordPieceObject()
    {
        Spawn(swordPieceObject);
    }

    public void ShowClubPieceObject()
    {
        Spawn(clubPieceObject);
    }

    public void ShowSkeletonSwordPieceObject()
    {
        Spawn(skeletonSwordPieceObject);
    }

    public void ShowSkeletonWarAxePieceObject()
    {
        Spawn(skeletonWarAxePieceObject);
    }

    public void ShowSkeletonHelmetPieceObject()
    {
        Spawn(skeletonHelmetPieceObject);
    }
    public void ShowSkeletonFullHelmetPieceObject()
    {
        Spawn(skeletonFullHelmetPieceObject);
    }

    
    private void Spawn(GameObject prefab)
    {
        if (prefab == null) return;

        if (currentObject != null)
            Destroy(currentObject);

        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        Vector3 position = player.position + player.forward * 0.5f;
        position.y += 0.55f;

        currentObject = Instantiate(
            prefab,
            position,
            Quaternion.LookRotation(player.forward)
        );

        var rotation = currentObject.GetComponent<InspectObjectRotation>();
        if (rotation != null)
            rotation.StartRotation();
    }

    public void DisablePlayerLook()
    {       
        playerBehaviour.ResetCameraRotation();
        playerBehaviour.disablePlayer = true;
    }

    public void ActivePlayer()
    {
        playerBehaviour.disablePlayer = false;
        var rotation = currentObject.GetComponent<InspectObjectRotation>();
        if (rotation != null)
            rotation.StopRotation();
    }

}
