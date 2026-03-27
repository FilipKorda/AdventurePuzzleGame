using UnityEngine;
using System.Collections;

public class ScaleLogic : MonoBehaviour
{
    [SerializeField] private GameObject jointHinge;
    [SerializeField] private GameObject[] helperHinges;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            StartCoroutine(RotateY(-5f));

        if (Input.GetKeyDown(KeyCode.N))
            StartCoroutine(RotateY(5f));
    }

    private IEnumerator RotateY(float delta)
    {
        float startMainY = jointHinge.transform.localEulerAngles.y;
        float targetMainY = startMainY + delta;

        float[] startHelpersY = new float[helperHinges.Length];
        float[] targetHelpersY = new float[helperHinges.Length];

        for (int i = 0; i < helperHinges.Length; i++)
        {
            startHelpersY[i] = helperHinges[i].transform.localEulerAngles.y;
            targetHelpersY[i] = startHelpersY[i] - delta;
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 5f;

            float mainY = Mathf.LerpAngle(startMainY, targetMainY, t);
            jointHinge.transform.localRotation = Quaternion.Euler(0f, mainY, 0f);

            for (int i = 0; i < helperHinges.Length; i++)
            {
                float helperY = Mathf.LerpAngle(startHelpersY[i], targetHelpersY[i], t);
                helperHinges[i].transform.localRotation = Quaternion.Euler(0f, helperY, 0f);
            }

            yield return null;
        }

        jointHinge.transform.localRotation = Quaternion.Euler(0f, targetMainY, 0f);

        for (int i = 0; i < helperHinges.Length; i++)
            helperHinges[i].transform.localRotation = Quaternion.Euler(0f, targetHelpersY[i], 0f);
    }
}