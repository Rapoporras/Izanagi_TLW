/*
    This script is part of the SO Game Events Architecture Project.
    You are free to use, modify, and distribute the code as you want.
    Credit is not required, but it is always appreciated.

    Author: Diego Ruiz Gil
    https://github.com/DiegoRuizGil/SO-Game-Events-Architecture-Unity
*/

using UnityEngine.Events;
using Bosses;

namespace GameEvents
{
    public class SeiryuAttackInfoListener : BaseGameEventListener<SeiryuAttackInfo, SeiryuAttackInfoEvent, UnityEvent<SeiryuAttackInfo>> { }
}