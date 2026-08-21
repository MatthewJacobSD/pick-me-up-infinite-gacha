import type { ReactNode, CSSProperties } from "react";

interface GothicFrameProps {
  children: ReactNode;
  className?: string;
  size?: "narrow" | "medium" | "wide" | "full";
  ornamentIntensity?: "minimal" | "standard" | "dramatic";
  glowIntensity?: number;
  innerGlow?: boolean;
  style?: CSSProperties;
}

const FRAME_WIDTHS: Record<string, string> = {
  narrow: "min(420px, 90vw)",
  medium: "min(560px, 92vw)",
  wide: "min(700px, 94vw)",
  full: "min(860px, 96vw)",
};

const CornerOrnaments = ({ intensity }: { intensity: string }) => {
  const baseStroke = intensity === "dramatic" ? "#e8e4f5" : "#d8d0f0";
  const durations = ["1.8s", "2.4s", "1.6s", "2.1s"];

  const corners = [
    { tx: 0, ty: 0, sx: 1, sy: 1, cx: 14, cy: 14 },
    { tx: 800, ty: 0, sx: -1, sy: 1, cx: 786, cy: 14 },
    { tx: 0, ty: 300, sx: 1, sy: -1, cx: 14, cy: 286 },
    { tx: 800, ty: 300, sx: -1, sy: -1, cx: 786, cy: 286 },
  ];

  return (
    <>
      {corners.map((c, ci) => (
        <g key={ci} transform={`translate(${c.tx},${c.ty}) scale(${c.sx},${c.sy})`}>
          <path d="M 8 4 L 2 2 L 8 10 L 14 2 L 20 10 L 26 2 L 20 4" fill="none" stroke={baseStroke} strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          <path d="M 4 18 L 2 10 L 6 4" fill="none" stroke="#c8c0e0" strokeWidth="1.2" strokeLinecap="round" opacity="0.7" />
          <path d="M 22 18 L 24 10 L 20 4" fill="none" stroke="#c8c0e0" strokeWidth="1.2" strokeLinecap="round" opacity="0.7" />
          <path d="M 10 22 L 6 16 L 10 10 L 14 16 Z" fill="#1c1a28" stroke={baseStroke} strokeWidth="1.2" />
          <circle cx={c.cx} cy={c.cy} r="1.8" fill="#e8e4f5" />
          <path d="M 8 4 L 2 2 L 8 10 L 14 2 L 20 10 L 26 2 L 20 4" fill="none" stroke="#ffffff" strokeWidth="1" strokeLinecap="round" strokeLinejoin="round" opacity="0">
            <animate attributeName="opacity" values="0;0;0.8;0.1;0.6;0;0;0.4;0;0" dur={durations[ci]} repeatCount="indefinite" begin={`${ci * 0.4}s`} />
          </path>
          <circle cx={c.cx} cy={c.cy} r="4" fill="none" stroke="#ffffff" strokeWidth="0.8" opacity="0">
            <animate attributeName="opacity" values="0;0;0.5;0;0.3;0" dur={durations[ci]} repeatCount="indefinite" begin={`${ci * 0.4}s`} />
          </circle>
        </g>
      ))}
    </>
  );
};

export default function GothicFrame({
  children,
  className = "",
  size = "medium",
  ornamentIntensity = "standard",
  glowIntensity = 0.4,
  innerGlow = false,
  style,
}: GothicFrameProps) {
  const borderWidth =
    ornamentIntensity === "dramatic"
      ? 3.5
      : ornamentIntensity === "standard"
        ? 2.8
        : 2;

  const frameWrapperStyle: CSSProperties = {
    position: "relative",
    width: FRAME_WIDTHS[size],
    maxWidth: "96vw",
    padding: "3px",
    animation: "pulseGlow 4s ease-in-out infinite",
    perspective: "1000px",
    ...style,
  };

  const svgStyle: CSSProperties = {
    position: "absolute",
    inset: -4,
    width: "calc(100% + 8px)",
    height: "calc(100% + 8px)",
    pointerEvents: "none",
    zIndex: 3,
    filter: "drop-shadow(0 2px 6px rgba(0, 0, 0, 0.4))",
  };

  const panelStyle: CSSProperties = {
    position: "relative",
    background:
      "linear-gradient(170deg, #121020 0%, #0a0814 40%, #06050c 100%)",
    borderRadius: 4,
    padding: "36px 48px",
    textAlign: "center",
    color: "var(--color-white)",
    overflow: "hidden",
    boxShadow: [
      `0 25px 80px rgba(0, 0, 0, 0.8)`,
      `0 10px 30px rgba(0, 0, 0, 0.6)`,
      `0 0 30px rgba(120, 90, 200, ${glowIntensity * 0.35})`,
      `inset 0 1px 0 rgba(232, 228, 245, 0.08)`,
      `inset 0 -1px 0 rgba(0, 0, 0, 0.6)`,
      `inset 2px 0 0 rgba(232, 228, 245, 0.04)`,
      `inset -2px 0 0 rgba(232, 228, 245, 0.04)`,
      `inset 0 0 40px rgba(120, 90, 200, 0.05)`,
    ].join(", "),
    transform: "rotateX(1.5deg) translateZ(0)",
    transformStyle: "preserve-3d" as const,
    animation: "panelBreathe 5s ease-in-out infinite",
  };

  const innerGlowStyle: CSSProperties = {
    position: "absolute",
    inset: 0,
    background:
      "radial-gradient(ellipse at center, rgba(140, 110, 220, 0.1) 0%, transparent 70%)",
    pointerEvents: "none",
  };

  const bevelHighlightStyle: CSSProperties = {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    height: 1,
    background:
      "linear-gradient(90deg, transparent 10%, rgba(232, 228, 245, 0.15) 50%, transparent 90%)",
    pointerEvents: "none",
    zIndex: 4,
  };

  return (
    <div className={`gothic-frame ${className}`} style={frameWrapperStyle}>
      <svg
        style={svgStyle}
        viewBox="0 0 800 300"
        preserveAspectRatio="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        <defs>
          <linearGradient
            id={`frameGrad-${size}`}
            x1="0%"
            y1="0%"
            x2="100%"
            y2="100%"
          >
            <stop offset="0%" stopColor="#e8e4f5" />
            <stop offset="50%" stopColor="#c8c0e0" />
            <stop offset="100%" stopColor="#e8e4f5" />
          </linearGradient>
          <filter
            id={`softGlow-${size}`}
            x="-20%"
            y="-20%"
            width="140%"
            height="140%"
          >
            <feGaussianBlur stdDeviation="1.2" result="blur" />
            <feMerge>
              <feMergeNode in="blur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
          <filter id="elecGlow" x="-50%" y="-50%" width="200%" height="200%">
            <feGaussianBlur stdDeviation="3" result="blur" />
            <feMerge>
              <feMergeNode in="blur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>

        <path
          d="M 40 15 C 55 5, 100 3, 160 8 L 640 8 C 700 3, 745 5, 760 15 C 775 30, 778 55, 775 80 L 775 220 C 778 245, 775 270, 760 285 C 745 295, 700 297, 640 292 L 160 292 C 100 297, 55 295, 40 285 C 25 270, 22 245, 25 220 L 25 80 C 22 55, 25 30, 40 15 Z"
          fill="none"
          stroke={`url(#frameGrad-${size})`}
          strokeWidth={borderWidth}
          filter={`url(#softGlow-${size})`}
        />

        <path
          d="M 55 30 C 68 22, 108 20, 165 24 L 635 24 C 692 20, 732 22, 745 30 C 758 42, 762 60, 758 82 L 758 218 C 762 240, 758 258, 745 270 C 732 278, 692 280, 635 276 L 165 276 C 108 280, 68 278, 55 270 C 42 258, 38 240, 42 218 L 42 82 C 38 60, 42 42, 55 30 Z"
          fill="none"
          stroke="#d0c8e8"
          strokeWidth={borderWidth * 0.5}
          opacity="0.7"
        />

        <CornerOrnaments intensity={ornamentIntensity} />

        <path
          d="M 12 100 C 0 112, 0 148, 12 160"
          fill="none"
          stroke="#c8c0e0"
          strokeWidth="1.4"
          opacity="0.6"
        />
        <path
          d="M 788 100 C 800 112, 800 148, 788 160"
          fill="none"
          stroke="#c8c0e0"
          strokeWidth="1.4"
          opacity="0.6"
        />

        {ornamentIntensity === "dramatic" && (
          <>
            <path
              d="M 400 0 C 395 4, 393 8, 396 12"
              fill="none"
              stroke="#d8d0f0"
              strokeWidth="1.8"
              strokeLinecap="round"
              opacity="0.6"
            />
            <path
              d="M 400 300 C 395 296, 393 292, 396 288"
              fill="none"
              stroke="#d8d0f0"
              strokeWidth="1.8"
              strokeLinecap="round"
              opacity="0.6"
            />
            <circle cx="400" cy="2" r="1.5" fill="#e0d8f8" opacity="0.5" />
            <circle cx="400" cy="298" r="1.5" fill="#e0d8f8" opacity="0.5" />
          </>
        )}
      </svg>

      <div className="gothic-panel" style={panelStyle}>
        {innerGlow && <div style={innerGlowStyle} />}
        <div style={bevelHighlightStyle} />
        <div style={{ position: "relative", zIndex: 2 }}>{children}</div>
      </div>
    </div>
  );
}
