import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";

export default function CombatInfoScreen({
  onAdvance,
}: {
  onAdvance: () => void;
}) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter" || e.key === " ") {
        e.preventDefault();
        onAdvance();
      }
      if (e.key === "Escape") onAdvance();
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
        cursor: "pointer",
      }}
      onClick={onAdvance}
    >
      <GothicFrame
        size="wide"
        ornamentIntensity="standard"
        innerGlow
      >
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 12,
            padding: "24px 0",
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
            COMBAT WILL AUTOMATICALLY PROCEED.
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
            WATCH A HIGH CLASS BATTLE
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
            MADE BY THE HERO&apos;S AI!
          </p>
          <p
            style={{
              fontFamily: "var(--font-primary)",
              fontSize: "clamp(0.7rem, 1.5vw, 0.82rem)",
              fontWeight: 500,
              letterSpacing: 2,
              color: "var(--color-purple-light)",
              textAlign: "center",
              lineHeight: 1.7,
              marginTop: 8,
            }}
          >
            CLICK TO CONTINUE
          </p>
        </div>
      </GothicFrame>
    </div>
  );
}
