import { useFlowState } from "./state/useFlowState";
import { storyScreens } from "./data/storyScreens";
import AmbientBackground from "./components/AmbientBackground";
import ScreenTransition from "./components/ScreenTransition";
import DeviceSelectScreen from "./screens/DeviceSelectScreen";
import InitialScreen from "./screens/InitialScreen";
import ConnectingScreen from "./screens/ConnectingScreen";
import CreatingAccountScreen from "./screens/CreatingAccountScreen";
import LoginSelectScreen from "./screens/LoginSelectScreen";
import AuthenticatingScreen from "./screens/AuthenticatingScreen";
import LoginConfirmationScreen from "./screens/LoginConfirmationScreen";
import NameInputScreen from "./screens/NameInputScreen";
import NameConfirmationScreen from "./screens/NameConfirmationScreen";
import WelcomeScreen from "./screens/WelcomeScreen";
import TutorialChoiceScreen from "./screens/TutorialChoiceScreen";
import StoryScreen from "./screens/StoryScreen";
import TutorialQuestScreen from "./screens/TutorialQuestScreen";
import CombatInfoScreen from "./screens/CombatInfoScreen";
import SummoningPromptScreen from "./screens/SummoningPromptScreen";
import MainMenuScreen from "./screens/MainMenuScreen";
import GameExitedScreen from "./screens/GameExitedScreen";

export default function App() {
  const {
    currentScreen,
    playerName,
    setPlayerName,
    transitioning,
    transitionTo,
    goNext,
    goBack,
    goForwardFromTutorial,
    advanceStory,
    isValidStoryScreen,
    device,
    setDevice,
    auth,
    startMockAuth,
    completeAuth,
    mainMenuPanel,
    setMainMenuPanel,
  } = useFlowState();

  const renderScreen = () => {
    switch (currentScreen) {
      case "device-select":
        return <DeviceSelectScreen onSelect={(d) => { setDevice(d); goNext(); }} />;

      case "initial":
        return <InitialScreen onBegin={goNext} />;

      case "connecting":
        return <ConnectingScreen onComplete={goNext} duration={2800} />;

      case "creating-account":
        return <CreatingAccountScreen onComplete={goNext} duration={2500} />;

      case "login-select":
        return <LoginSelectScreen device={device} onSelect={startMockAuth} onBack={goBack} />;

      case "authenticating":
        return <AuthenticatingScreen provider={auth.provider!} onComplete={completeAuth} />;

      case "login-confirmation":
        return <LoginConfirmationScreen auth={auth} onContinue={goNext} />;

      case "name-input":
        return (
          <NameInputScreen
            currentName={playerName}
            onNameChange={setPlayerName}
            onConfirm={goNext}
            onClear={() => setPlayerName("")}
          />
        );

      case "name-confirmation":
        return (
          <NameConfirmationScreen
            playerName={playerName}
            onConfirm={goNext}
            onBack={goBack}
          />
        );

      case "welcome":
        return <WelcomeScreen playerName={playerName} onContinue={goNext} />;

      case "tutorial-choice":
        return <TutorialChoiceScreen onChoice={goForwardFromTutorial} />;

      case "tutorial-quest":
        return <TutorialQuestScreen onAdvance={advanceStory} />;

      case "combat-info":
        return <CombatInfoScreen onAdvance={advanceStory} />;

      case "summoning-prompt":
        return (
          <SummoningPromptScreen
            playerName={playerName}
            onAdvance={advanceStory}
          />
        );

      case "main-menu":
        return (
          <MainMenuScreen
            auth={auth}
            device={device}
            panel={mainMenuPanel}
            onPanelChange={setMainMenuPanel}
            onStartGame={() => {}}
            onQuit={() => { setMainMenuPanel(null); transitionTo("complete"); }}
          />
        );

      case "complete":
        return <GameExitedScreen />;

      default:
        if (isValidStoryScreen) {
          const config = storyScreens.find((s) => s.id === currentScreen);
          if (config) {
            return <StoryScreen config={config} onAdvance={advanceStory} />;
          }
        }
        return null;
    }
  };

  return (
    <div
      style={{
        position: "relative",
        width: "100vw",
        height: "100vh",
        overflow: "hidden",
      }}
    >
      <AmbientBackground />
      <ScreenTransition
        transitioning={transitioning}
        screenKey={currentScreen}
      >
        {renderScreen()}
      </ScreenTransition>
    </div>
  );
}
