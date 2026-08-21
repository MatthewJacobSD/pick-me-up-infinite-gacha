import { useEffect, useCallback } from "react";
import GothicFrame from "../components/GothicFrame";

interface WelcomeScreenProps {
  playerName: string;
  onContinue: () => void;
  duration?: number;
}

export default function WelcomeScreen({
  playerName,
  onContinue,
  duration = 3500,
}: WelcomeScreenProps) {
  const handleClick = useCallback(() => {
    onContinue();
  }, [onContinue]);

  useEffect(() => {
    const timeout = setTimeout(onContinue, duration);
    window.addEventListener("click", handleClick);
    window.addEventListener("keydown", handleClick);
    return () => {
      clearTimeout(timeout);
      window.removeEventListener("click", handleClick);
      window.removeEventListener("keydown", handleClick);
    };
  }, [onContinue, duration, handleClick]);

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        cursor: "pointer",
      }}
    >
      <GothicFrame size="wide" ornamentIntensity="dramatic" glowIntensity={0.6}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 8,
            padding: "20px 0",
          }}
        >
          <h1
            style={{
              fontFamily: "var(--font-decorative)",
              fontSize: "clamp(1.3rem, 4vw, 2.2rem)",
              fontWeight: 900,
              letterSpacing: 5,
              color: "var(--color-white)",
              textShadow:
                "0 0 16px rgba(200, 180, 255, 0.5), 0 0 32px rgba(160, 136, 224, 0.3)",
              lineHeight: 1.5,
              textAlign: "center",
            }}
          >
            <span style={{ color: "var(--color-purple-light)" }}>
              {playerName}
            </span>
            , WELCOME TO
          </h1>
          <h1
            style={{
              fontFamily: "var(--font-decorative)",
              fontSize: "clamp(1.3rem, 4vw, 2.2rem)",
              fontWeight: 900,
              letterSpacing: 5,
              color: "var(--color-white)",
              textShadow:
                "0 0 16px rgba(200, 180, 255, 0.5), 0 0 32px rgba(160, 136, 224, 0.3)",
              lineHeight: 1.5,
              textAlign: "center",
            }}
          >
            THE WORLD OF PICK ME UP!
          </h1>
        </div>
      </GothicFrame>
    </div>
  );
}
