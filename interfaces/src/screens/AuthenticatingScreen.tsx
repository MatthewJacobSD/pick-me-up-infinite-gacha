import { useState, useEffect } from "react";
import type { LoginProvider } from "../types";

interface AuthenticatingScreenProps {
  provider: LoginProvider;
  onComplete: () => void;
}

const PHASES = [
  { text: "CONNECTING...", dur: 600 },
  { text: "AUTHENTICATING...", dur: 600 },
  { text: "LOGIN SUCCESSFUL", dur: 500 },
];

export default function AuthenticatingScreen({ provider, onComplete }: AuthenticatingScreenProps) {
  const [phaseIdx, setPhaseIdx] = useState(0);

  useEffect(() => {
    let timeout: ReturnType<typeof setTimeout>;
    if (phaseIdx < PHASES.length) {
      timeout = setTimeout(() => setPhaseIdx((p) => p + 1), PHASES[phaseIdx].dur);
    } else {
      timeout = setTimeout(onComplete, 300);
    }
    return () => clearTimeout(timeout);
  }, [phaseIdx, onComplete]);

  const providerLabel = provider.charAt(0).toUpperCase() + provider.slice(1);

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
      <div style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        gap: 24,
      }}>
        <div style={{
          width: 60,
          height: 60,
          borderRadius: "50%",
          border: "2px solid rgba(200, 180, 255, 0.3)",
          borderTopColor: "var(--color-purple-light)",
          animation: "spin 1s linear infinite",
        }} />

        <p style={{
          fontFamily: "var(--font-primary)",
          fontSize: "clamp(0.85rem, 2vw, 1.1rem)",
          fontWeight: 700,
          letterSpacing: 4,
          color: "var(--color-white)",
          textShadow: "0 0 10px var(--color-text-glow)",
          textAlign: "center",
        }}>
          {phaseIdx < PHASES.length ? PHASES[phaseIdx].text : "LOGIN SUCCESSFUL"}
        </p>

        <p style={{
          fontFamily: "var(--font-primary)",
          fontSize: "clamp(0.65rem, 1.5vw, 0.78rem)",
          fontWeight: 500,
          letterSpacing: 2,
          color: "var(--color-purple-light)",
          opacity: 0.7,
        }}>
          {providerLabel}
        </p>
      </div>
    </div>
  );
}
