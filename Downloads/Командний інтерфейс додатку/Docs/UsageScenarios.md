# Usage Scenarios

## 1. Text Editing Mode

1. Run the application with no arguments.
2. At the mode prompt type `text` to start the structured text editor.
3. Use the commands:
   - `pwd` to inspect the current location within the tree.
   - `add container section` → `Name: Introduction` to create a container.
   - `cd introduction` to move into the new section.
   - `add leaf paragraph` → provide a name and body text to create content.
   - `print --whole --id` to display the entire document including element identifiers.
4. Navigate back to the root with `up` or `cd /`.

## 2. Character Management Mode

1. Start the app and select the `chars` mode.
2. Create entities:
   - `create char` to add a playable character (follow the interactive prompts).
   - `create item` and `create ability` for inventory and skills.
3. Assign created entities:
   - `add --char_id <characterName> --id <itemOrAbilityId>` to link items or abilities.
4. Trigger actions:
   - `act attack hero villain --id fireball` to run a combat scenario.
5. Inspect current state with `ls char`, `ls item`, or `ls ability --id <name>`.

## 3. Command Loader Demonstration

1. Build the project and run the console application with the `--demo` flag.
2. The program will:
   - Load commands from `commands.json` via the JSON persistence provider.
   - Execute every command through the application command executor.
   - Execute a single command named `print_hello`.

Use this scenario to showcase the modular persistence and application layers independently of the CLI.

