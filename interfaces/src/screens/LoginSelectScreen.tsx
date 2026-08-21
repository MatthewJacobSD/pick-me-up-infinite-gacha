import { useCallback, useEffect } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";
import type { LoginProvider, DeviceType } from "../types";

interface LoginSelectScreenProps {
  device: DeviceType;
  onSelect: (provider: LoginProvider) => void;
  onBack: () => void;
}

function ProviderIcon({ provider }: { provider: LoginProvider }) {
  const icons: Record<LoginProvider, string> = {
    google: "G",
    facebook: "f",
    apple: "\uF8FF",
    guest: "\u25C7",
  };
  return (
    <span style={{
      display: "inline-flex",
      alignItems: "center",
      justifyContent: "center",
      width: 32,
      height: 32,
      borderRadius: "50%",
      background: provider === "google" ? "#4285f4"
        : provider === "facebook" ? "#1877f2"
        : provider === "apple" ? "#ffffff"
        : "rgba(160, 136, 224, 0.3)",
      color: provider === "apple" ? "#000" : "#fff",
      fontFamily: provider === "apple" ? "system-ui, sans-serif" : "var(--font-primary)",
      fontSize: provider === "apple" ? "1.2rem" : "0.9rem",
      fontWeight: 700,
    }}>
      {icons[provider]}
    </span>
  );
}

export default function LoginSelectScreen({ device, onSelect, onBack }: LoginSelectScreenProps) {
  const providers: { id: LoginProvider; label: string }[] = [
    { id: "google", label: "CONTINUE WITH GOOGLE" },
    { id: "facebook", label: "CONTINUE WITH FACEBOOK" },
    ...(device === "mobile" || device === "pc"
      ? [{ id: "apple" as LoginProvider, label: "CONTINUE WITH APPLE" }]
      : []),
    { id: "guest", label: "CONTINUE AS GUEST" },
  ];

  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Escape") onBack();
    },
    [onBack]
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
      <GothicFrame size="medium" ornamentIntensity="dramatic">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 20, padding: "8px 0" }}>
          <p style={{
            fontFamily: "var(--font-decorative)",
            fontSize: "clamp(0.9rem, 2.5vw, 1.3rem)",
            fontWeight: 700,
            letterSpacing: 5,
            color: "var(--color-white)",
            textShadow: "0 0 14px var(--color-text-glow)",
          }}>
            CONNECT TO ACCOUNT
          </p>
          <p style={{
            fontFamily: "var(--font-primary)",
            fontSize: "clamp(0.65rem, 1.5vw, 0.78rem)",
            fontWeight: 500,
            letterSpacing: 2,
            color: "var(--color-white-dim)",
            opacity: 0.7,
          }}>
            SELECT YOUR LOGIN METHOD
          </p>

          <div style={{ display: "flex", flexDirection: "column", gap: 10, width: "100%", maxWidth: 320 }}>
            {providers.map((p) => (
              <button
                key={p.id}
                onClick={() => onSelect(p.id)}
                style={{
                  display: "flex",
                  alignItems: "center",
                  gap: 14,
                  padding: "14px 20px",
                  background: "linear-gradient(170deg, #1a1728 0%, #110f1c 100%)",
                  border: "1px solid rgba(200, 180, 255, 0.2)",
                  borderRadius: 4,
                  cursor: "pointer",
                  transition: "all 0.25s ease",
                  width: "100%",
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.5)";
                  e.currentTarget.style.boxShadow = "0 2px 16px rgba(120, 90, 200, 0.25)";
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.2)";
                  e.currentTarget.style.boxShadow = "none";
                }}
              >
                <ProviderIcon provider={p.id} />
                <span style={{
                  fontFamily: "var(--font-primary)",
                  fontSize: "clamp(0.75rem, 1.8vw, 0.9rem)",
                  fontWeight: 600,
                  letterSpacing: 2,
                  color: "var(--color-white)",
                }}>{p.label}</span>
              </button>
            ))}
          </div>

          <div style={{ display: "flex", alignItems: "center", gap: 12, marginTop: 4, width: "100%", maxWidth: 320 }}>
            <div style={{ flex: 1, height: 1, background: "rgba(200, 180, 255, 0.15)" }} />
            <span style={{
              fontFamily: "var(--font-primary)",
              fontSize: "0.7rem",
              letterSpacing: 2,
              color: "var(--color-white-dim)",
              opacity: 0.5,
            }}>OR</span>
            <div style={{ flex: 1, height: 1, background: "rgba(200, 180, 255, 0.15)" }} />
          </div>

          <GothicButton variant="secondary" onClick={onBack}>
            Back
          </GothicButton>
        </div>
      </GothicFrame>
    </div>
  );
}
