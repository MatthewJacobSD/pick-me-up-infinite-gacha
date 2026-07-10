# Frontend Tasks

## 1. Title Screen with Login Verification

**Department:** Frontend
**Status:** Pending

### Objective
Create the initial title screen with automatic authentication check logic.

### Minimum Scope
- Display the title screen.
- Check whether the user is already logged in when the screen loads.
- If a valid token exists, tap the screen and go to the main menu.
- If there is no valid login, show the login buttons after interaction.
- Follow the navigation flow defined for the game.

### Minimum Unit Tests
- Test screen rendering.
- Test token detection.
- Test redirection to the main menu when authenticated.
- Test login button visibility when not authenticated.

### Done When
- The title screen works as the game entry point.

---

## 2. Main Game Menu

**Department:** Frontend
**Status:** Pending

### Objective
Create the main menu after login, with functional basic navigation.

### Minimum Scope
- Display the buttons:
  - Play
  - Options
  - Exit
- Allow navigation between screens.
- Prepare the base for future game options.
- Keep the structure clean for later expansion.

### Minimum Unit Tests
- Test button rendering.
- Test clicks on each button.
- Test navigation to the correct screens.
- Test exit behavior.

### Done When
- The user can enter the menu and navigate through the main buttons.

---

## 3. Login Screen

**Department:** Frontend
**Status:** Pending

### Objective
Create the login interface and make the front-end communicate correctly with the backend.

### Minimum Scope
- Login with email and password.
- Login with Google.
- Prepare the front-end for future Facebook login without making it mandatory now.
- Receive the token from the backend.
- Store the token safely in the app flow.
- Redirect to the main menu after login.

### Minimum Unit Tests
- Test rendering of fields and buttons.
- Test credential submission.
- Test token reception and storage.
- Test error handling when login fails.
- Test the Google login flow.

### Done When
- The front-end can authenticate and take the player to the main menu.

---

## 4. Front-End WebSocket Integration

**Department:** Frontend
**Status:** Pending

### Objective
Connect the front-end to the backend using WebSocket with token authentication, receiving and sending data correctly.

### Minimum Scope
- Connect to the WebSocket after authentication.
- Send the token during connection.
- Receive backend responses.
- Prepare the front-end for real-time game data exchange.
- Handle basic disconnect and reconnect behavior.

### Minimum Unit Tests
- Test connection with a token.
- Test failure without a token.
- Test message reception.
- Test disconnect handling.

### Done When
- The front-end can communicate with the backend in real time with authentication.
