using MoreMountains.NiceVibrations;
using MyBox;

namespace Game.Scripts.Core
{
    public class VibrationHandler : Singleton<VibrationHandler>
    {
        public static void HapticFeedback(HapticTypes type = HapticTypes.LightImpact)
        {
            if (!MMVibrationManager.HapticsSupported()) return;
            
            MMVibrationManager.Haptic(type);
        }
    }
}
