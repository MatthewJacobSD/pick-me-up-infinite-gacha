import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import type { DeviceType } from "../types";

interface DeviceSelectScreenProps {
  onSelect: (device: DeviceType) => void;
}

const devices: { id: DeviceType; label: string; icon: string; hint: string }[] = [
  { id: "pc", label: "PC", icon: "\u2318", hint: "KEYBOARD & MOUSE" },
  { id: "console", label: "CONSOLE", icon: "\u25C6", hint: "CONTROLLER" },
  { id: "mobile", label: "MOBILE", icon: "\u25CF", hint: "TOUCH INPUT" },
];

export default function DeviceSelectScreen({ onSelect }: DeviceSelectScreenProps) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "1") onSelect("pc");
      if (e.key === "2") onSelect("console");
      if (e.key === "3") onSelect("mobile");
    },
    [onSelect]
  );

  useEffect(() => {
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [handleKeyDown]);

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        gap: 24,
      }}
    >
      <GothicFrame size="wide" ornamentIntensity="dramatic">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 24, padding: "12px 0" }}>
          <p style={{
            fontFamily: "var(--font-decorative)",
            fontSize: "clamp(1rem, 3vw, 1.6rem)",
            fontWeight: 700,
            letterSpacing: 6,
            color: "var(--color-white)",
            textShadow: "0 0 16px var(--color-text-glow)",
          }}>
            SELECT TESTING DEVICE
          </p>

          <div style={{ display: "flex", gap: 20, flexWrap: "wrap", justifyContent: "center" }}>
            {devices.map((d) => (
              <button
                key={d.id}
                onClick={() => onSelect(d.id)}
                style={{
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                  gap: 10,
                  padding: "24px 32px",
                  minWidth: 140,
                  background: "linear-gradient(170deg, #1a1728 0%, #110f1c 100%)",
                  border: "1.5px solid rgba(200, 180, 255, 0.25)",
                  borderRadius: 6,
                  cursor: "pointer",
                  transition: "all 0.3s ease",
                  boxShadow: "0 4px 20px rgba(0,0,0,0.4), inset 0 1px 0 rgba(232,228,245,0.08)",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.6)";
                  e.currentTarget.style.boxShadow = "0 4px 30px rgba(120,90,200,0.3), inset 0 1px 0 rgba(232,228,245,0.12)";
                  e.currentTarget.style.transform = "translateY(-2px)";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.25)";
                  e.currentTarget.style.boxShadow = "0 4px 20px rgba(0,0,0,0.4), inset 0 1px 0 rgba(232,228,245,0.08)";
                  e.currentTarget.style.transform = "translateY(0)";
                }}
              >
                <span style={{ fontSize: "2rem", color: "var(--color-purple-light)" }}>{d.icon}</span>
                <span style={{
                  fontFamily: "var(--font-primary)",
                  fontSize: "1.1rem",
                  fontWeight: 700,
                  letterSpacing: 3,
                  color: "var(--color-white)",
                }}>{d.label}</span>
                <span style={{
                  fontFamily: "var(--font-primary)",
                  fontSize: "0.65rem",
                  fontWeight: 500,
                  letterSpacing: 2,
                  color: "var(--color-white-dim)",
                  opacity: 0.6,
                }}>{d.hint}</span>
              </button>
            ))}
          </div>
        </div>
      </GothicFrame>
    </div>
  );
}
