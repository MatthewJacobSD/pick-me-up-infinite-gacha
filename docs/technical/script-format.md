# Script Format Reference

> Format rules for manwha chapter documentation.
> Updated when new patterns emerge.

---

## Content Types

| Type | Format | Example |
|---|---|---|
| **Dialogue** | `Character: text` | `Islat Han: Wait a minute!` |
| **Narrative** | `Character - narrating: text` | `Han - narrating: My name is Islan Han.` |
| **Internal Monologue** | `Character: *in his mind* text` | `Han: *in his mind* I'm fucked!` |
| **Notification** | `Notification: text` | `Notification: Stage clear!` |
| **Alert** | `Alert-Notification: text` | `Alert-Notification: Warning!` |
| **TIPS** | `TIPS: text` | `TIPS: Heroes without weapons...` |
| **Sound** | `*action* SOUND` | `*roar* Kyaaa!!` |
| **Stat Block** | `• Stat: value` | `• Strength: 14/14` |

---

## Dialogue Rules

### Basic Format
```
Character Name: Dialogue text
Character Name: Dialogue text (continued)
```

### With Actions/Emotions
```
Character Name: *action, emotion* Dialogue text
Character Name: *shouts* Dialogue text
Character Name: *in his mind* Internal thought
```

### Multi-Line Dialogue
```
Character Name: First line
Character Name: Second line
Character Name: Third line
```

### NPC Format
```
NPC#: Dialogue text
NPC#: *action* Dialogue text
```

---

## Narrative Rules

### Format
```
Character - narrating: Narrative text
Character - narrating: Continued narrative text
```

### Usage
- Background information
- Character thoughts (non-dialogue)
- World-building explanations
- Time transitions

---

## Notification Rules

### Standard Notification
```
Notification: text
```

### Alert Notification
```
Alert-Notification: Warning!
Alert-Notification: Warning!
Alert-Notification: Warning!
```

### Stat Block Notification
```
Notification:
• Character (stars) Level X (exp X/X)
• Class: Class Name
• Strength: X/X Intelligence: X/X
• Health: X/X Agility: X/X
• Possessed skill: skill name (level X)
```

### Death Notification
```
Notification: Character (stars) has returned to the goddess' embrace.
Notification: His fighting spirit will be remembered for eternity.
```

### Synthesis Notification
```
Notification: Synthesis Complete!
Notification: Character (stars) has become light and is disappearing.
```

---

## TIPS Rules

### Format
```
TIPS: Tip text
```

### Usage
- Game mechanics explanations
- Strategy hints
- System tutorials

---

## Sound Rules

### Format
```
*action* SOUND
```

### Examples
```
*roar* Kyaaa!!
*screams* UUUAAAH!
*pants* Huff Huff
*shouts* HEY!
```

### Capitalization
- All active sounds in CAPS
- `KYAAAAA` not `Kyaaaa`
- `KUGH!` not `Kugh!`
- `UUUAAAH!` not `Uwaaaah!`

---

## Action/Emotion Tags

### Common Tags
```
*shouts*
*screams*
*pants*
*sigh*
*angry*
*scared*
*panics*
*confused*
*determined*
*locked in*
*fuming*
*pleased*
*shocked*
```

### Placement
- Before dialogue: `Character: *action* text`
- Within dialogue: `Character: text *action* text`
- Standalone: `*action*`

---

## Scene Structure

### Scene Header
```
### Scene X: Title
```

### Scene Content
```
Dialogue lines
Notification lines
TIPS lines
Sound lines
```

---

## Character Name Rules

### Consistent Spelling
- `Islat Han` (not Islan Han)
- `Iselle` (not Islette)
- `Niflheimr` (not Nilflheim)
- `ANYTNG` (not Anytng)
- `Molmont` (not Mormont)
- `Lyla` (not Lyle - when referring to character)

### Name Variations
- `Han` = short for Islat Han
- `Master` = ANYTNG
- `Loki` = Han's identity

---

## Line Arrangement

### Dialogue Flow
- Each character line on separate line
- Continuation lines for same character
- Notification breaks between dialogue

### Stat Blocks
- Separate from dialogue
- Bullet points for each stat
- Level, class, skills included

---

## Example (Ch.1 Format)

```
Islat Han: Islat Han.
Islat Han: I was a farmer.
Islat Han: I don't know anything either.
•	Farmer
•	Status: Clueless, Composed, Collected

NPC1: This means that all 11 of us don't know where we are?

Molmont Carl: *shouting* DON'T SAY NONSENSE LIKE THAT!

Notification: A party has been formed Drag and drop the heroes

Notification:
•	Lyle (1 star) has joined 'Party 1'
•	Ranto (1 star) has joined 'Party 1'

TIPS: heroes without weapons carry an 'old iron sword (F)' into battle.

Goblin: *roar* Kyaaa!!

Han - narrating: My name is Islan Han.
Han - narrating: My real name is Han Seojin.
```
