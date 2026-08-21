import type { ReactNode } from "react";

interface ScreenTransitionProps {
  children: ReactNode;
  transitioning: boolean;
  screenKey: string;
}

export default function ScreenTransition({
  children,
  transitioning,
  screenKey,
}: ScreenTransitionProps) {
  return (
    <div
      key={screenKey}
      style={{
        position: "absolute",
        inset: 0,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        opacity: transitioning ? 0 : 1,
        transform: transitioning ? "scale(0.97)" : "scale(1)",
        transition: "opacity 0.35s ease, transform 0.35s ease",
      }}
    >
      {children}
    </div>
  );
}
