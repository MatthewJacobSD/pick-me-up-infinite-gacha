import { useCallback, useEffect } from "react";
import MissionBanner from "../components/MissionBanner";

export default function TutorialQuestScreen({
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
        perspective: "1200px",
      }}
      onClick={onAdvance}
    >
      <MissionBanner
        floor={1}
        category="GOBLIN HUNT"
        goal="TAKE DOWN THE GOBLINS THAT INVADED THE TOWN!"
      />
    </div>
  );
}
