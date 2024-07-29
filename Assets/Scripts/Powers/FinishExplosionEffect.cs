
public class FinishExplosionEffect : FinishDefaultEffect
{
    protected override void SetFinishOptions()
    {
        Explosion sourceExplosion = GetComponentInParent<Explosion>();

        if (sourceExplosion != null)
        {
            sourceExplosion.gameObject.SetActive(false);
        }
    }
}
