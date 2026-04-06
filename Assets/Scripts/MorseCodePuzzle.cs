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
        {'0', "-----"}, {'{', "--...."}, {'}', "..--.."},
        {']', ".-.-"}, {'[', "-.-..-"}, {'|', ".--.-"}, {':', "..--.-"},
        {'>', ".-.-."}, {'<', "-.-.-"}, {'!', ".--.--"}, {'@', "--..--"},
        {'#', ".-..-."}, {'$', "--.-."}, {'%', ".-.--"}, {'&', "--..-"},
        {'*', "...-."}, {'(', "-...-"}, {')', ".-..."}, {'_', "-.---"},
        {'+', "-.-.."}, {'=', "-..--"},

    };

    [SerializeField] private GameObject firstSegment;
    [SerializeField] private GameObject secondSegment;
    [SerializeField] private GameObject thirdSegment;
    [SerializeField] private GameObject fourthSegment;
    [SerializeField] private GameObject fifthSegment;
    [SerializeField] private GameObject sixthSegment;
    [SerializeField] private GameObject seventhSegment;

    [SerializeField] private GameObject button;

    [SerializeField] private Animator animator;
    [SerializeField] private LaserBeam laserBeam;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            DisableSegments();
            RotateOneByOneAllSegments();
        }
#endif
    }

    public void MorseCodeSolved()
    {
        if (
            firstSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 1 &&
            secondSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 5 &&
            thirdSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 4 &&
            fourthSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 5 &&
            fifthSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 2 &&
            sixthSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 1 &&
            seventhSegment.GetComponent<InteractableItem>().CurrentCryptexIndex == 3
           )
        {
            DisableSegments();
            RotateOneByOneAllSegments();
        }
    }


    private void DisableSegments()
    {
        firstSegment.GetComponent<BoxCollider>().enabled = false;
        secondSegment.GetComponent<BoxCollider>().enabled = false;
        thirdSegment.GetComponent<BoxCollider>().enabled = false;
        fourthSegment.GetComponent<BoxCollider>().enabled = false;
        fifthSegment.GetComponent<BoxCollider>().enabled = false;
        sixthSegment.GetComponent<BoxCollider>().enabled = false;
        seventhSegment.GetComponent<BoxCollider>().enabled = false;
        button.GetComponent<BoxCollider>().enabled = false;
    }

    private void RotateOneByOneAllSegments()
    {
        StartCoroutine(RotateSegmentsCoroutine());
        Debug.Log("Solved");
    }

    private IEnumerator RotateSegmentsCoroutine()
    {
        GameObject[] segments = {
        firstSegment,
        secondSegment,
        thirdSegment,
        fourthSegment,
        fifthSegment,
        sixthSegment,
        seventhSegment
    };

        float rotateDuration = 3.8f;
        float waitBetween = 0.75f;

        foreach (var segment in segments)
        {
            StartCoroutine(RotateSegment(segment, rotateDuration));
            Services.Audio.PlaySFX("MovingStoneKryptex");
            yield return new WaitForSeconds(waitBetween);
        }

        yield return new WaitForSeconds(2);

        animator.SetTrigger("Interact");

        yield return new WaitForSeconds(2);

        laserBeam.ToggleLaser(true);
    }

    private IEnumerator RotateSegment(GameObject segment, float duration)
    {
        Quaternion startRotation = segment.transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(315f, 0f, 0f);

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            segment.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            yield return null;
        }
        segment.transform.localRotation = targetRotation;
    }

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

    public void OMorseCode()////git
    {
        string message = "O";

        PlayMessage(message);
    }

    public void ExclamationMarkMorseCode()  ////git
    {
        string message = "!";

        PlayMessage(message);
    }

    public void QMorseCode() ////git
    {
        string message = "Q";

        PlayMessage(message);
    }

    public void YMorseCode() ////git
    {
        string message = "Y";

        PlayMessage(message);
    }

    public void OpeningBracketMorseCode() ////git
    {
        string message = "(";

        PlayMessage(message);
    }

    public void TMorseCode() ////git
    {
        string message = "T";

        PlayMessage(message);
    }

    public void EqualsMorseCode() ////git
    {
        string message = "=";

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
