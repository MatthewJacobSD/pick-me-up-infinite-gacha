# Backend Tasks

## 1. Database Modeling

**Department:** Backend
**Status:** Pending

### Objective
Define and implement the structure of both databases, correctly separating account data from gameplay data.

### Minimum Scope
- Create the MySQL structure for:
  - `id`
  - `username`
  - `email`
  - `phone number`
  - `island coordinates`
  - `creation date`
  - `basic settings`
- Create the MongoDB structure for:
  - `xp`
  - `gold`
  - `crystal`
  - `hp`
  - `mp`
  - `character level`
- Document the reason for separating the data between the two databases.
- Ensure the initial registration creates the base records in both databases.

### Minimum Unit Tests
- Validate that the schemas/models were created correctly.
- Validate that the required fields exist.
- Validate that the MySQL/MongoDB separation is respected.
- Validate the creation of the default data.

### Done When
- The structure of both databases is working and validated by tests.

---

## 2. Backend Connection to MySQL and MongoDB

**Department:** Backend
**Status:** Pending

### Objective
Create the backend connection layer for both databases and ensure it works safely and reliably.

### Minimum Scope
- Create the MySQL connection.
- Create the MongoDB connection.
- Use environment variables for credentials.
- Create a data access layer separated by responsibility.
- Create a connection check during server startup.

### Minimum Unit Tests
- Test connection with mocked databases.
- Test connection failure handling.
- Test read and write operations in the access layers.
- Test environment variable loading.

### Done When
- The backend can read and save data in both databases without hardcoded credentials.

---

## 3. Token-Based Authentication

**Department:** Backend
**Status:** Pending

### Objective
Implement the account authentication system with token generation and validation.

### Minimum Scope
- Login with email and password.
- Login with Google.
- Generate a token after successful login.
- Validate the token on protected routes.
- Keep the architecture ready to add Facebook later without major restructuring.

### Minimum Unit Tests
- Test login with valid credentials.
- Test login with invalid credentials.
- Test token generation.
- Test token validation and expiration.
- Test access denied without a token.

### Done When
- The backend authenticates the user and returns a secure token.

---

## 4. Authenticated WebSocket Integration

**Department:** Backend
**Status:** Pending

### Objective
Create the backend real-time entry point through WebSocket, using token authentication.

### Minimum Scope
- Open the WebSocket connection.
- Validate the token on connection.
- Block access for unauthenticated users.
- Prepare the channel to receive and send game data.
- Create the base structure for future game messages.

### Minimum Unit Tests
- Test valid connection with a token.
- Test rejection without a token.
- Test rejection with an invalid token.
- Test mocked message sending and receiving.

### Done When
- The backend accepts only authenticated clients and responds correctly through WebSocket.
