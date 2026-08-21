import type { StoryScreenConfig } from "../types";

export const storyScreens: StoryScreenConfig[] = [
  {
    id: "story-1",
    lines: [
      "IN A SMALL TOWN OF THE HIME PROVINCE,",
      'THERE WAS A BOY NAMED "HAN ISLAT."',
    ],
    size: "wide",
    alignment: "center",
    verticalPosition: "center",
    ornamentIntensity: "standard",
    lineSpacing: 1.8,
  },
  {
    id: "story-2",
    lines: [
      "TOWNIA, THE GROUND WHERE HUMANS LIVE",
      "ALONGSIDE SPECIES.",
    ],
    size: "medium",
    alignment: "left",
    verticalPosition: "center",
    ornamentIntensity: "standard",
    lineSpacing: 2.2,
  },
  {
    id: "story-3",
    lines: [
      "AN UNKNOWN ENEMY",
      "INVADES THIS PEACEFUL LAND!",
    ],
    size: "narrow",
    alignment: "center",
    verticalPosition: "center",
    ornamentIntensity: "standard",
    lineSpacing: 2.4,
  },
  {
    id: "story-4",
    lines: [
      "YOU, MASTER!",
      "IF YOU WISH TO SAVE THE WORLD,",
      "CLIMB THE TOWER!",
    ],
    size: "wide",
    alignment: "center",
    verticalPosition: "lower",
    ornamentIntensity: "standard",
    lineSpacing: 1.8,
    innerGlow: true,
    textOffset: 12,
  },
  {
    id: "story-5",
    lines: [
      "MANY HEROES WILL JOIN YOU.",
    ],
    size: "wide",
    alignment: "center",
    verticalPosition: "center",
    ornamentIntensity: "dramatic",
    lineSpacing: 2.0,
    showContinue: true,
  },
];
