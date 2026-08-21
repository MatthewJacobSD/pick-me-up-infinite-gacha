export default function GameExitedScreen() {
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
      <div style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        gap: 16,
      }}>
        <p style={{
          fontFamily: "var(--font-decorative)",
          fontSize: "clamp(1rem, 3vw, 1.6rem)",
          fontWeight: 700,
          letterSpacing: 6,
          color: "var(--color-white)",
          textShadow: "0 0 16px var(--color-text-glow)",
          textAlign: "center",
        }}>
          GAME EXITED
        </p>
        <p style={{
          fontFamily: "var(--font-primary)",
          fontSize: "clamp(0.65rem, 1.5vw, 0.78rem)",
          fontWeight: 500,
          letterSpacing: 2,
          color: "var(--color-white-dim)",
          opacity: 0.6,
        }}>
          PROTOTYPE
        </p>
      </div>
    </div>
  );
}
