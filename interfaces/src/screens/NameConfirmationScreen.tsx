import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";

interface NameConfirmationScreenProps {
  playerName: string;
  onConfirm: () => void;
  onBack: () => void;
}

export default function NameConfirmationScreen({
  playerName,
  onConfirm,
  onBack,
}: NameConfirmationScreenProps) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter") onConfirm();
      if (e.key === "Escape") onBack();
    },
    [onConfirm, onBack]
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
      <GothicFrame size="medium" ornamentIntensity="standard">
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 24,
            padding: "16px 0",
          }}
        >
          <div
            style={{
              fontFamily: "var(--font-decorative)",
              fontSize: "clamp(1.8rem, 5vw, 2.8rem)",
              fontWeight: 900,
              letterSpacing: 8,
              color: "var(--color-white)",
              textShadow:
                "0 0 20px rgba(200, 180, 255, 0.5), 0 0 40px rgba(160, 136, 224, 0.25)",
              animation: "textGlowPulse 3s ease-in-out infinite",
            }}
          >
            {playerName}
          </div>

          <div
            style={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              gap: 4,
            }}
          >
            <p
              style={{
                fontSize: "clamp(0.8rem, 1.8vw, 0.95rem)",
                fontWeight: 600,
                letterSpacing: 2,
                color: "var(--color-white-dim)",
                opacity: 0.85,
              }}
            >
              YOU CAN USE THIS NAME.
            </p>
            <p
              style={{
                fontSize: "clamp(0.8rem, 1.8vw, 0.95rem)",
                fontWeight: 600,
                letterSpacing: 2,
                color: "var(--color-white-dim)",
                opacity: 0.85,
              }}
            >
              WILL YOU USE IT?
            </p>
          </div>

          <div
            style={{
              display: "flex",
              gap: 20,
              marginTop: 12,
            }}
          >
            <GothicButton variant="primary" onClick={onConfirm}>
              Yes
            </GothicButton>
            <GothicButton variant="secondary" onClick={onBack}>
              No
            </GothicButton>
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
