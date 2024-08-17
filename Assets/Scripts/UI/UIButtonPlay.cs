
public class UIButtonPlay : UIButtonBase
{
    protected override void SetEventListeners()
    {
        Btn.onClick.AddListener(SceneLoader.Instance.LoadGameplayScene);
    }
}
