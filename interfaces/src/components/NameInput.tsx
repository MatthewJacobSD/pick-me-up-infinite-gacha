import type { ChangeEvent } from "react";

interface NameInputProps {
  value: string;
  onChange: (value: string) => void;
  maxLength?: number;
  placeholder?: string;
}

export default function NameInput({
  value,
  onChange,
  maxLength = 6,
  placeholder = "",
}: NameInputProps) {
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const raw = e.target.value;
    const filtered = raw.replace(/[^A-Za-z]/g, "").slice(0, maxLength);
    onChange(filtered);
  };

  return (
    <div
      style={{
        position: "relative",
        width: "100%",
        maxWidth: 320,
        margin: "0 auto",
      }}
    >
      <input
        type="text"
        value={value}
        onChange={handleChange}
        maxLength={maxLength}
        placeholder={placeholder}
        autoComplete="off"
        spellCheck={false}
        style={{
          width: "100%",
          padding: "16px 20px",
          fontFamily: "var(--font-decorative)",
          fontSize: "1.6rem",
          fontWeight: 700,
          letterSpacing: 4,
          textAlign: "center",
          textTransform: "uppercase",
          color: "var(--color-white)",
          background: "rgba(15, 12, 24, 0.8)",
          border: "1.5px solid rgba(160, 136, 224, 0.35)",
          borderRadius: 4,
          caretColor: "var(--color-purple-light)",
          transition: "border-color 0.25s ease, box-shadow 0.25s ease",
        }}
        onFocus={(e) => {
          e.currentTarget.style.borderColor = "rgba(180, 150, 240, 0.7)";
          e.currentTarget.style.boxShadow =
            "0 0 16px rgba(120, 90, 200, 0.3), inset 0 0 20px rgba(120, 90, 200, 0.05)";
        }}
        onBlur={(e) => {
          e.currentTarget.style.borderColor = "rgba(160, 136, 224, 0.35)";
          e.currentTarget.style.boxShadow = "none";
        }}
      />
      <div
        style={{
          position: "absolute",
          bottom: -24,
          right: 0,
          fontSize: "0.72rem",
          color: "var(--color-white-dim)",
          letterSpacing: 1,
          opacity: 0.6,
        }}
      >
        {value.length}/{maxLength}
      </div>
    </div>
  );
}
