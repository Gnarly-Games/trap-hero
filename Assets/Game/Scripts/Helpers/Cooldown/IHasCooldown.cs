using System;

namespace Game.Scripts.Helpers.Cooldown
{
    public interface IHasCooldown
    {
        Action ActionToExecute { get; }
        float CooldownDuration { get;}
    }
}
