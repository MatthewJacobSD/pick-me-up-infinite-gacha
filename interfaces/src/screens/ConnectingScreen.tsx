import { useState, useEffect } from "react";

interface ConnectingScreenProps {
  onComplete: () => void;
  duration?: number;
}

export default function ConnectingScreen({ onComplete, duration = 3000 }: ConnectingScreenProps) {
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const start = Date.now();
    const tick = () => {
      const elapsed = Date.now() - start;
      const pct = Math.min((elapsed / duration) * 100, 100);
      setProgress(pct);
      if (pct < 100) requestAnimationFrame(tick);
    };
    const raf = requestAnimationFrame(tick);
    const timeout = setTimeout(onComplete, duration);
    return () => {
      cancelAnimationFrame(raf);
      clearTimeout(timeout);
    };
  }, [onComplete, duration]);

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        gap: 32,
      }}
    >
      <div
        style={{
          fontFamily: "var(--font-decorative)",
          fontSize: "clamp(1.4rem, 4vw, 2.6rem)",
          fontWeight: 900,
          letterSpacing: 6,
          color: "var(--color-white)",
          textShadow: "0 0 20px rgba(200, 180, 255, 0.5), 0 0 40px rgba(120, 90, 200, 0.3)",
          textAlign: "center",
          lineHeight: 1.4,
        }}
      >
        PICK ME UP
      </div>

      <div style={{ width: "min(500px, 80vw)", display: "flex", flexDirection: "column", alignItems: "center", gap: 16 }}>
        <p style={{
          fontFamily: "var(--font-primary)",
          fontSize: "clamp(0.8rem, 2vw, 1rem)",
          fontWeight: 600,
          letterSpacing: 3,
          color: "var(--color-white-dim)",
          textShadow: "0 0 8px rgba(200, 180, 255, 0.3)",
        }}>
          CONNECTING TO THE GAME SERVER...
        </p>

        <div style={{
          width: "100%",
          height: 8,
          background: "rgba(20, 18, 32, 0.8)",
          borderRadius: 4,
          border: "1px solid rgba(200, 180, 255, 0.15)",
          overflow: "hidden",
          position: "relative",
        }}>
          <div style={{
            position: "absolute",
            inset: 0,
            background: "linear-gradient(90deg, rgba(120, 90, 200, 0.6), rgba(170, 140, 255, 0.8))",
            width: `${progress}%`,
            borderRadius: 4,
            transition: "width 0.1s linear",
            boxShadow: "0 0 12px rgba(150, 120, 230, 0.5)",
          }} />
        </div>

        <div style={{ display: "flex", gap: 8, marginTop: 4 }}>
          {[0, 1, 2, 3, 4].map((i) => (
            <div
              key={i}
              style={{
                width: 6,
                height: 6,
                borderRadius: "50%",
                background: "var(--color-purple-light)",
                opacity: progress > (i + 1) * 18 ? 0.9 : 0.2,
                transition: "opacity 0.3s",
              }}
            />
          ))}
        </div>
      </div>
    </div>
  );
}
