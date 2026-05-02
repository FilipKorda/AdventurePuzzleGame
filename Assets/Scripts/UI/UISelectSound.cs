using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectSound : MonoBehaviour,
    ISelectHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    ISubmitHandler
{
    private string selectSoundId = "UISelect";
    private string clickSoundId = "UIClick";
    private string backSoundId = "UIBack";

    private bool pointerInside;

    public bool isBackButton = false;

    public void OnSelect(BaseEventData eventData)
    {
        Services.Audio.PlaySFX(selectSoundId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerInside)
            return;

        pointerInside = true;
        Services.Audio.PlaySFX(selectSoundId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInside = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBackButton)
        {
            Services.Audio.PlaySFX(backSoundId);
        }
        else
        {
            Services.Audio.PlaySFX(clickSoundId);
        }

    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (isBackButton)
        {
            Services.Audio.PlaySFX(backSoundId);
        }
        else
        {
            Services.Audio.PlaySFX(clickSoundId);
        }
    }

    private void OnDisable()
    {
        pointerInside = false;
    }
}
