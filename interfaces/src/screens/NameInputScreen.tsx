import { useEffect, useCallback } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";
import NameInput from "../components/NameInput";

interface NameInputScreenProps {
  currentName: string;
  onNameChange: (name: string) => void;
  onConfirm: () => void;
  onClear: () => void;
}

const VALID_NAME = /^[A-Za-z]{2,6}$/;

export default function NameInputScreen({
  currentName,
  onNameChange,
  onConfirm,
  onClear,
}: NameInputScreenProps) {
  const isValid = VALID_NAME.test(currentName.trim());

  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter" && isValid) {
        onConfirm();
      }
    },
    [isValid, onConfirm]
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
            padding: "12px 0",
          }}
        >
          <h2
            style={{
              fontFamily: "var(--font-decorative)",
              fontSize: "clamp(1.1rem, 3vw, 1.6rem)",
              fontWeight: 700,
              letterSpacing: 5,
              color: "var(--color-white)",
              textShadow: "0 0 12px var(--color-text-glow)",
            }}
          >
            CHOOSE YOUR NAME
          </h2>

          <p
            style={{
              fontSize: "clamp(0.7rem, 1.5vw, 0.82rem)",
              fontWeight: 500,
              letterSpacing: 2,
              color: "var(--color-white-dim)",
              opacity: 0.7,
              lineHeight: 1.6,
            }}
          >
            2~6 CHARACTERS, SPACES/SPECIAL CHARACTERS ARE NOT ALLOWED.
          </p>

          <NameInput
            value={currentName}
            onChange={onNameChange}
            maxLength={6}
          />

          <div
            style={{
              display: "flex",
              gap: 20,
              marginTop: 20,
            }}
          >
            <GothicButton
              variant="primary"
              disabled={!isValid}
              onClick={onConfirm}
            >
              Yes
            </GothicButton>
            <GothicButton variant="secondary" onClick={onClear}>
              No
            </GothicButton>
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
