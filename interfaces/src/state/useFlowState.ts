import { useState, useCallback, useRef } from "react";
import type { ScreenId, DeviceType, LoginProvider, AuthState, MainMenuPanel } from "../types";
import { register, consumeLoginCode } from "../api";

const SCREEN_ORDER: ScreenId[] = [
  "device-select",
  "initial",
  "connecting",
  "creating-account",
  "login-select",
  "authenticating",
  "login-confirmation",
  "name-input",
  "name-confirmation",
  "welcome",
  "tutorial-choice",
  "story-1",
  "story-2",
  "story-3",
  "story-4",
  "story-5",
  "tutorial-quest",
  "combat-info",
  "summoning-prompt",
  "complete",
  "main-menu",
];

const STORY_SCREENS: ScreenId[] = [
  "story-1", "story-2", "story-3", "story-4", "story-5",
  "tutorial-quest", "combat-info", "summoning-prompt",
];

function randomUsername(): string {
  const names = ["ShadowKnight", "Nightfall", "VoidWalker", "CrimsonBlade", "StormCaller", "DarkRaven"];
  return names[Math.floor(Math.random() * names.length)];
}

export function useFlowState() {
  const [currentScreen, setCurrentScreen] = useState<ScreenId>("device-select");
  const [playerName, setPlayerName] = useState("");
  const [transitioning, setTransitioning] = useState(false);
  const [device, setDevice] = useState<DeviceType>("pc");
  const [auth, setAuth] = useState<AuthState>({ provider: null, username: "", characterId: "" });
  const [mainMenuPanel, setMainMenuPanel] = useState<MainMenuPanel>(null);
  const lockRef = useRef(false);

  const transitionTo = useCallback((screen: ScreenId, delay = 400) => {
    if (lockRef.current) return;
    lockRef.current = true;
    setTransitioning(true);
    setTimeout(() => {
      setCurrentScreen(screen);
      setTimeout(() => { setTransitioning(false); lockRef.current = false; }, 50);
    }, delay);
  }, []);

  const goNext = useCallback(() => {
    const idx = SCREEN_ORDER.indexOf(currentScreen);
    if (idx < SCREEN_ORDER.length - 1) transitionTo(SCREEN_ORDER[idx + 1]);
  }, [currentScreen, transitionTo]);

  const goBack = useCallback(() => {
    if (currentScreen === "name-confirmation") transitionTo("name-input");
    else if (currentScreen === "tutorial-choice") transitionTo("welcome");
    else if (currentScreen === "login-select") transitionTo("initial");
    else if (currentScreen === "name-input") transitionTo("login-confirmation");
    else { const idx = SCREEN_ORDER.indexOf(currentScreen); if (idx > 0) transitionTo(SCREEN_ORDER[idx - 1]); }
  }, [currentScreen, transitionTo]);

  const goForwardFromTutorial = useCallback((choice: "yes" | "no") => {
    transitionTo(choice === "no" ? "main-menu" : "story-1");
  }, [transitionTo]);

  const advanceStory = useCallback(() => {
    const idx = STORY_SCREENS.indexOf(currentScreen);
    transitionTo(idx < STORY_SCREENS.length - 1 ? STORY_SCREENS[idx + 1] : "main-menu");
  }, [currentScreen, transitionTo]);

  const startOAuthLogin = useCallback((provider: "google" | "facebook") => {
    setAuth({ provider, username: "", characterId: "" });
    transitionTo("authenticating");
  }, [transitionTo]);

  const startRegister = useCallback(async (email: string, password: string) => {
    setAuth({ provider: null, username: "", characterId: "" });
    transitionTo("authenticating");
    try {
      const result = await register(email, password);
      setAuth({ provider: null, username: result.username, characterId: result.publicCode });
    } catch { /* handled by caller */ }
  }, [transitionTo]);

  const completeAuth = useCallback(() => {
    if (!auth.username) setAuth(prev => ({ ...prev, username: randomUsername() }));
    transitionTo("login-confirmation");
  }, [auth, transitionTo]);

  const isValidStoryScreen = STORY_SCREENS.includes(currentScreen);

  return {
    currentScreen, playerName, setPlayerName, transitioning, transitionTo,
    goNext, goBack, goForwardFromTutorial, advanceStory, isValidStoryScreen,
    device, setDevice, auth, startOAuthLogin, startRegister, completeAuth,
    mainMenuPanel, setMainMenuPanel,
  };
}
