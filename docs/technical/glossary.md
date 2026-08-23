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
