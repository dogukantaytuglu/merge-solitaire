namespace Whatwapp.MergeSolitaire.Game
{
    public class BombBlockVisual : BaseBlockVisual
    {
        public void Init()
        {
            _spriteRenderer.color = _colorSettings.BombBlockColor;
        }
    }
}