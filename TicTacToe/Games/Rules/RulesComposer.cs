using System.Collections.Generic;

namespace TicTacToe.Games.Rules
{
    public class RulesComposer : RuleAssistant
    {
        private readonly ISet<IRuleAssistant> _assistants = new HashSet<IRuleAssistant>();

        public virtual void Add(IRuleAssistant assistant)
        {
            _assistants.Add(assistant);
        }

        public override void AcceptMove(Move move)
        {
            foreach (var assistant in _assistants)
                assistant.AcceptMove(move);
        }
    }
}
