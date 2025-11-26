using System;
using System.Linq;
using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Core.Domain.Characters;

namespace RefactoredCommandSystem.Cli.Modes.Characters.Commands
{
    public class ActCommand : CharactersCommandBase
    {
        public ActCommand(CharacterRegistry registry, IConsoleAdapter console)
            : base(registry, console)
        {
        }

        public override string Verb => "act";
        public override string Description => "act <attack|heal|ability> <actor> <target> [--id <ability>] - performs an action.";

        public override void Handle(CommandInput input)
        {
            if (input.Arguments.Count < 3)
            {
                Console.WriteLine("Usage: act <attack|heal|ability> <actor> <target> [--id <ability>]");
                return;
            }

            var actionType = input.Arguments[0].ToLowerInvariant();
            var actor = Registry.RequireCharacter(input.Arguments[1]);
            var target = Registry.RequireCharacter(input.Arguments[2]);
            Ability? ability = null;

            if (input.TryGetOption("id", out var abilityKey) && !string.IsNullOrWhiteSpace(abilityKey))
            {
                ability = Registry.RequireAbility(abilityKey!);
            }
            else
            {
                ability = FindAbilityForAction(actor, actionType);
            }

            switch (actionType)
            {
                case "attack":
                    ExecuteAttack(actor, target, ability);
                    break;
                case "heal":
                    ExecuteHeal(actor, target, ability);
                    break;
                case "ability":
                    ExecuteUtility(actor, target, ability);
                    break;
                default:
                    Console.WriteLine("Unknown action type.");
                    break;
            }
        }

        private Ability? FindAbilityForAction(Character actor, string actionType)
        {
            return actionType switch
            {
                "attack" => actor.Abilities.FirstOrDefault(a => a.Kind == AbilityKind.Attack),
                "heal" => actor.Abilities.FirstOrDefault(a => a.Kind == AbilityKind.Heal),
                "ability" => actor.Abilities.FirstOrDefault(),
                _ => null
            };
        }

        private void ExecuteAttack(Character actor, Character target, Ability? ability)
        {
            var damage = ability?.EffectValue ?? 10;
            target.Damage(damage);
            var source = ability is null ? "basic attack" : $"ability '{ability.Name}'";
            Console.WriteLine($"{actor.Name} attacks {target.Name} with {source}, dealing {damage} damage. Target HP: {target.HealthPoints}.");
        }

        private void ExecuteHeal(Character actor, Character target, Ability? ability)
        {
            var healValue = ability?.EffectValue ?? 10;
            target.Heal(healValue);
            var source = ability is null ? "basic heal" : $"ability '{ability.Name}'";
            Console.WriteLine($"{actor.Name} heals {target.Name} via {source}, restoring {healValue} HP. Target HP: {target.HealthPoints}.");
        }

        private void ExecuteUtility(Character actor, Character target, Ability? ability)
        {
            var abilityName = ability?.Name ?? "improvised action";
            Console.WriteLine($"{actor.Name} uses {abilityName} on {target.Name}. The effect is narrative and has no numeric outcome.");
        }
    }
}

