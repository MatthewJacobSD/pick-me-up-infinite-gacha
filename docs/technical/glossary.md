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
