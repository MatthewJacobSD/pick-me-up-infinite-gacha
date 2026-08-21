import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";
import type { StoryScreenConfig } from "../types";
import type { CSSProperties } from "react";

interface StoryScreenProps {
  config: StoryScreenConfig;
  onAdvance: () => void;
}

export default function StoryScreen({ config, onAdvance }: StoryScreenProps) {
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

  const verticalStyle: CSSProperties =
    config.verticalPosition === "upper"
      ? { marginTop: "-8vh" }
      : config.verticalPosition === "lower"
        ? { marginTop: "8vh" }
        : {};

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        cursor: config.showContinue ? "default" : "pointer",
        ...verticalStyle,
      }}
      onClick={!config.showContinue ? onAdvance : undefined}
    >
      <GothicFrame
        size={config.size}
        ornamentIntensity={config.ornamentIntensity}
        innerGlow={config.innerGlow}
      >
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems:
              config.alignment === "left"
                ? "flex-start"
                : config.alignment === "right"
                  ? "flex-end"
                  : "center",
            textAlign: config.alignment,
            padding: `${config.textOffset ?? 0}px 0 8px`,
          }}
        >
          {config.lines.map((line, i) => (
            <p
              key={i}
              style={{
                fontFamily: "var(--font-primary)",
                fontSize: "clamp(0.82rem, 2vw, 1.02rem)",
                fontWeight: 600,
                letterSpacing: 2,
                lineHeight: config.lineSpacing,
                color: "var(--color-white)",
                textShadow: "0 0 8px rgba(200, 180, 255, 0.3)",
                maxWidth: config.size === "narrow" ? 360 : 560,
              }}
            >
              {line}
            </p>
          ))}

          {config.showContinue && (
            <div style={{ marginTop: 32 }}>
              <GothicButton variant="primary" onClick={onAdvance}>
                Continue
              </GothicButton>
            </div>
          )}
        </div>
      </GothicFrame>
    </div>
  );
}
