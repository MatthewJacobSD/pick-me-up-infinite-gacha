import type { CSSProperties } from "react";

interface GothicCornerProps {
  position: "top-left" | "top-right" | "bottom-left" | "bottom-right";
  size?: number;
  color?: string;
  glowColor?: string;
}

function getCornerPosition(position: string): CSSProperties {
  switch (position) {
    case "top-left":
      return { top: -8, left: -8 };
    case "top-right":
      return { top: -8, right: -8 };
    case "bottom-left":
      return { bottom: -8, left: -8 };
    case "bottom-right":
      return { bottom: -8, right: -8 };
    default:
      return {};
  }
}

export default function GothicCorner({
  position,
  size = 40,
  color = "#d8d0f0",
}: GothicCornerProps) {
  const transforms: Record<string, string> = {
    "top-left": "",
    "top-right": "scale(-1, 1)",
    "bottom-left": "scale(1, -1)",
    "bottom-right": "scale(-1, -1)",
  };

  const filterId = `cornerGlow-${position}`;

  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 60 60"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      style={{ position: "absolute", ...getCornerPosition(position) }}
    >
      <defs>
        <filter id={filterId} x="-30%" y="-30%" width="160%" height="160%">
          <feGaussianBlur stdDeviation="1.5" result="blur" />
          <feMerge>
            <feMergeNode in="blur" />
            <feMergeNode in="SourceGraphic" />
          </feMerge>
        </filter>
      </defs>
      <g
        transform={`translate(60, 60) ${transforms[position]}`}
        filter={`url(#${filterId})`}
      >
        <path
          d="M 8 4 C 4 4, 2 6, 2 10 L 2 28 C 2 30, 3 32, 5 33 C 7 34, 9 33, 10 31 L 12 27"
          stroke={color}
          strokeWidth="2"
          strokeLinecap="round"
          fill="none"
        />
        <path
          d="M 8 4 C 12 2, 18 2, 24 4 C 28 6, 28 8, 26 10 L 22 14"
          stroke={color}
          strokeWidth="2"
          strokeLinecap="round"
          fill="none"
        />
        <path
          d="M 14 2 C 10 0, 6 2, 5 6"
          stroke={color}
          strokeWidth="1.5"
          strokeLinecap="round"
          fill="none"
          opacity="0.7"
        />
        <circle cx="5" cy="5" r="2" fill={color} opacity="0.8" />
        <circle cx="3" cy="18" r="1.2" fill={color} opacity="0.5" />
        <path
          d="M 2 36 C 0 40, 2 46, 6 48"
          stroke={color}
          strokeWidth="1.2"
          strokeLinecap="round"
          fill="none"
          opacity="0.5"
        />
        <path
          d="M 36 2 C 40 0, 46 2, 48 6"
          stroke={color}
          strokeWidth="1.2"
          strokeLinecap="round"
          fill="none"
          opacity="0.5"
        />
      </g>
    </svg>
  );
}
