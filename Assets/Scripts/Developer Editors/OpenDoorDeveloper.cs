using UnityEngine;

public class OpenDoorDeveloper : MonoBehaviour
{
    public Animator anim;
    public bool openDoor = false;

    private void Update()
    {
        if (openDoor) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("Interact");
            anim.SetTrigger("Open");
        }
    }
}
