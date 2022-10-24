using Platformer3d.CharacterSystem.NPC;
using UnityEngine;

namespace Platformer3d.Interaction
{
    public interface ITalkable : IPerformer
    {
        public void Talk();
        public void SetConversation(string id, bool reload);
    }

    public class TalkNPCTrigger : InteractionTrigger
    {
        [SerializeField]
        private BaseNPC _targetNPC;

        private ITalkable _talkabkeNPC;

        public override string ActionId => _targetNPC.ActionId;

        private void Start()
        {
            _interactionTarget = _talkabkeNPC = _targetNPC.GetComponent<ITalkable>();
        }

        public override void Perform()
        {
            _talkabkeNPC.Talk();
        }
    }
}