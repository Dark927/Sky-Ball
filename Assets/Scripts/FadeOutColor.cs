using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutColor : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Range(0f, 1f)]
    [SerializeField] private float _alpha = 0;
    [SerializeField] private float _duration = 2f;

    private Material _material;
    private Color _originalColor;

    private const float _invalidAlpha = -1f;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods 

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _originalColor = _material.color;
    }

    private IEnumerator FadeOutCoroutine(float alpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(_originalColor.a, alpha, elapsedTime / duration);

            Color newColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, newAlpha);
            _material.color = newColor;

            yield return null;
        }

        _material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public void StartFadeOut(float alpha = _invalidAlpha)
    {
        if (Mathf.Approximately(alpha, _invalidAlpha))
        {
            alpha = _alpha;
        }

        StartCoroutine(FadeOutCoroutine(alpha, _duration));
    }

    #endregion
}
