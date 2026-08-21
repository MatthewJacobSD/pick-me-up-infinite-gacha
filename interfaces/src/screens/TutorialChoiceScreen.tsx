import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";

interface TutorialChoiceScreenProps {
  onChoice: (choice: "yes" | "no") => void;
}

export default function TutorialChoiceScreen({
  onChoice,
}: TutorialChoiceScreenProps) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Enter") onChoice("yes");
      if (e.key === "Escape") onChoice("no");
    },
    [onChoice]
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
            gap: 20,
            padding: "16px 0",
          }}
        >
          <div
            style={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              gap: 6,
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
              WILL YOU START THE TUTORIAL?
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
              }}
            >
              ON COMPLETION, YOU WILL RECEIVE
              <br />A PREDETERMINED REWARD.
            </p>
          </div>

          <div
            style={{
              display: "flex",
              gap: 20,
              marginTop: 12,
            }}
          >
            <GothicButton variant="primary" onClick={() => onChoice("yes")}>
              Yes
            </GothicButton>
            <GothicButton variant="secondary" onClick={() => onChoice("no")}>
              No
            </GothicButton>
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
