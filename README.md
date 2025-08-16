# 📚 Bookshop — Full-stack Online Bookstore

I built a full-stack bookstore that lets customers **browse items, place orders, and leave reviews**, while giving employees the tools to **manage inventory and fulfill shipments**.

---

## 🚀 What I Built

### 🗂 Rich Catalog & Data Model
- Stores both **customers and employees**.  
- Supports **books, magazines, newspapers**, plus *typeless* items.  
- Each item has: `name`, `description`, `image URL`, `publishing date`, `language`, `price`, `stock`, `minimum age`, and a `publisher` (name, address, email, phone).  
- **Used items**: annotations + condition (`mint/good/fair/poor`).  
- **New items**: packaging (`sealed/none`).  
- **Books**: genre(s), ≥1 author, page count, cover type (`hard/soft/spiral`).  
- **Magazines**: special vs. non-special editions.  
- **Newspapers**: headline + topics.  

---

### 🛒 Customer Experience
- Browse catalog & view item details.  
- Add quantities to cart → proceed to **checkout** → **payment**.  
- System saves `payment type`, `timestamp`, and `amount`.  
- Customers can:  
  - View order history.  
  - Cancel orders (only if paid and before preparation).  
  - Inspect details of past orders.  
- **Age gating** ensures customers can only access age-appropriate items.  

---

### 📦 Order Lifecycle
- Orders move through states:  
  **Cart → Pending → Confirmed → Preparation → Shipped → Delivered**.  
- Carts older than 1 month are auto-deleted.  
- Employees can:  
  - Advance orders from *Confirmed* to *Preparation*.  
  - Handle shipping and delivery updates.  
- Explicit methods include:  
  - `Checkout()`  
  - `ConfirmOrder()` (creates/verifies payment)  
  - `CancelOrder()` (allowed only before preparation)  

---

### ⭐ Reviews with Business Rules
- Customers can **leave, edit, and delete reviews** *only for delivered items*.  
- UI shows a *“Leave Review”* button when the rule is satisfied.  
- Invalid input is highlighted before submission.  

---

### 🧑‍💼 Employee Console
- Staff can:  
  - Add new items.  
  - Browse, edit, or delete existing items.  
  - See **confirmed orders** awaiting fulfillment.  

---

## 🔐 Authentication & Session Management
- **JWT-based auth** with short-lived *access tokens* + long-lived *refresh tokens*.  
- Clients (iOS/macOS):  
  - Store tokens securely in **Keychain**.  
  - Attach `Authorization: Bearer <token>` on each request.  
- Unified networking layer:  
  - Handles `401 Unauthorized` by **awaiting a refresh**.  
  - **Retries once** automatically.  
  - Prevents race conditions and infinite loops.  

---

## 🛠 Technical Highlights
- **EF Core, Code-First, SQLite**  
  - File-based DB for local hosting.  
  - LINQ for queries, async I/O.  

- **Validation Strategy**  
  - Database-level constraints & Fluent API.  
  - API-level DTO annotations.  
  - Uniqueness checks in server logic.  

- **Modeling & Associations**  
  - One-to-one, one-to-many, many-to-many via navigation properties.  
  - Association-with-attributes modeled as a separate entity.  
  - Cascade deletes where appropriate.  

- **Multi-Aspect Item Design**  
  - Items split into **New/Used** (*disjoint & complete*).  
  - Optionally into **Book/Magazine/Newspaper** (*disjoint & incomplete*).  
  - Composition + `{XOR, incomplete}` constraint.  
  - Discriminator column persists subtypes.  
  - Age gating evolved from raw `MinimumAge` into **AgeCategory** value object.  

---

✨ This project showcases **end-to-end ownership**: API design, secure session management, SwiftUI MVVM architecture, structured concurrency, and a scalable domain model for a real bookstore.
