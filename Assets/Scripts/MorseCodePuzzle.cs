using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MorseCodePuzzle : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoxCollider[] boxColliderButtons;
    [SerializeField] private Image[] numberImages;

    [Header("Moving Wall")]
    [SerializeField] private GameObject wallUp;
    [SerializeField] private GameObject wallDown;
    [SerializeField] private float moveTime = 1f;

    [Header("Morse Code")]
    [SerializeField] private float unitTime = 0.2f;
    Dictionary<char, string> morse = new Dictionary<char, string>()
    {
        {'A', ".-"},   {'B', "-..."}, {'C', "-.-."}, {'D', "-.."},
        {'E', "."},    {'F', "..-."}, {'G', "--."},  {'H', "...."},
        {'I', ".."},   {'J', ".---"}, {'K', "-.-"},  {'L', ".-.."},
        {'M', "--"},   {'N', "-."},   {'O', "---"},  {'P', ".--."},
        {'Q', "--.-"}, {'R', ".-."},  {'S', "..."},  {'T', "-"},
        {'U', "..-"},  {'V', "...-"}, {'W', ".--"},  {'X', "-..-"},
        {'Y', "-.--"}, {'Z', "--.."},
        {'1', ".----"}, {'2', "..---"}, {'3', "...--"},
        {'4', "....-"}, {'5', "....."}, {'6', "-...."},
        {'7', "--..."}, {'8', "---.."}, {'9', "----."},
        {'0', "-----"}
    };

    public void ActivateMorseCodePuzzle()
    {
        foreach (BoxCollider boxCollider in boxColliderButtons)
        {
            boxCollider.enabled = true;
        }

        foreach (Image image in numberImages)
        {
            image.enabled = false;
        }

        MoveWalls();

        Debug.Log("Morse code puzzle activated!");
    }

    public void MoveWalls()
    {
        StartCoroutine(MoveWall(wallUp.transform, 1.5f));
        StartCoroutine(MoveWall(wallDown.transform, -0.5f));
    }

    private IEnumerator MoveWall(Transform wall, float targetY)
    {
        Vector3 startPos = wall.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            wall.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        wall.localPosition = targetPos;
    }

    public void ZMorseCode()
    {
        string message = "Z";

        PlayMessage(message);
    }

    private IEnumerator DisableButtonColliders()
    {
        foreach (BoxCollider boxCollider in boxColliderButtons)
        {
            boxCollider.enabled = false;
        }

        yield return new WaitForSeconds(4f);

        foreach (BoxCollider boxCollider in boxColliderButtons)
        {
            boxCollider.enabled = true;
        }

        yield return null;
    }

    private void PlayMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(DisableButtonColliders());
        StartCoroutine(PlayCoroutine(message.ToUpper()));
    }

    IEnumerator PlayCoroutine(string message)
    {
        foreach (char c in message)
        {
            if (c == ' ')
            {
                yield return new WaitForSeconds(unitTime * 7);
                continue;
            }

            if (!morse.ContainsKey(c))
                continue;

            string code = morse[c];

            foreach (char symbol in code)
            {
                if (symbol == '.')
                {
                    Services.Audio.PlaySFX("ShortBeep");
                    yield return new WaitForSeconds(unitTime);
                }
                else if (symbol == '-')
                {
                    Services.Audio.PlaySFX("LongBeep");
                    yield return new WaitForSeconds(unitTime * 3);
                }

                yield return new WaitForSeconds(unitTime);
            }

            yield return new WaitForSeconds(unitTime * 3);
        }
    }


}
