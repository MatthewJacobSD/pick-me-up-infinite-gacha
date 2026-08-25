# Terminology Map — Dedication Glossary

| Long Term | Technical Term | Description |
|---|---|---|
| The Master | User/Client | The player entity controlling the account |
| The Smartphone | Client Interface | The Unity frontend rendering the data |
| The Sanctuary | Game State / Town Service | The persistent data regarding buildings, resources, and passive timers |
| Heroes/Units | Entity / Character Object | The Gacha units. They have stats, IDs, and a `IsDeceased` flag |
| The Tower | Combat Instance | A session of gameplay where battle logic is calculated |
| Dimensional Store | Gacha Service | The RNG engine for unit generation |
| Intervention | Interrupt Request | A real-time command sent via WebSocket to alter the battle simulation |
| Space Rift | Latency/Lag | Network delay we must mitigate via prediction |
| Streams | Linked Quest Chain | Quest sequences seeded by exploration hints; chain analysis is player-skill dependent (Ch. 15) |
| Affinity Link | Summon Cohort Bonus | Rare consecutive-summon outcome; linked heroes are stronger in the same party (Ch. 15) |
| Open Duel | Formal Hero Challenge | In-game duel request between heroes; requires master approval (Yes/No); special conditions allowed if both sides agree; interference forbidden; loser is consumed per condition and cannot be cancelled mid-duel (Ch. 16-17) |
| Returned to the Goddess' Embrace | Permadeath Notice | System death message template ("fighting spirit will be remembered for eternity") confirming permanent deletion |
| [Sudden Death] | Cause-of-Death Classification | System-attributed death cause — self-defence kills are logged as "suicide due to stress" against the violators |
| Manager Self-Defence | Anti-Interference Enforcement | Manager's sanctioned lethal response to rule violations (e.g. interfering in an active duel) after one warning |
| Weeding | Newbie Attrition Pass | Master-driven synthesis of fresh summons deemed useless; targets chosen without supporter consideration (Ch. 19) |
| Party Suggestions | Player Roster Proposal | Hero-submitted party rosters the master may accept or reject; the reverse path (hero-invites-hero) uses a master Yes/No prompt (Ch. 19-20) |
| Drawing Lots | Random Party Split | Heroes split into parties by lot to spread annihilation risk and balance growth; leaders draw for their groups (Ch. 19) |
| Crack of Time and Space | Newcomer Test Instance | System-opened test stage that evaluates newly summoned heroes; destination floor can be overridden by the master (Ch. 20-21; naming variance preserved) |
| Magic Hall | Mage Support Facility | Combined magic research lab + alchemy lab + library facility; unlocks research skills, item synthesis, and mage knowledge development (Ch. 24) |
| Isralta Mine | Fire-Attribute Material Source | Third weekday dungeon; fire attribute stones for promotion/crafting (opened Ch. 24) |
| Switching | Weapon Quick-Swap Skill | Lets a fighter switch between dagger and arrows mid-fight at any range; learned through drilling (Ch. 24-25) |
| Fire Resistance / Pain Resistance | Endurance Skills | Obtained by enduring brazier fire (10-second count) / pain respectively; health potions heal but inflict extreme pain, making Pain Resistance a melee requirement (Ch. 24) |
| Fire Magic Classes | Mana Tier System | Class 1 ignition, Class 2 burning/explosive, Class 3 upward-directed explosion; higher class = higher mana consumption (Ch. 25) |
| Tactical Station | Cooperation Command Layer | Mission system letting the master issue tactical guidelines to heroes during cooperation missions; complex for new players (Ch. 25) |
| Tank Without Armor | Artillery-Caster Doctrine | Han's analogy for fragile heavy-damage mages: firepower in exchange for defense and mobility — party must protect the cast window (Ch. 23) |
| Defend Mission | Objective-Protection Instance | Mission type where the goal is to protect an objective ("Stop the fall of the city"); mission failure kills every hero regardless of location (Ch. 26) |
| Three-Part Warning | Highest Difficulty Tier | Mission opening with three consecutive "Warning!" messages — signifier of maximum difficulty (first seen Ch. 26) |
| Twin Goddesses | Fail-Condition Object | Statue on the central tower; its destruction fails the Defend mission even if all enemies are killed (Ch. 26) |
| Ladder Cart | Anti-Wall Siege Equipment | The only siege equipment on Floor 10; exclusive to the northern goblin army; counter: destroy ladders as they hook onto walls (Ch. 26) |
| NPC Perception Gap | Observer-Only Presence | Heroes are visible and touchable to NPCs but cannot be heard or seen by them (Jenna's porter test, Ch. 26) |
| Bait Doctrine | Sanctioned Attrition Tactic | Master-approved use of soldiers/refugees as bait when defenses break ("It might be necessary." — Han, Ch. 26) |
