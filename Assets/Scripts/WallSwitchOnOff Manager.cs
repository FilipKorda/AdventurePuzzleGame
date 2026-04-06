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

    [SerializeField] private BoxCollider[] boxColliders;

    [SerializeField] private Color highlightColor;
    private Color baseColor;

    [SerializeField] private Renderer diodaRenderer;
    [SerializeField] private Renderer diodaRenderer1;
    [SerializeField] private Renderer diodaRenderer2;
    [SerializeField] private Renderer diodaRenderer3;
    [SerializeField] private Renderer diodaRenderer4;
    [SerializeField] private Renderer diodaRenderer5;

    private void Start()
    {

        baseColor = diodaRenderer.material.color;
        baseColor = diodaRenderer1.material.color;
        baseColor = diodaRenderer2.material.color;
        baseColor = diodaRenderer3.material.color;
        baseColor = diodaRenderer4.material.color;
        baseColor = diodaRenderer5.material.color;

    }

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

    private void DisableAllSwitches()
    {
        foreach (var boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }
    }

    private void CheckWin()
    {
        if (switchA.IsOn)
        {
            diodaRenderer.material.color = highlightColor;
        }
        else
        {
            diodaRenderer.material.color = baseColor;
        }
        if (switchB.IsOn)
        {
            diodaRenderer1.material.color = highlightColor;
        }
        else
        {
            diodaRenderer1.material.color = baseColor;
        }
        if (switchC.IsOn)
        {
            diodaRenderer2.material.color = highlightColor;
        }
        else
        {
            diodaRenderer2.material.color = baseColor;
        }
        if (switchD.IsOn)
        {
            diodaRenderer3.material.color = highlightColor;
        }
        else
        {
            diodaRenderer3.material.color = baseColor;
        }
        if (switchE.IsOn)
        {
            diodaRenderer4.material.color = highlightColor;
        }
        else
        {
            diodaRenderer4.material.color = baseColor;
        }
        if (switchF.IsOn)
        {
            diodaRenderer5.material.color = highlightColor;
        }
        else
        {
            diodaRenderer5.material.color = baseColor;
        }

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
        DisableAllSwitches();
        animator.SetTrigger("Open");
        Services.Audio.PlaySFX("chain");
    }
}