using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [SerializeField] private UIGameplay _gameplay;
    [SerializeField] private UIPause _pause;

    public UIGameplay Gameplay => _gameplay;
    public UIPause Pause => _pause;


    public void SwitchGameplayToPauseUI()
    {
        DisableGameplayUI();
        EnablePauseUI();
    }

    public void SwitchPauseToGameplayUI()
    {
        DisablePauseUI();
        EnableGameplayUI();
    }


    public void EnableGameplayUI()
    {
        _gameplay.gameObject.SetActive(true);
    }

    public void EnablePauseUI()
    {
        _pause.gameObject.SetActive(true);
    }

    public void DisableGameplayUI()
    {
        _gameplay.gameObject.SetActive(false);
    }

    public void DisablePauseUI()
    {
        _pause.gameObject.SetActive(false);
    }


    private void Start()
    {
        CheckForNullReferences();
    }

    private void CheckForNullReferences()
    {
        if ((_gameplay == null) || (_pause == null))
        {
            string errorMsg = $"{nameof(_gameplay)} == {_gameplay}, {nameof(_pause)} == {_pause}. - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
        }
    }
}
