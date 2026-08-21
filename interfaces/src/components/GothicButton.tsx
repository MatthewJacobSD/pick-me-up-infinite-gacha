import type { ReactNode } from "react";

interface GothicButtonProps {
  children: ReactNode;
  onClick?: () => void;
  disabled?: boolean;
  variant?: "primary" | "secondary";
  className?: string;
}

export default function GothicButton({
  children,
  onClick,
  disabled = false,
  variant = "primary",
  className = "",
}: GothicButtonProps) {
  const isPrimary = variant === "primary";

  return (
    <button
      className={`gothic-button ${className}`}
      onClick={onClick}
      disabled={disabled}
      style={{
        position: "relative",
        padding: "14px 42px",
        fontFamily: "var(--font-primary)",
        fontSize: "1rem",
        fontWeight: 700,
        letterSpacing: 3,
        textTransform: "uppercase",
        color: disabled
          ? "rgba(200, 190, 220, 0.35)"
          : isPrimary
            ? "var(--color-white)"
            : "var(--color-silver)",
        background: disabled
          ? "rgba(30, 25, 45, 0.4)"
          : isPrimary
            ? "linear-gradient(135deg, #2a1f4e 0%, #1e1638 100%)"
            : "linear-gradient(135deg, #1a1528 0%, #140f20 100%)",
        border: `1.5px solid ${
          disabled
            ? "rgba(160, 136, 224, 0.15)"
            : isPrimary
              ? "rgba(160, 136, 224, 0.5)"
              : "rgba(160, 136, 224, 0.3)"
        }`,
        borderRadius: 4,
        cursor: disabled ? "not-allowed" : "pointer",
        transition: "all 0.25s ease",
        boxShadow: disabled
          ? "none"
          : isPrimary
            ? "0 0 16px rgba(120, 90, 200, 0.3), inset 0 1px 0 rgba(255,255,255,0.05)"
            : "0 0 10px rgba(120, 90, 200, 0.15)",
        opacity: disabled ? 0.5 : 1,
        textShadow: disabled
          ? "none"
          : "0 0 8px rgba(200, 180, 255, 0.3)",
      }}
      onMouseEnter={(e) => {
        if (!disabled) {
          e.currentTarget.style.boxShadow = isPrimary
            ? "0 0 24px rgba(140, 110, 220, 0.5), inset 0 1px 0 rgba(255,255,255,0.08)"
            : "0 0 16px rgba(140, 110, 220, 0.3)";
          e.currentTarget.style.borderColor = "rgba(180, 150, 240, 0.7)";
          e.currentTarget.style.transform = "translateY(-1px)";
        }
      }}
      onMouseLeave={(e) => {
        if (!disabled) {
          e.currentTarget.style.boxShadow = isPrimary
            ? "0 0 16px rgba(120, 90, 200, 0.3), inset 0 1px 0 rgba(255,255,255,0.05)"
            : "0 0 10px rgba(120, 90, 200, 0.15)";
          e.currentTarget.style.borderColor = isPrimary
            ? "rgba(160, 136, 224, 0.5)"
            : "rgba(160, 136, 224, 0.3)";
          e.currentTarget.style.transform = "translateY(0)";
        }
      }}
    >
      {children}
    </button>
  );
}
