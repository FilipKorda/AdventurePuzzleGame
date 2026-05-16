using UnityEngine;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class LocalizedControlIconText : MonoBehaviour
{
    [SerializeField] private ControlPanelsManager controlPanelsManager;
    [SerializeField] private VariablesGroupAsset controlsVariablesGroup;

    [Header("Variable Names")]
    [SerializeField] private string PpmName = "PPM";
    [SerializeField] private string LpmName = "LPM";
    [SerializeField] private string QName = "Q";
    [SerializeField] private string WName = "W";
    [SerializeField] private string AName = "A";
    [SerializeField] private string SName = "S";
    [SerializeField] private string DName = "D";
    [SerializeField] private string EscName = "ESC";
    [SerializeField] private string FName = "F";
    [SerializeField] private string EName = "E";
    [SerializeField] private string MouseName = "Mouse";
    [SerializeField] private string LeavePuzzleName = "LeavePuzzle";
    [SerializeField] private string RName = "R";

    [SerializeField] private string AlchemyRecipesNameE = "AlchemyRecipesE";
    [SerializeField] private string AlchemyRecipesNameQ = "AlchemyRecipesQ";
    
    [SerializeField] private string WFurnitureMovementName = "WFurnitureMovement";
    [SerializeField] private string SFurnitureMovementName = "SFurnitureMovement";
    [SerializeField] private string AFurnitureMovementName = "AFurnitureMovement";
    [SerializeField] private string DFurnitureMovementName = "DFurnitureMovement";

    [Header("Keyboard Sprite Names")]
    [SerializeField] private string PpmKeyboardIcon = "ControlsSheet_2";
    [SerializeField] private string LpmKeyboardIcon = "ControlsSheet_3";
    [SerializeField] private string DropKeyboardIcon = "ControlsSheet_8";

    [SerializeField] private string WKeyboardIcon = "ControlsSheet_12";
    [SerializeField] private string AKeyboardIcon = "ControlsSheet_13";
    [SerializeField] private string SKeyboardIcon = "ControlsSheet_14";
    [SerializeField] private string DKeyboardIcon = "ControlsSheet_15";

    [SerializeField] private string EscKeyboardIcon = "ControlsSheet_9";
    [SerializeField] private string FKeyboardIcon = "ControlsSheet_10";
    [SerializeField] private string EKeyboardIcon = "ControlsIcons_v2_0";

    [SerializeField] private string MouseKeyboardIcon = "ControlsIcons_v2_1";

    [SerializeField] private string RKeyboardIcon = "ControlsSheetJustR_0";

    [Header("Controller Sprite Names")]
    [SerializeField] private string controllerAIcon = "ControlsSheet_7";
    [SerializeField] private string controllerBIcon = "ControlsSheet_6";
    [SerializeField] private string controllerXIcon = "ControlsSheet_4";
    [SerializeField] private string controllerYIcon = "ControlsSheet_5";

    [SerializeField] private string controllerStickIcon = "ControlsSheet_0";
    [SerializeField] private string controllerNavigationIcon = "ControlsSheet_1";


    private void OnEnable()
    {
        if (controlPanelsManager != null)
            controlPanelsManager.OnInputDeviceChanged += RefreshIcons;

        RefreshIcons(controlPanelsManager != null && controlPanelsManager.IsUsingController);
    }

    private void OnDisable()
    {
        if (controlPanelsManager != null)
            controlPanelsManager.OnInputDeviceChanged -= RefreshIcons;
    }

    public void RefreshIcons(bool usingController)
    {
        if (controlsVariablesGroup == null)
            return;

        using (PersistentVariablesSource.UpdateScope())
        {
            SetStringVariable(PpmName, usingController ? controllerBIcon : PpmKeyboardIcon);
            SetStringVariable(QName, usingController ? controllerXIcon : DropKeyboardIcon);

            SetStringVariable(AlchemyRecipesNameE, usingController ? controllerNavigationIcon : EKeyboardIcon);
            SetStringVariable(AlchemyRecipesNameQ, usingController ? controllerNavigationIcon : DropKeyboardIcon);

            SetStringVariable(AName, usingController ? controllerNavigationIcon : AKeyboardIcon);
            SetStringVariable(DName, usingController ? controllerNavigationIcon : DKeyboardIcon);

            SetStringVariable(EName, usingController ? controllerBIcon : EKeyboardIcon);

            SetStringVariable(LpmName, usingController ? controllerAIcon : LpmKeyboardIcon);

            SetStringVariable(FName, usingController ? controllerYIcon : FKeyboardIcon);

            SetStringVariable(WFurnitureMovementName, usingController ? controllerNavigationIcon : WKeyboardIcon);
            SetStringVariable(SFurnitureMovementName, usingController ? controllerNavigationIcon : SKeyboardIcon);
            SetStringVariable(AFurnitureMovementName, usingController ? controllerNavigationIcon : AKeyboardIcon);
            SetStringVariable(DFurnitureMovementName, usingController ? controllerNavigationIcon : DKeyboardIcon);

            SetStringVariable(MouseName, usingController ? controllerStickIcon : MouseKeyboardIcon);

            SetStringVariable(LeavePuzzleName, usingController ? controllerBIcon : EKeyboardIcon);

            SetStringVariable(RName, usingController ? controllerYIcon : RKeyboardIcon);

        }
    }

    private void SetStringVariable(string variableName, string value)
    {
        if (!controlsVariablesGroup.TryGetValue(variableName, out var variable))
            return;

        if (variable is StringVariable stringVariable)
            stringVariable.Value = value;
    }
}
