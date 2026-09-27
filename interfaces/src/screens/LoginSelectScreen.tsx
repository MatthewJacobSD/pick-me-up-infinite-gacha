import { useCallback, useEffect, useState } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";
import type { DeviceType } from "../types";
import { getLoginUrl } from "../api";

interface LoginSelectScreenProps {
  device: DeviceType;
  onSelect: (provider: "google" | "facebook") => void;
  onRegister: (email: string, password: string) => void;
  onBack: () => void;
}

export default function LoginSelectScreen({ device, onSelect, onRegister, onBack }: LoginSelectScreenProps) {
  const [mode, setMode] = useState<"providers" | "register">("providers");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleProviderLogin = async (provider: "google" | "facebook") => {
    try {
      const { redirectUrl } = await getLoginUrl(provider);
      window.location.href = redirectUrl;
    } catch {
      onSelect(provider);
    }
  };

  const handleRegister = () => {
    if (!email || !password) {
      setError("Email and password are required.");
      return;
    }
    if (password.length < 12) {
      setError("Password must be at least 12 characters.");
      return;
    }
    setError("");
    onRegister(email, password);
  };

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

  const inputStyle = {
    width: "100%",
    padding: "12px 16px",
    background: "rgba(15, 12, 25, 0.8)",
    border: "1px solid rgba(200, 180, 255, 0.2)",
    borderRadius: 4,
    color: "#e8e0ff",
    fontFamily: "var(--font-primary)",
    fontSize: "0.85rem",
    letterSpacing: 1,
    outline: "none",
    boxSizing: "border-box" as const,
  };

  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", width: "100%", height: "100%" }}>
      <GothicFrame size="medium" ornamentIntensity="dramatic">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 20, padding: "8px 0" }}>
          <img
            src="/logo/apple-touch-icon.jpg"
            alt="Moebius Order"
            style={{ width: 64, height: 64, borderRadius: 12, objectFit: "cover" }}
          />

          <p style={{
            fontFamily: "var(--font-decorative)",
            fontSize: "clamp(0.9rem, 2.5vw, 1.3rem)",
            fontWeight: 700,
            letterSpacing: 5,
            color: "var(--color-white)",
            textShadow: "0 0 14px var(--color-text-glow)",
          }}>
            {mode === "providers" ? "CONNECT TO ACCOUNT" : "CREATE ACCOUNT"}
          </p>

          {mode === "providers" ? (
            <>
              <div style={{ display: "flex", flexDirection: "column", gap: 10, width: "100%", maxWidth: 320 }}>
                <button onClick={() => handleProviderLogin("google")} style={{
                  display: "flex", alignItems: "center", gap: 14, padding: "14px 20px",
                  background: "linear-gradient(170deg, #1a1728 0%, #110f1c 100%)",
                  border: "1px solid rgba(200, 180, 255, 0.2)", borderRadius: 4, cursor: "pointer",
                  width: "100%", transition: "all 0.25s ease",
                }}>
                  <span style={{ display: "inline-flex", alignItems: "center", justifyContent: "center", width: 32, height: 32, borderRadius: "50%", background: "#4285f4", color: "#fff", fontWeight: 700 }}>G</span>
                  <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.9rem", fontWeight: 600, letterSpacing: 2, color: "var(--color-white)" }}>CONTINUE WITH GOOGLE</span>
                </button>

                <button onClick={() => handleProviderLogin("facebook")} style={{
                  display: "flex", alignItems: "center", gap: 14, padding: "14px 20px",
                  background: "linear-gradient(170deg, #1a1728 0%, #110f1c 100%)",
                  border: "1px solid rgba(200, 180, 255, 0.2)", borderRadius: 4, cursor: "pointer",
                  width: "100%", transition: "all 0.25s ease",
                }}>
                  <span style={{ display: "inline-flex", alignItems: "center", justifyContent: "center", width: 32, height: 32, borderRadius: "50%", background: "#1877f2", color: "#fff", fontWeight: 700 }}>f</span>
                  <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.9rem", fontWeight: 600, letterSpacing: 2, color: "var(--color-white)" }}>CONTINUE WITH FACEBOOK</span>
                </button>
              </div>

              <div style={{ display: "flex", alignItems: "center", gap: 12, marginTop: 4, width: "100%", maxWidth: 320 }}>
                <div style={{ flex: 1, height: 1, background: "rgba(200, 180, 255, 0.15)" }} />
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.7rem", letterSpacing: 2, color: "var(--color-white-dim)", opacity: 0.5 }}>OR</span>
                <div style={{ flex: 1, height: 1, background: "rgba(200, 180, 255, 0.15)" }} />
              </div>

              <GothicButton variant="secondary" onClick={() => setMode("register")}>
                Register with Email
              </GothicButton>
            </>
          ) : (
            <>
              <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} style={inputStyle} />
              <input type="password" placeholder="Password (12+ chars)" value={password} onChange={(e) => setPassword(e.target.value)} style={inputStyle} />
              {error && <p style={{ color: "#ff6b6b", fontSize: "0.75rem", margin: 0 }}>{error}</p>}
              <GothicButton variant="primary" onClick={handleRegister}>
                Register
              </GothicButton>
              <GothicButton variant="secondary" onClick={() => setMode("providers")}>
                Back to Login
              </GothicButton>
            </>
          )}

          <GothicButton variant="secondary" onClick={onBack}>
            Back
          </GothicButton>
        </div>
      </GothicFrame>
    </div>
  );
}
