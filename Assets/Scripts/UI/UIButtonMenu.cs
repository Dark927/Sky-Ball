
public class UIButtonMenu : UIButtonBase
{
    protected override void SetEventListeners()
    {
        Btn.onClick.AddListener(PauseManager.Instance.Unpause);
        Btn.onClick.AddListener(SceneLoader.Instance.LoadMainMenuScene);
    }
}
