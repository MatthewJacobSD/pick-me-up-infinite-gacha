import type { CSSProperties } from "react";

export interface MissionBannerProps {
  floor?: number | string;
  category?: string;
  goal?: string;
  className?: string;
}

export default function MissionBanner({
  floor = 4,
  category = "SUBJUGATION",
  goal = "ANNIHILATE ALL THE ENEMIES!",
  className = "",
}: MissionBannerProps) {
  const wrapperStyle: CSSProperties = {
    position: "relative",
    width: "min(580px, 95vw)",
    filter: "drop-shadow(0 0 20px rgba(170, 140, 255, 0.4))",
    animation: "pulseGlow 4s ease-in-out infinite",
    perspective: "800px",
  };

  const bannerStyle: CSSProperties = {
    position: "relative",
    background:
      "linear-gradient(170deg, #121020 0%, #0a0814 40%, #06050c 100%)",
    borderRadius: 3,
    padding: "32px 48px 30px",
    textAlign: "center",
    color: "#f2f0f8",
    overflow: "hidden",
    fontFamily: "var(--font-primary)",
    boxShadow: [
      "0 20px 60px rgba(0, 0, 0, 0.8)",
      "0 8px 24px rgba(0, 0, 0, 0.6)",
      "0 0 24px rgba(120, 90, 200, 0.15)",
      "inset 0 1px 0 rgba(232, 228, 245, 0.08)",
      "inset 0 -1px 0 rgba(0, 0, 0, 0.6)",
      "inset 2px 0 0 rgba(232, 228, 245, 0.04)",
      "inset -2px 0 0 rgba(232, 228, 245, 0.04)",
    ].join(", "),
    transform: "rotateX(2deg)",
    transformStyle: "preserve-3d" as const,
  };

  const innerGlowStyle: CSSProperties = {
    position: "absolute",
    inset: 0,
    background:
      "radial-gradient(ellipse at center, rgba(140, 110, 220, 0.06) 0%, transparent 70%)",
    pointerEvents: "none",
  };

  const svgFrameStyle: CSSProperties = {
    position: "absolute",
    inset: -30,
    width: "calc(100% + 60px)",
    height: "calc(100% + 60px)",
    pointerEvents: "none",
    zIndex: 2,
  };

  const floorStyle: CSSProperties = {
    position: "relative",
    zIndex: 3,
    fontFamily: "var(--font-decorative)",
    fontSize: "clamp(1.9rem, 5vw, 2.55rem)",
    fontWeight: 700,
    letterSpacing: 7,
    marginBottom: 14,
    textShadow: [
      "0 0 14px rgba(200, 180, 255, 0.55)",
      "0 2px 4px rgba(0, 0, 0, 0.7)",
    ].join(", "),
    transform: "translateZ(8px)",
  };

  const separatorStyle: CSSProperties = {
    width: "85%",
    height: 1,
    background: "linear-gradient(90deg, transparent, rgba(200, 180, 255, 0.25), transparent)",
    margin: "0 auto 10px",
  };

  const missionStyle: CSSProperties = {
    position: "relative",
    zIndex: 3,
    fontFamily: "var(--font-primary)",
    fontSize: "clamp(0.85rem, 2vw, 1.05rem)",
    fontWeight: 600,
    letterSpacing: 1.6,
    lineHeight: 1.55,
    opacity: 0.9,
    marginBottom: 4,
    textShadow: "0 1px 3px rgba(0, 0, 0, 0.6)",
  };

  const goalStyle: CSSProperties = {
    position: "relative",
    zIndex: 3,
    fontFamily: "var(--font-primary)",
    fontSize: "clamp(0.85rem, 2vw, 1.05rem)",
    fontWeight: 600,
    letterSpacing: 1.6,
    lineHeight: 1.55,
    opacity: 0.9,
    textShadow: "0 1px 3px rgba(0, 0, 0, 0.6)",
  };

  const corners = [
    { d: "M 20 30 L 8 18 L 20 6 L 32 18 Z", cx: 20, cy: 18, dur: "1.8s", begin: "0s" },
    { d: "M 616 30 L 604 18 L 616 6 L 628 18 Z", cx: 616, cy: 18, dur: "2.2s", begin: "0.5s" },
    { d: "M 20 190 L 8 178 L 20 166 L 32 178 Z", cx: 20, cy: 178, dur: "1.6s", begin: "0.3s" },
    { d: "M 616 190 L 604 178 L 616 166 L 628 178 Z", cx: 616, cy: 178, dur: "2s", begin: "0.7s" },
  ];

  const spikes = [
    { d: "M 14 22 L 4 12 L 14 2", stroke: "#e8e4f5" },
    { d: "M 26 22 L 36 12 L 26 2", stroke: "#e8e4f5" },
    { d: "M 610 22 L 600 12 L 610 2", stroke: "#e8e4f5" },
    { d: "M 622 22 L 632 12 L 622 2", stroke: "#e8e4f5" },
    { d: "M 14 194 L 4 184 L 14 174", stroke: "#e8e4f5" },
    { d: "M 26 194 L 36 184 L 26 174", stroke: "#e8e4f5" },
    { d: "M 610 194 L 600 184 L 610 174", stroke: "#e8e4f5" },
    { d: "M 622 194 L 632 184 L 622 174", stroke: "#e8e4f5" },
  ];

  return (
    <div className={`banner-wrapper ${className}`} style={wrapperStyle}>
      <svg style={svgFrameStyle} viewBox="0 0 636 210" preserveAspectRatio="none">
        <defs>
          <linearGradient id="mb-frameGrad" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stopColor="#e8e4f5" />
            <stop offset="50%" stopColor="#c8c0e0" />
            <stop offset="100%" stopColor="#e8e4f5" />
          </linearGradient>
          <filter id="mb-softGlow" x="-20%" y="-20%" width="140%" height="140%">
            <feGaussianBlur stdDeviation="1.5" result="blur" />
            <feMerge>
              <feMergeNode in="blur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
          <filter id="mb-elecGlow" x="-50%" y="-50%" width="200%" height="200%">
            <feGaussianBlur stdDeviation="3" result="blur" />
            <feMerge>
              <feMergeNode in="blur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>

        <path
          d="M 38 18 C 50 8, 80 5, 110 8 L 526 8 C 556 5, 586 8, 598 18 C 610 30, 614 48, 610 65 L 610 145 C 614 162, 610 180, 598 192 C 586 202, 556 205, 526 202 L 110 202 C 80 205, 50 202, 38 192 C 26 180, 22 162, 26 145 L 26 65 C 22 48, 26 30, 38 18 Z"
          fill="none"
          stroke="url(#mb-frameGrad)"
          strokeWidth="3"
          filter="url(#mb-softGlow)"
        />

        <path
          d="M 50 28 C 60 20, 85 18, 115 20 L 521 20 C 551 18, 576 20, 586 28 C 596 38, 600 50, 596 66 L 596 144 C 600 160, 596 172, 586 182 C 576 190, 551 192, 521 190 L 115 190 C 85 192, 60 190, 50 182 C 40 172, 36 160, 40 144 L 40 66 C 36 50, 40 38, 50 28 Z"
          fill="none"
          stroke="#d0c8e8"
          strokeWidth="1.2"
          opacity="0.7"
        />

        {corners.map((c, i) => (
          <g key={i}>
            <path d={c.d} fill="#1c1a28" stroke="#e8e4f5" strokeWidth="1.8" strokeLinejoin="round" />
            <circle cx={c.cx} cy={c.cy} r="2" fill="#e8e4f5" />
            <circle cx={c.cx} cy={c.cy} r="4" fill="none" stroke="#ffffff" strokeWidth="0.8" opacity="0">
              <animate attributeName="opacity" values="0;0;0.6;0;0.4;0" dur={c.dur} repeatCount="indefinite" begin={c.begin} />
            </circle>
          </g>
        ))}

        {spikes.map((s, i) => (
          <path key={i} d={s.d} fill="none" stroke={s.stroke} strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
        ))}

        <path d="M 18 85 C 6 95, 6 115, 18 125" fill="none" stroke="#c8c0e0" strokeWidth="1.4" opacity="0.6" />
        <path d="M 618 85 C 630 95, 630 115, 618 125" fill="none" stroke="#c8c0e0" strokeWidth="1.4" opacity="0.6" />
      </svg>

      <div style={bannerStyle}>
        <div style={innerGlowStyle} />
        <div style={floorStyle}>FLOOR {floor}</div>
        <div style={separatorStyle} />
        <div style={missionStyle}>MISSION CATEGORY - {category}</div>
        <div style={goalStyle}>GOAL - {goal}</div>
      </div>
    </div>
  );
}
