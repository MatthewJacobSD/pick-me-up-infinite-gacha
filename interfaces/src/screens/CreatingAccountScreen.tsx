import { useEffect, useState } from "react";
import GothicFrame from "../components/GothicFrame";
import LoadingDots from "../components/LoadingDots";

interface CreatingAccountScreenProps {
  onComplete: () => void;
  duration?: number;
}

export default function CreatingAccountScreen({
  onComplete,
  duration = 2500,
}: CreatingAccountScreenProps) {
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setProgress((p) => Math.min(p + 1, 100));
    }, duration / 100);

    const timeout = setTimeout(() => {
      onComplete();
    }, duration);

    return () => {
      clearInterval(interval);
      clearTimeout(timeout);
    };
  }, [onComplete, duration]);

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
      <GothicFrame size="wide" ornamentIntensity="standard">
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 28,
            padding: "8px 0",
          }}
        >
          <h2
            style={{
              fontFamily: "var(--font-decorative)",
              fontSize: "clamp(1.2rem, 3.5vw, 1.8rem)",
              fontWeight: 700,
              letterSpacing: 6,
              color: "var(--color-white)",
              textShadow: "0 0 14px var(--color-text-glow)",
            }}
          >
            CREATING YOUR ACCOUNT.
          </h2>

          <LoadingDots count={3} size={10} />

          <div
            style={{
              width: "80%",
              maxWidth: 320,
              height: 3,
              background: "rgba(100, 80, 170, 0.15)",
              borderRadius: 2,
              overflow: "hidden",
            }}
          >
            <div
              style={{
                width: `${progress}%`,
                height: "100%",
                background:
                  "linear-gradient(90deg, var(--color-purple-mid), var(--color-purple-light))",
                borderRadius: 2,
                transition: "width 30ms linear",
                boxShadow: "0 0 8px var(--color-purple-glow)",
              }}
            />
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
