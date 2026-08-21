interface LoadingDotsProps {
  count?: number;
  color?: string;
  size?: number;
}

export default function LoadingDots({
  count = 3,
  color = "var(--color-purple-light)",
  size = 8,
}: LoadingDotsProps) {
  return (
    <div
      style={{
        display: "flex",
        gap: size * 0.8,
        justifyContent: "center",
        alignItems: "center",
        padding: `${size}px 0`,
      }}
    >
      {Array.from({ length: count }).map((_, i) => (
        <div
          key={i}
          style={{
            width: size,
            height: size,
            borderRadius: "50%",
            background: color,
            animation: `dotBounce 1.2s ease-in-out ${i * 0.15}s infinite`,
            boxShadow: `0 0 ${size}px ${color}`,
          }}
        />
      ))}
    </div>
  );
}
