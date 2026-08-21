import { useEffect } from "react";

interface InitialScreenProps {
  onBegin: () => void;
}

export default function InitialScreen({ onBegin }: InitialScreenProps) {
  useEffect(() => {
    const handler = () => onBegin();
    window.addEventListener("click", handler);
    window.addEventListener("keydown", handler);
    return () => {
      window.removeEventListener("click", handler);
      window.removeEventListener("keydown", handler);
    };
  }, [onBegin]);

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        cursor: "pointer",
        userSelect: "none",
      }}
    >
      <p
        style={{
          fontFamily: "var(--font-primary)",
          fontSize: "clamp(0.85rem, 2vw, 1.1rem)",
          fontWeight: 500,
          letterSpacing: 4,
          textTransform: "uppercase",
          color: "var(--color-white-dim)",
          opacity: 0.45,
          animation: "subtlePulse 3s ease-in-out infinite",
          textShadow: "0 0 12px rgba(160, 136, 224, 0.3)",
        }}
      >
        Click anywhere to begin
      </p>
    </div>
  );
}
