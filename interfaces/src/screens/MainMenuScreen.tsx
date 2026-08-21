import { useCallback, useEffect, useState } from "react";
import GothicFrame from "../components/GothicFrame";
import GothicButton from "../components/GothicButton";
import type { AuthState, DeviceType, MainMenuPanel } from "../types";

interface MainMenuScreenProps {
  auth: AuthState;
  device: DeviceType;
  panel: MainMenuPanel;
  onPanelChange: (panel: MainMenuPanel) => void;
  onStartGame: () => void;
  onQuit: () => void;
}

const NEWS_ITEMS = [
  { title: "NEW ADVENTURERS HAVE ARRIVED", desc: "A fresh batch of heroes has been summoned to the tower.", date: "AUG 20" },
  { title: "TOWER EVENT SOON", desc: "A new tower event will begin next week. Prepare your party!", date: "AUG 22" },
  { title: "BALANCE UPDATE", desc: "Hero balance adjustments have been applied. Check the patch notes.", date: "AUG 18" },
];

const FAQ_ITEMS = [
  { q: "WHAT IS PICK ME UP?", a: "A dark fantasy gacha RPG where you summon heroes and climb an endless tower." },
  { q: "HOW DO I START?", a: "Select your device, log in, choose a name, and begin the tutorial." },
  { q: "HOW DOES THE TUTORIAL WORK?", a: "You'll learn the basics of combat, summoning, and party formation." },
  { q: "WHAT HAPPENS WHEN A CHARACTER DIES?", a: "Fallen heroes can be revived or replaced. Manage your roster wisely." },
];

function PanelOverlay({ children, onClose }: { children: React.ReactNode; onClose: () => void }) {
  return (
    <div
      style={{
        position: "absolute",
        inset: 0,
        background: "rgba(6, 5, 11, 0.85)",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        zIndex: 10,
        backdropFilter: "blur(4px)",
      }}
      onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}
    >
      {children}
    </div>
  );
}

function SettingsPanel({ onClose }: { onClose: () => void }) {
  const [music, setMusic] = useState(70);
  const [sfx, setSfx] = useState(80);
  const [quality, setQuality] = useState("HIGH");

  return (
    <PanelOverlay onClose={onClose}>
      <GothicFrame size="medium" ornamentIntensity="standard">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 20, padding: "8px 0", minWidth: 300 }}>
          <p style={{ fontFamily: "var(--font-decorative)", fontSize: "clamp(0.9rem, 2.5vw, 1.2rem)", fontWeight: 700, letterSpacing: 5, color: "var(--color-white)", textShadow: "0 0 12px var(--color-text-glow)" }}>
            SETTINGS
          </p>
          {[
            { label: "MUSIC VOLUME", value: music, onChange: setMusic },
            { label: "SFX VOLUME", value: sfx, onChange: setSfx },
          ].map((s) => (
            <div key={s.label} style={{ width: "100%", display: "flex", flexDirection: "column", gap: 6 }}>
              <div style={{ display: "flex", justifyContent: "space-between" }}>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.75rem", letterSpacing: 2, color: "var(--color-white-dim)" }}>{s.label}</span>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.75rem", letterSpacing: 1, color: "var(--color-purple-light)" }}>{s.value}%</span>
              </div>
              <input type="range" min="0" max="100" value={s.value} onChange={(e) => s.onChange(Number(e.target.value))}
                style={{ width: "100%", accentColor: "var(--color-purple-mid)", height: 4 }} />
            </div>
          ))}
          <div style={{ width: "100%", display: "flex", flexDirection: "column", gap: 6 }}>
            <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.75rem", letterSpacing: 2, color: "var(--color-white-dim)" }}>GRAPHICS QUALITY</span>
            <div style={{ display: "flex", gap: 8 }}>
              {["LOW", "MEDIUM", "HIGH"].map((q) => (
                <button key={q} onClick={() => setQuality(q)} style={{
                  flex: 1, padding: "8px 0", fontFamily: "var(--font-primary)", fontSize: "0.7rem", letterSpacing: 1,
                  background: quality === q ? "var(--color-purple-dark)" : "transparent",
                  border: `1px solid ${quality === q ? "var(--color-purple-light)" : "rgba(200,180,255,0.2)"}`,
                  borderRadius: 3, color: "var(--color-white)", cursor: "pointer",
                }}>{q}</button>
              ))}
            </div>
          </div>
          <GothicButton variant="secondary" onClick={onClose}>Close</GothicButton>
        </div>
      </GothicFrame>
    </PanelOverlay>
  );
}

function NewsPanel({ onClose }: { onClose: () => void }) {
  return (
    <PanelOverlay onClose={onClose}>
      <GothicFrame size="medium" ornamentIntensity="standard">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 16, padding: "8px 0" }}>
          <p style={{ fontFamily: "var(--font-decorative)", fontSize: "clamp(0.9rem, 2.5vw, 1.2rem)", fontWeight: 700, letterSpacing: 5, color: "var(--color-white)", textShadow: "0 0 12px var(--color-text-glow)" }}>
            GAME NEWS
          </p>
          {NEWS_ITEMS.map((item, i) => (
            <div key={i} style={{ width: "100%", padding: "12px 16px", background: "rgba(26, 23, 40, 0.6)", borderRadius: 4, border: "1px solid rgba(200,180,255,0.1)" }}>
              <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 4 }}>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.8rem", fontWeight: 700, letterSpacing: 1, color: "var(--color-purple-light)" }}>{item.title}</span>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.65rem", color: "var(--color-white-dim)", opacity: 0.5 }}>{item.date}</span>
              </div>
              <p style={{ fontFamily: "var(--font-primary)", fontSize: "0.75rem", letterSpacing: 1, color: "var(--color-white-dim)", opacity: 0.8 }}>{item.desc}</p>
            </div>
          ))}
          <GothicButton variant="secondary" onClick={onClose}>Close</GothicButton>
        </div>
      </GothicFrame>
    </PanelOverlay>
  );
}

function FaqPanel({ onClose }: { onClose: () => void }) {
  const [openIdx, setOpenIdx] = useState<number | null>(null);
  return (
    <PanelOverlay onClose={onClose}>
      <GothicFrame size="medium" ornamentIntensity="standard">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 12, padding: "8px 0" }}>
          <p style={{ fontFamily: "var(--font-decorative)", fontSize: "clamp(0.9rem, 2.5vw, 1.2rem)", fontWeight: 700, letterSpacing: 5, color: "var(--color-white)", textShadow: "0 0 12px var(--color-text-glow)" }}>
            FAQ
          </p>
          {FAQ_ITEMS.map((item, i) => (
            <div key={i} style={{ width: "100%" }}>
              <button onClick={() => setOpenIdx(openIdx === i ? null : i)} style={{
                width: "100%", padding: "12px 16px", background: "rgba(26, 23, 40, 0.6)", border: "1px solid rgba(200,180,255,0.1)",
                borderRadius: 4, cursor: "pointer", display: "flex", justifyContent: "space-between", alignItems: "center",
              }}>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.8rem", fontWeight: 600, letterSpacing: 1, color: "var(--color-white)", textAlign: "left" }}>{item.q}</span>
                <span style={{ fontFamily: "var(--font-primary)", fontSize: "1rem", color: "var(--color-purple-light)", transform: openIdx === i ? "rotate(45deg)" : "none", transition: "transform 0.2s" }}>+</span>
              </button>
              {openIdx === i && (
                <p style={{ fontFamily: "var(--font-primary)", fontSize: "0.75rem", letterSpacing: 1, color: "var(--color-white-dim)", padding: "10px 16px", opacity: 0.8, lineHeight: 1.6 }}>
                  {item.a}
                </p>
              )}
            </div>
          ))}
          <GothicButton variant="secondary" onClick={onClose}>Close</GothicButton>
        </div>
      </GothicFrame>
    </PanelOverlay>
  );
}

function SupportPanel({ onClose }: { onClose: () => void }) {
  return (
    <PanelOverlay onClose={onClose}>
      <GothicFrame size="narrow" ornamentIntensity="standard">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 16, padding: "12px 0" }}>
          <p style={{ fontFamily: "var(--font-decorative)", fontSize: "clamp(0.9rem, 2.5vw, 1.2rem)", fontWeight: 700, letterSpacing: 5, color: "var(--color-white)", textShadow: "0 0 12px var(--color-text-glow)" }}>
            SUPPORT THE DEVELOPERS
          </p>
          <p style={{ fontFamily: "var(--font-primary)", fontSize: "0.8rem", letterSpacing: 1, color: "var(--color-white-dim)", opacity: 0.8, lineHeight: 1.7, textAlign: "center", maxWidth: 320 }}>
            This is a prototype testing interface. Your support helps us build the full game experience.
          </p>
          <p style={{ fontFamily: "var(--font-primary)", fontSize: "0.7rem", letterSpacing: 1, color: "var(--color-purple-light)", opacity: 0.6 }}>
            MOCK DONATION - NO REAL PAYMENT
          </p>
          <GothicButton variant="secondary" onClick={onClose}>Close</GothicButton>
        </div>
      </GothicFrame>
    </PanelOverlay>
  );
}

function QuitPanel({ onClose, onQuit }: { onClose: () => void; onQuit: () => void }) {
  return (
    <PanelOverlay onClose={onClose}>
      <GothicFrame size="narrow" ornamentIntensity="standard">
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 20, padding: "12px 0" }}>
          <p style={{ fontFamily: "var(--font-decorative)", fontSize: "clamp(0.9rem, 2.5vw, 1.2rem)", fontWeight: 700, letterSpacing: 4, color: "var(--color-white)", textShadow: "0 0 12px var(--color-text-glow)" }}>
            QUIT GAME?
          </p>
          <p style={{ fontFamily: "var(--font-primary)", fontSize: "0.8rem", letterSpacing: 1, color: "var(--color-white-dim)", opacity: 0.7, textAlign: "center" }}>
            ARE YOU SURE YOU WANT TO EXIT?
          </p>
          <div style={{ display: "flex", gap: 16 }}>
            <GothicButton variant="primary" onClick={onQuit}>Yes</GothicButton>
            <GothicButton variant="secondary" onClick={onClose}>No</GothicButton>
          </div>
        </div>
      </GothicFrame>
    </PanelOverlay>
  );
}

export default function MainMenuScreen({ auth, device, panel, onPanelChange, onStartGame, onQuit }: MainMenuScreenProps) {
  const handleKeyDown = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Escape" && panel) onPanelChange(null);
    },
    [panel, onPanelChange]
  );

  useEffect(() => {
    window.addEventListener("keydown", handleKeyDown);
    return () => window.removeEventListener("keydown", handleKeyDown);
  }, [handleKeyDown]);

  const showQuit = device === "pc" || device === "console";

  return (
    <div style={{ position: "relative", width: "100%", height: "100%", display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "center" }}>
      <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 28 }}>
        <p style={{
          fontFamily: "var(--font-decorative)",
          fontSize: "clamp(0.65rem, 1.5vw, 0.78rem)",
          fontWeight: 500,
          letterSpacing: 3,
          color: "var(--color-purple-light)",
          opacity: 0.6,
        }}>
          {auth.username.toUpperCase()} \u2022 {auth.characterId}
        </p>

        <p style={{
          fontFamily: "var(--font-decorative)",
          fontSize: "clamp(1.8rem, 5vw, 3rem)",
          fontWeight: 900,
          letterSpacing: 8,
          color: "var(--color-white)",
          textShadow: "0 0 24px rgba(200, 180, 255, 0.5), 0 0 48px rgba(120, 90, 200, 0.3)",
          textAlign: "center",
          lineHeight: 1.3,
        }}>
          PICK ME UP
        </p>

        <GothicButton variant="primary" onClick={onStartGame}>
          START GAME
        </GothicButton>

        <div style={{ display: "flex", gap: 16, flexWrap: "wrap", justifyContent: "center" }}>
          <MenuIconBtn label="SUPPORT" icon="\u2605" onClick={() => onPanelChange("support")} />
          <MenuIconBtn label="SETTINGS" icon="\u2699" onClick={() => onPanelChange("settings")} />
          <MenuIconBtn label="NEWS" icon="\u2139" onClick={() => onPanelChange("news")} />
          <MenuIconBtn label="FAQ" icon="?" onClick={() => onPanelChange("faq")} />
          {showQuit && (
            <MenuIconBtn label="QUIT" icon="\u2716" onClick={() => onPanelChange("quit")} />
          )}
        </div>
      </div>

      {panel === "settings" && <SettingsPanel onClose={() => onPanelChange(null)} />}
      {panel === "news" && <NewsPanel onClose={() => onPanelChange(null)} />}
      {panel === "faq" && <FaqPanel onClose={() => onPanelChange(null)} />}
      {panel === "support" && <SupportPanel onClose={() => onPanelChange(null)} />}
      {panel === "quit" && <QuitPanel onClose={() => onPanelChange(null)} onQuit={onQuit} />}
    </div>
  );
}

function MenuIconBtn({ label, icon, onClick }: { label: string; icon: string; onClick: () => void }) {
  return (
    <button
      onClick={onClick}
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        gap: 6,
        padding: "12px 18px",
        background: "rgba(20, 18, 32, 0.6)",
        border: "1px solid rgba(200, 180, 255, 0.15)",
        borderRadius: 4,
        cursor: "pointer",
        transition: "all 0.25s ease",
        minWidth: 72,
      }}
      onMouseEnter={(e) => {
        e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.4)";
        e.currentTarget.style.boxShadow = "0 2px 12px rgba(120, 90, 200, 0.2)";
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.borderColor = "rgba(200, 180, 255, 0.15)";
        e.currentTarget.style.boxShadow = "none";
      }}
    >
      <span style={{ fontSize: "1.2rem", color: "var(--color-purple-light)" }}>{icon}</span>
      <span style={{ fontFamily: "var(--font-primary)", fontSize: "0.55rem", letterSpacing: 1, color: "var(--color-white-dim)" }}>{label}</span>
    </button>
  );
}
