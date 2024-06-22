using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutColor : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    [Range(0f, 1f)]
    [SerializeField] float alphaAlpha = 0;
    [SerializeField] float duradion = 2f;

    Material material;
    Color originalColor;

    const float invalidAlpha = -1f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods 

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        originalColor = material.color;
    }

    IEnumerator FadeOutCoroutine(float alpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(originalColor.a, alpha, elapsedTime / duration);

            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            material.color = newColor;

            yield return null;
        }

        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public void StartFadeOut(float alpha = invalidAlpha)
    {
        if (Mathf.Approximately(alpha, invalidAlpha))
        {
            alpha = alphaAlpha;
        }

        StartCoroutine(FadeOutCoroutine(alpha, duradion));
    }

    #endregion
}
