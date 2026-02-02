using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class MultiStageMover : MonoBehaviour
{
    [Header("Ustawienia Obiektu do Poruszania")]
    [Tooltip("Obiekt, który bêdzie siê porusza³ miêdzy pozycjami.")]
    public GameObject objectToMove;

    [Tooltip("Lista 5 pozycji docelowych, do których obiekt ma siê przesuwaæ.")]
    public Transform[] targetPositions = new Transform[5];

    [Tooltip("Prêdkoœæ poruszania siê obiektu.")]
    public float moveSpeed = 5f;

    private Vector3 originalPosition;
    private Coroutine moveCoroutine;

    private int currentStage = 0;
    private bool isMoving = false;

    public GameObject fireObject;
    public LocalizedString localizeString;

    void Start()
    {
        originalPosition = objectToMove.transform.position;

        objectToMove.SetActive(false);
    }

    public void MoveWaterUp()
    {
        if(fireObject.activeSelf == false)
        {
            NotificationSystem.Instance.ShowNotification(localizeString, 3);
            Debug.Log("Nie mo¿na przesun¹æ obiektu - ogieñ jest wy³¹czony.");
            return;
        }

        moveCoroutine = StartCoroutine(MoveObjectToNextStage());
    }

    public void ResetObjectState()
    {
        if (isMoving && moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        if (objectToMove != null)
        {
            objectToMove.transform.position = originalPosition;
            objectToMove.SetActive(false);
        }

        currentStage = 0;
        isMoving = false;

        Debug.Log("Stan obiektu zosta³ zresetowany.");
    }

    private IEnumerator MoveObjectToNextStage()
    {
        isMoving = true;

        if (currentStage == 0 && !objectToMove.activeSelf)
        {
            objectToMove.transform.position = originalPosition;
            objectToMove.SetActive(true);
        }

        Vector3 endPosition = targetPositions[currentStage].position;

        while (Vector3.Distance(objectToMove.transform.position, endPosition) > 0.01f)
        {
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                endPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        objectToMove.transform.position = endPosition;
        currentStage++;
        isMoving = false;
        moveCoroutine = null; 
    }
}