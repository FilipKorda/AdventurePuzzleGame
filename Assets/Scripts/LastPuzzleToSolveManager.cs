using System.Collections;
using UnityEngine;

public class LastPuzzleToSolveManager : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(8, 4);
    public LastPuzzleButton[,] buttons;
    [SerializeField] private LastPuzzle lastPuzzle;

    [SerializeField] private ChestManager chestObject1;
    [SerializeField] private ChestManager chestObject2;
    [SerializeField] private float dist;

    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;

    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;

    private bool[,] initialStates;


    private void Start()
    {
        buttons = new LastPuzzleButton[size.x, size.y];
        initialStates = new bool[size.x, size.y];

        foreach (LastPuzzleButton b in GetComponentsInChildren<LastPuzzleButton>())
        {
            Vector2Int idx = b.index;
            buttons[idx.x, idx.y] = b;
            b.Init(idx, this);
        }

        DisableButtons();
        SaveInitialStates();
    }

    private void SaveInitialStates()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                initialStates[x, y] = buttons[x, y].isButtonActive;
            }
    }

    public void ResetPuzzle()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                buttons[x, y].SetState(initialStates[x, y]);
            }
    }


    public void ToggleAt(Vector2Int index)
    {
        if (index.x < 0 || index.y < 0) return;
        if (index.x >= size.x || index.y >= size.y) return;

        buttons[index.x, index.y].Toggle();
    }

    public void CheckSolved()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                if (!buttons[x, y].isButtonActive)
                    return;
            }

        lastPuzzle.ExitAfterWin();
        MoveChest();
        DisableButtons();
    }

    public void DisableButtons()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                buttons[x, y].GetComponent<Collider>().enabled = false;
            }
    }

    public void EnabledButtons()
    {
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                buttons[x, y].GetComponent<Collider>().enabled = true;
            }
    }

    private void MoveChest()
    {
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
        StartCoroutine(MoveChestCoroutine(chestObject1));
        StartCoroutine(MoveChestCoroutine(chestObject2));

        rotateAllGearsRoomTen.RotateTenGears();
        Services.Audio.PlaySFX("ChestMovingStone");
    }

    private IEnumerator MoveChestCoroutine(ChestManager chest)
    {
        Vector3 startPos = chest.transform.position;
        Vector3 endPos = startPos + Vector3.left * dist;
        float duration = 3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            chest.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        chest.transform.position = endPos;
        chest.OpenChest();
    }

    public void RotateHandles()
    {
        StartCoroutine(RotateHandle(leftHandle.transform, Vector3.up));
        StartCoroutine(RotateHandle(rightHandle.transform, Vector3.forward));
    }

    private IEnumerator RotateHandle(Transform target, Vector3 axis)
    {
        float time = 0f;
        float duration = 3f;

        Quaternion startRot = target.localRotation;
        Quaternion endRot = startRot * Quaternion.AngleAxis(90f, axis);

        while (time < duration)
        {
            time += Time.deltaTime;
            target.localRotation = Quaternion.Slerp(startRot, endRot, time / duration);
            yield return null;
        }

        target.localRotation = endRot;
    }
}