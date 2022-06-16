using RSG;

namespace Smartplank.Scripts.AnimationTransitions
{
    public interface ITransitionAnimation
    {
        public Promise AnimateShow();

        public Promise AnimateHide();
    }
}