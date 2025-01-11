using TMPro;
using UnityEngine;

namespace Bosses
{
    public class SeiryuDebugState : MonoBehaviour
    {
        [Header("Seiryu Scripts")]
        public SeiryuController seiryuController;
        public SeiryuClaw leftClaw;
        public SeiryuClaw rightClaw;

        [Header("UI")]
        public TextMeshProUGUI seiryuStateText;
        public TextMeshProUGUI leftClawStateText;
        public TextMeshProUGUI rightClawStateText;
        public TextMeshProUGUI transitionText;
        public TextMeshProUGUI phaseText;

        private void Update()
        {
            seiryuStateText.text = $"Seiryu: {seiryuController.CurrentState.ToString()}";
            leftClawStateText.text = $"L.Claw: {leftClaw.state}";
            rightClawStateText.text = $"R.Claw: {rightClaw.state}";
            transitionText.text = $"Transition: {seiryuController.transitionToNextPhase}";
            phaseText.text = $"Phase: {seiryuController.phase}";
        }
    }
}