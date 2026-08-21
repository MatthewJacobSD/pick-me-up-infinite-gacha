export type DeviceType = "pc" | "console" | "mobile";

export type LoginProvider = "google" | "facebook" | "apple" | "guest";

export interface AuthState {
  provider: LoginProvider | null;
  username: string;
  characterId: string;
}

export type MainMenuPanel =
  | null
  | "settings"
  | "news"
  | "faq"
  | "support"
  | "quit";

export type ScreenId =
  | "device-select"
  | "initial"
  | "connecting"
  | "creating-account"
  | "login-select"
  | "authenticating"
  | "login-confirmation"
  | "name-input"
  | "name-confirmation"
  | "welcome"
  | "tutorial-choice"
  | "story-1"
  | "story-2"
  | "story-3"
  | "story-4"
  | "story-5"
  | "tutorial-quest"
  | "combat-info"
  | "summoning-prompt"
  | "complete"
  | "main-menu";

export type PanelSize = "narrow" | "medium" | "wide" | "full";
export type PanelAlignment = "center" | "left" | "right";
export type PanelVerticalPosition = "center" | "upper" | "lower";
export type OrnamentIntensity = "minimal" | "standard" | "dramatic";

export interface StoryScreenConfig {
  id: ScreenId;
  lines: string[];
  size: PanelSize;
  alignment: PanelAlignment;
  verticalPosition: PanelVerticalPosition;
  ornamentIntensity: OrnamentIntensity;
  lineSpacing: number;
  textOffset?: number;
  innerGlow?: boolean;
  showContinue?: boolean;
}
