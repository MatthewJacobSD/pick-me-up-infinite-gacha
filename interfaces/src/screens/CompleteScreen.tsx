import GothicFrame from "../components/GothicFrame";

export default function CompleteScreen() {
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
      <GothicFrame size="wide" ornamentIntensity="minimal" glowIntensity={0.2}>
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 12,
            padding: "24px 0",
          }}
        >
          <p
            style={{
              fontFamily: "var(--font-primary)",
              fontSize: "clamp(1rem, 2.5vw, 1.4rem)",
              fontWeight: 600,
              letterSpacing: 4,
              color: "var(--color-white-dim)",
              opacity: 0.65,
              textShadow: "0 0 8px rgba(160, 136, 224, 0.25)",
              animation: "subtlePulse 3s ease-in-out infinite",
            }}
          >
            To be continued…
          </p>
        </div>
      </GothicFrame>
    </div>
  );
}
