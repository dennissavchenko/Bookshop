# 📚 Bookshop — Full-stack Online Bookstore
Bookshop is a full-stack system demonstrating **complex software design** across three components:
- **Server (ASP.NET Core)** — handles database communication, validation, and business logic.
- **iOS App (SwiftUI)** — customer-facing app implementing use cases such as browsing and leaving reviews.
- **macOS App (SwiftUI)** — employee console for inventory and order management.

---

## 🏗 System Design

The main focus of the project is **software architecture and modeling**.  
Detailed design diagrams, state diagrams, use cases, and scenarios are available in the attached project documentation.

### Key Design Features
- **Multi-aspect modeling** for catalog items (condition, type).  
- **Associations with attributes** (e.g., order items with quantity/price).  
- **Custom constraints** to enforce business rules (e.g., review only on delivered orders).  
- **Inheritance strategies** (new/used items, books/magazines/newspapers).  
- Server follows **SOLID principles** and exposes clean **RESTful APIs**.  

---

## 🔐 Authentication

- **JWT-based authentication** with access tokens + refresh tokens.  
- Several endpoints secured with role-based access.  
- Clients (iOS/macOS) securely store tokens in **Keychain** and automatically refresh them when expired.  

---

## 📱 Client Applications

### iOS App (Customer Role)
- Communicates with the server via API.  
- Implements use cases from documentation, including **leaving a comment on received items**.  
- Built with **SwiftUI + MVVM** for clean separation of UI and logic.  

### macOS App (Employee Role)
- Separate app dedicated to employee workflows.  
- **Login view** for employee authentication.  
- Workspace includes:  
  - Add / delete / update bookshop items.  
  - Access and modify order information.  
- Also built with **SwiftUI + MVVM**.  

---

## ⚙️ Tech Stack

- **Backend**: ASP.NET Core, Entity Framework Core, SQLite, JWT  
- **Clients**: Swift, SwiftUI, MVVM, URLSession (async/await), Keychain  

---

## 📄 Documentation

The repository includes detailed **Project Documentation** with:
- System design diagram  
- State diagram and descriptions  
- Use cases and use case scenarios  

---

✨ This project demonstrates how to design and implement a **full software system** with a strong focus on **architecture, domain modeling, and clean client–server communication**.


