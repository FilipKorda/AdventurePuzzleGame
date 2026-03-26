using UnityEngine;

public class WallSwitchOnOffManager : MonoBehaviour
{
    [SerializeField] private InteractableItem switchA;
    [SerializeField] private InteractableItem switchB;
    [SerializeField] private InteractableItem switchC;
    [SerializeField] private InteractableItem switchD;
    [SerializeField] private InteractableItem switchE;
    [SerializeField] private InteractableItem switchF;

    [SerializeField] private Animator animator;

    public void OnSwitchPressed(WallSwitchType type)
    {
        switch (type)
        {
            case WallSwitchType.A:
                switchA.Toggle();
                switchB.Toggle();
                break;

            case WallSwitchType.B:
                switchB.Toggle();
                switchC.Toggle();
                break;

            case WallSwitchType.C:
                switchC.Toggle();
                switchD.Toggle();
                break;

            case WallSwitchType.D:
                switchD.Toggle();
                switchE.Toggle();
                break;

            case WallSwitchType.E:
                switchE.Toggle();
                switchF.Toggle();
                break;

            case WallSwitchType.F:
                switchF.Toggle();
                switchA.Toggle();
                break;
        }

        CheckWin();
    }

    private void CheckWin()
    {
        if (
            switchA.IsOn &&
            switchB.IsOn &&
            switchC.IsOn &&
            switchD.IsOn &&
            switchE.IsOn &&
            switchF.IsOn
        )
        {
            OpenChainCage();
        }
    }

    private void OpenChainCage()
    {
        animator.SetTrigger("Open");
    }
}