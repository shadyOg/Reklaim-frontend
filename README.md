# Reklaim Frontend Plan

This document is the frontend guide for the Reklaim campus lost-and-found app. It is meant to help the frontend team understand the product, the chosen architecture, the page structure, and the implementation order.

## 1. Project Goal

Reklaim is a campus-focused lost-and-found platform where students can:

- post lost or found items
- search and browse listings
- upload item photos
- submit claims with proof details
- review incoming claims and approve or deny them
- coordinate contact only after verification

The frontend should make the experience fast, simple, and trustworthy. Students should be able to use it in a few taps without confusion.

---

## 2. Recommended Stack Decision

### Chosen approach: Blazor Web App

We recommend using Blazor Web App (interactive server mode for the MVP) instead of classic ASP.NET MVC.

### Why Blazor is the better fit

- Same .NET ecosystem as the backend, so the team can stay in one language stack: C#
- Easier to build interactive forms, claim flows, and feeds than MVC views with a lot of custom JavaScript
- Component-based structure makes it easier to reuse cards, tables, filters, and modals
- Better fit for a campus app that has a lot of form-heavy pages and state management
- Less friction for a team that is already using ASP.NET Core on the backend

### Why not MVC for this project?

MVC is still valid, but for this app it would likely add more plumbing:

- more view logic mixed into Razor pages
- more server-side HTML and form handling
- more custom effort for interactive UI states
- a less clean component model for reusability

For an MVP, Blazor is the cleaner and more maintainable choice, especially if the team is not already experienced with JavaScript-heavy frontend frameworks.

> Recommendation: use Blazor Server or Blazor Web App with interactive server mode for the MVP. This keeps setup simple and reduces frontend complexity without sacrificing functionality.

---

## 3. Frontend Architecture

The frontend should be treated as a UI layer that talks to the ASP.NET Core Web API backend.

### Proposed structure

```text
Reklaim-frontend/
├── Components/
│   ├── Layout/
│   │   ├── NavMenu.razor
│   │   ├── TopBar.razor
│   │   └── Footer.razor
│   ├── Shared/
│   │   ├── SearchBar.razor
│   │   ├── ItemCard.razor
│   │   ├── FilterPanel.razor
│   │   ├── EmptyState.razor
│   │   └── StatusBadge.razor
│   ├── Auth/
│   │   ├── LoginForm.razor
│   │   └── RegisterForm.razor
│   ├── Posts/
│   │   ├── PostForm.razor
│   │   ├── PostDetails.razor
│   │   └── PostList.razor
│   └── Claims/
│       ├── ClaimButton.razor
│       ├── ClaimRequestForm.razor
│       └── ClaimReviewPanel.razor
├── Pages/
│   ├── Index.razor
│   ├── Login.razor
│   ├── Register.razor
│   ├── PostCreate.razor
│   ├── PostDetails.razor
│   ├── MyClaims.razor
│   ├── ClaimReview.razor
│   └── Profile.razor
├── Models/
│   ├── UserDto.cs
│   ├── ItemPostDto.cs
│   ├── ClaimRequestDto.cs
│   └── ApiResult.cs
├── Services/
│   ├── AuthService.cs
│   ├── PostService.cs
│   ├── ClaimService.cs
│   ├── FileUploadService.cs
│   └── ApiClient.cs
├── Helpers/
│   ├── DateFormatter.cs
│   ├── ValidationHelper.cs
│   └── EnumMapper.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── App.razor
├── Program.cs
├── appsettings.json
├── README.md
└── LICENSE
```

### Main responsibilities

- Pages handle routing and screen-level composition
- Components handle reusable UI sections
- Services handle API communication
- Models represent data from the backend
- Helpers keep formatting and validation logic in one place

---

## 4. Proposed User Flow

### A. Authentication

1. User enters university email
2. App validates domain
3. User signs up or logs in
4. If valid, user is redirected to the dashboard

### B. Feed / Hub

1. User lands on Lost & Found dashboard
2. User can switch between Lost Items and Found Items
3. User can search by keyword
4. User can filter by category, date, location, and status
5. User opens an item to view details

### C. Creating a listing

1. User clicks “Report a Lost Item” or “Report a Found Item”
2. User fills out title, description, category, location, and image
3. Item is submitted to backend
4. Item appears in the relevant feed

### D. Claiming an item

1. User clicks “Claim” on a post
2. User submits proof details explaining why the item belongs to them
3. System creates a claim request
4. Finder receives the request and reviews it
5. Finder approves or denies the request
6. If approved, contact details are revealed to both parties

### E. Finder review

1. Finder opens their claim review inbox
2. They see pending claims for their posts
3. They review proof description
4. They approve or reject request

---

## 5. Proposed Pages

### 5.1 Landing / Hub Page

Purpose: central dashboard for browsing listings.

Features:

- toggle between Lost and Found
- search bar
- filter sidebar or top filters
- item cards with title, image, category, date, and status
- CTA buttons like “View Details” and “Report Item”

### 5.2 Login and Register Pages

Purpose: user access and domain validation.

Features:

- email field
- password field
- university email validation
- form-level error messages
- redirect after successful login

### 5.3 Create Post Page

Purpose: add a lost or found item.

Features:

- title
- description
- location
- category dropdown
- post type selector
- image upload
- submit button
- draft/validation feedback

### 5.4 Post Details Page

Purpose: view details of an item and take action.

Features:

- image gallery or single image
- title and description
- metadata like date and category
- status badge
- claim button
- owner/finder poster info only when appropriate

### 5.5 Claim Form

Purpose: allow a claimant to submit proof.

Features:

- text area for proof description
- validation for length and required content
- submit action
- success message and pending state

### 5.6 Claim Review Dashboard

Purpose: allow finder to act on requests.

Features:

- list of pending claims
- claim details and proof description
- approve / deny buttons
- confirmation modal after action

### 5.7 Profile / User Dashboard

Purpose: show the user’s activity.

Features:

- posts they created
- claims they submitted
- claim history
- account info

---

## 6. Important UI/UX Principles

### Keep it simple and trustworthy

The platform is for students and should feel safe, clean, and clear.

Important principles:

- no public personal data by default
- clear status badges for item state
- strong validation on auth and claim submission
- visible success/error feedback for forms
- minimal clutter on key actions

### Mobile-first design

Because this is a campus app, students are likely to use it from phones. The design should work well on small screens first.

### Fast and clear actions

Users should be able to:

- search instantly
- open an item quickly
- create a post in under a minute
- submit a claim without confusion

---

## 7. Proposed Component Design

### Reusable UI pieces

These components should be built early because they will be used all over the app:

- ItemCard
- SearchBar
- FilterPanel
- StatusBadge
- EmptyState
- LoadingSpinner
- FormField
- ModalDialog
- ConfirmationDialog

### Reusable page patterns

- List pages with filters and cards
- Detail pages with image + metadata + CTA area
- Form pages with validation and form actions
- Review panels for claims and moderation

---

## 8. Data Contracts to Expect from the API

The frontend should assume the following backend models are available:

### User

- Id
- Name
- StudentEmail
- PhoneNumber
- DateJoined

### ItemPost

- Id
- Title
- Description
- LocationFound
- Category
- PostType
- ImageUrl
- DatePosted
- Status
- UserId

### ClaimRequest

- Id
- PostId
- ClaimerUserId
- ProofDescription
- Status

The frontend should treat these as DTOs and avoid building business logic assumptions that conflict with the API contract.

---

## 9. MVP Implementation Order

### Phase 1: Project setup

- create Blazor frontend project
- configure app structure
- set up base layout and navigation
- add styling system and theme

### Phase 2: Authentication

- login page
- register page
- university email validation
- redirect logic after sign in

### Phase 3: Feed

- dashboard page
- lost/found tabs
- search and filters
- item card list

### Phase 4: Item creation

- create post form
- image upload UI
- post submission flow
- item detail view

### Phase 5: Claim flow

- claim request form
- pending state screen
- review dashboard
- approval / denial actions

### Phase 6: Polish and QA

- loading states
- empty states
- form validation
- mobile responsiveness
- accessibility checks
- final testing pass

---

## 10. Frontend State and Service Pattern

Because this is an ASP.NET full-stack app, we will keep the front-end simple and predictable.

### Service pattern

Use a service layer for all backend calls:

- AuthService
- PostService
- ClaimService
- FileUploadService
- ApiClient

This keeps components focused on UI instead of HTTP logic.

### Recommended patterns

- inject services into components
- use async event handlers for API actions
- keep page state local when possible
- centralize validation and API error handling
- avoid duplicating logic across pages

---

## 11. Styling and Layout Approach

### Recommended UI style

- clean and minimal
- campus-friendly and trustworthy
- strong contrast for CTAs
- good spacing for forms and cards
- mobile-first layout

### Suggested layout structure

- top navigation bar
- left or top filter panel
- main content area
- action buttons clearly visible
- cards with images and status tags

---

## 12. Good Practices for the Team

- Keep components small and reusable
- Use consistent naming conventions
- Keep one responsibility per component
- Validate forms before sending requests
- Show loading states for network actions
- Handle empty results gracefully
- Never expose personal contact info unless the claim has been approved
- Ensure the UI matches the API contract exactly

---

## 13. Risks to Watch

- Too much complexity in the first version
- mixing API logic directly into components
- exposing private contact information too early
- building the UI before clarifying backend response shapes
- ignoring mobile usability

The team should keep the MVP focused on the core lost-and-found flow and not add advanced features too early.

---

## 14. Final Recommendation

For this project, Blazor is the best frontend choice because it matches the .NET backend, keeps the stack simple, and reduces the need for JavaScript-heavy architecture. The team can build a clean, maintainable MVP without overengineering the frontend.

The main goal for the first sprint is to build a working flow:

- sign in
- create post
- browse feed
- submit claim
- review claim
- reveal contact on approval

Once that works, the product can be improved with extra polish and features.

---

## 15. Suggested Next Step

The frontend team should begin by creating the base Blazor project structure and implementing these screens in order:

1. Login/Register
2. Hub page
3. Post details
4. Create item page
5. Claim form
6. Review dashboard
7. Polish / responsive design

This gives the team a clear execution order and keeps the build focused on the minimum viable product.
