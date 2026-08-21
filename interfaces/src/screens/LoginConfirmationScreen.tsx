import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import type { AuthState } from "../types";

interface LoginConfirmationScreenProps {
  auth: AuthState;
  onContinue: () => void;
}

export default function LoginConfirmationScreen({ auth, onContinue }: LoginConfirmationScreenProps) {
  const providerLabel = auth.provider
    ? auth.provider.charAt(0).toUpperCase() + auth.provider.slice(1)
    : "Guest";

  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter" || e.key === " ") {
        e.preventDefault();
        onContinue();
      }
    },
    [onContinue]
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
        cursor: "pointer",
      }}
      onClick={onContinue}
    >
      <GothicFrame size="medium" ornamentIntensity="dramatic" innerGlow>
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 16, padding: "16px 0" }}>
          <p style={{
            fontFamily: "var(--font-decorative)",
            fontSize: "clamp(1rem, 2.5vw, 1.4rem)",
            fontWeight: 700,
            letterSpacing: 5,
            color: "var(--color-white)",
            textShadow: "0 0 14px var(--color-text-glow)",
          }}>
            LOGGED IN
          </p>

          <div style={{
            width: "80%",
            height: 1,
            background: "linear-gradient(90deg, transparent, rgba(200, 180, 255, 0.3), transparent)",
          }} />

          <p style={{
            fontFamily: "var(--font-primary)",
            fontSize: "clamp(0.8rem, 2vw, 1rem)",
            fontWeight: 600,
            letterSpacing: 3,
            color: "var(--color-purple-light)",
          }}>
            LOGGED IN WITH {providerLabel.toUpperCase()}
          </p>

          <p style={{
            fontFamily: "var(--font-decorative)",
            fontSize: "clamp(1.2rem, 3.5vw, 2rem)",
            fontWeight: 900,
            letterSpacing: 4,
            color: "var(--color-white)",
            textShadow: "0 0 16px rgba(200, 180, 255, 0.5)",
          }}>
            {auth.username}
          </p>

          <p style={{
            fontFamily: "var(--font-primary)",
            fontSize: "clamp(0.6rem, 1.3vw, 0.72rem)",
            fontWeight: 500,
            letterSpacing: 2,
            color: "var(--color-white-dim)",
            opacity: 0.6,
          }}>
            CHARACTER ID: {auth.characterId}
          </p>
        </div>
      </GothicFrame>
    </div>
  );
}
