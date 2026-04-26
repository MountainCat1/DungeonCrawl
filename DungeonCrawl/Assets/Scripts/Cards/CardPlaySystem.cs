using DefaultNamespace.Systems.Base;

namespace DefaultNamespace.Cards
{
    public interface ICardPlaySystem
    {
        void Play(Card card);
    }
    
    public class CardPlaySystem : GameSystem, ICardPlaySystem
    {
        public void Play(Card card)
        {
            card.Play();
        }
    }
}