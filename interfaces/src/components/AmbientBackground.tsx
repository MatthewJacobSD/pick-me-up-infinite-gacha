import { useMemo } from "react";

export default function AmbientBackground() {
  const particles = useMemo(() => {
    return Array.from({ length: 18 }).map((_, i) => ({
      id: i,
      left: `${5 + Math.random() * 90}%`,
      delay: Math.random() * 12,
      duration: 10 + Math.random() * 14,
      size: 1.5 + Math.random() * 2.5,
      opacity: 0.15 + Math.random() * 0.25,
    }));
  }, []);

  return (
    <div
      style={{
        position: "fixed",
        inset: 0,
        zIndex: 0,
        overflow: "hidden",
        pointerEvents: "none",
      }}
    >
      <div
        style={{
          position: "absolute",
          inset: 0,
          background:
            "radial-gradient(ellipse at 50% 40%, #12101f 0%, #0a0914 40%, #06050b 100%)",
        }}
      />

      <div
        style={{
          position: "absolute",
          inset: 0,
          background:
            "radial-gradient(ellipse at 50% 50%, transparent 30%, rgba(8, 6, 16, 0.7) 100%)",
        }}
      />

      <div
        style={{
          position: "absolute",
          top: "30%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          width: "60vw",
          height: "40vh",
          borderRadius: "50%",
          background:
            "radial-gradient(ellipse, rgba(80, 60, 150, 0.06) 0%, transparent 70%)",
          filter: "blur(40px)",
        }}
      />

      {particles.map((p) => (
        <div
          key={p.id}
          style={{
            position: "absolute",
            bottom: "-10px",
            left: p.left,
            width: p.size,
            height: p.size,
            borderRadius: "50%",
            background: "var(--color-purple-light)",
            opacity: 0,
            animation: `particleFloat ${p.duration}s ease-in-out ${p.delay}s infinite`,
          }}
        />
      ))}
    </div>
  );
}
