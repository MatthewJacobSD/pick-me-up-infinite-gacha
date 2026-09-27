const API_BASE = "http://localhost:5137";

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options?.headers,
    },
  });
  if (!res.ok) {
    const body = await res.json().catch(() => ({}));
    throw new Error(body.detail || `HTTP ${res.status}`);
  }
  return res.json();
}

function authHeaders(token: string): Record<string, string> {
  return { Authorization: `Bearer ${token}` };
}

// ── Auth ────────────────────────────────────────────────────

export async function getLoginUrl(provider: "google" | "facebook") {
  return request<{ redirectUrl: string }>(`/api/auth/login-url/${provider}`);
}

export async function consumeLoginCode(loginCode: string) {
  return request<{
    sessionId: string;
    accessToken: string;
    refreshToken: string;
    publicCode: string;
    gameEmail: string;
    identity: { provider: number; email: string; name: string; externalId: string };
  }>("/api/auth/consume-login-code", {
    method: "POST",
    body: JSON.stringify({ loginCode }),
  });
}

export async function register(email: string, password: string) {
  return request<{
    loginCode: string;
    publicCode: string;
    gameEmail: string;
    username: string;
  }>("/api/auth/register", {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
}

export async function refresh(refreshToken: string) {
  return request<{ accessToken: string; refreshToken: string }>("/api/auth/refresh", {
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });
}

export async function logout(sessionId: string, token: string) {
  return request<void>("/api/auth/logout", {
    method: "POST",
    headers: authHeaders(token),
    body: JSON.stringify({ sessionId }),
  });
}

// ── Preferences ────────────────────────────────────────────

export type PreferenceDomain =
  | "gameplay" | "accessibility" | "language"
  | "notifications" | "social" | "audio" | "ui";

export async function getAllPreferences(token: string) {
  return request<Record<string, unknown> & { version: number }>(
    "/account/preferences",
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export async function getPreferenceSlice(domain: PreferenceDomain, token: string) {
  return request<{ settings: unknown; version: number }>(
    `/account/preferences/${domain}`,
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export async function putPreferenceSlice(domain: PreferenceDomain, body: unknown, token: string) {
  return request<{ settings: unknown; version: number }>(
    `/account/preferences/${domain}`,
    {
      method: "PUT",
      headers: { Authorization: `Bearer ${token}` },
      body: JSON.stringify(body),
    }
  );
}

// ── Social ──────────────────────────────────────────────────

export async function getFriends(token: string) {
  return request<string[]>(`/account/social/friends`, {
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function sendFriendRequest(targetUserId: string, token: string) {
  return request<void>(`/account/social/friends/requests`, {
    method: "POST",
    headers: { Authorization: `Bearer ${token}` },
    body: JSON.stringify({ targetUserId }),
  });
}

export async function getBlocks(token: string) {
  return request<string[]>(`/account/social/blocks`, {
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function blockUser(targetUserId: string, token: string) {
  return request<void>(`/account/social/blocks`, {
    method: "POST",
    headers: { Authorization: `Bearer ${token}` },
    body: JSON.stringify({ targetUserId }),
  });
}

// ── Profile ─────────────────────────────────────────────────

export async function getProfile(token: string) {
  return request<{ username: string; avatar: { avatarUrl: string; avatarType: string }; version: number }>(
    `/account/profile`,
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

// ── Health ──────────────────────────────────────────────────

export async function healthCheck() {
  return request<string>("/health");
}
