import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";

interface SummoningPromptScreenProps {
  playerName: string;
  onAdvance: () => void;
}

export default function SummoningPromptScreen({
  playerName,
  onAdvance,
}: SummoningPromptScreenProps) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter" || e.key === " ") {
        e.preventDefault();
        onAdvance();
      }
    },
    [onAdvance]
  );

  useEffect(() => {
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [handleKeyDown]);

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
      }}
    >
      <GothicFrame size="wide" ornamentIntensity="dramatic" glowIntensity={0.5}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 16,
            padding: "20px 0",
          }}
        >
          <p
            style={{
              fontFamily: "var(--font-primary)",
              fontSize: "clamp(0.85rem, 2vw, 1.05rem)",
              fontWeight: 700,
              letterSpacing: 3,
              color: "var(--color-white)",
              textShadow: "0 0 10px var(--color-text-glow)",
              textAlign: "center",
              lineHeight: 1.7,
            }}
          >
            <span style={{ color: "var(--color-purple-light)" }}>
              {playerName}
            </span>
            , WILL YOU PULL FOR A COMRADE
          </p>
          <p
            style={{
              fontFamily: "var(--font-primary)",
              fontSize: "clamp(0.85rem, 2vw, 1.05rem)",
              fontWeight: 700,
              letterSpacing: 3,
              color: "var(--color-white)",
              textShadow: "0 0 10px var(--color-text-glow)",
              textAlign: "center",
              lineHeight: 1.7,
            }}
          >
            BEFORE THE NEXT STAGE?
          </p>
          <p
            style={{
              fontFamily: "var(--font-primary)",
              fontSize: "clamp(0.7rem, 1.5vw, 0.82rem)",
              fontWeight: 500,
              letterSpacing: 2,
              color: "var(--color-white-dim)",
              opacity: 0.75,
              textAlign: "center",
              lineHeight: 1.7,
              marginTop: 4,
            }}
          >
            CLICK THE &ldquo;SUMMON&rdquo; TAB IN THE MENU!
            <br />
            THE FIRST SUMMON IS FREE!
            <br />
            500 GEMS FOR HIGH RANK SUMMON HAVE BEEN GIVEN.
          </p>
          <div style={{ marginTop: 12 }}>
            <GothicButton variant="primary" onClick={onAdvance}>
              Continue
            </GothicButton>
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
