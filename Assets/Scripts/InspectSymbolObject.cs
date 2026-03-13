using UnityEngine;

public class InspectSymbolObject : MonoBehaviour
{
    [SerializeField] private GameObject shrineObject;
    [SerializeField] private GameObject pillarObject;
    [SerializeField] private GameObject graveObject;
    [SerializeField] private GameObject brokenPillarObject;
    [SerializeField] private GameObject woodenSwordObject;

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
