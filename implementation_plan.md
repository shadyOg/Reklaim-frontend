## 5-Day Frontend Sprint Plan

This is a practical 3-phase breakdown so the frontend team can move fast without waiting on each other. The work is split so everyone has clear ownership, but there is still overlap where needed for handoff and testing.

---

## Phase 1 — Foundation & App Shell
Duration: Days 1–2

Goal:
Set up the base Blazor app, core layout, and the shared UI pieces needed for everything else.

### Tasks by person

#### Shadrack Dorkenoo — Frontend Lead
- Set up the base Blazor project structure
- Create the main app layout and navigation
- Create the home shell and route structure
- Define the shared app styling/theme
- Review all UI components before they are used across pages
- Own final structure and consistency across the app

#### Joel Adom Yaw Opoku
- Build the login and register pages
- Create the form template for auth pages
- Add validation for university email input
- Set up redirect flow after successful login
- Build reusable form field components

#### Samuel Kofi Ntem Amankwah
- Build shared components:
  - SearchBar
  - StatusBadge
  - EmptyState
  - LoadingSpinner
  - ItemCard shell
- Create the filter panel structure
- Prepare the card layout for listing pages
- Start responsive styling for mobile view

### Deliverables by end of Phase 1
- App skeleton runs successfully
- Shared reusable components exist
- Login/register UI is ready
- Main layout and navigation are in place
- Team has a consistent style base to build on

---

## Phase 2 — Core Product Flow: Feed, Posts, and Detail Views
Duration: Days 3–4

Goal:
Build the actual product experience: listing posts, searching, filtering, creating new posts, and viewing details.

### Tasks by person

#### Shadrack Dorkenoo
- Build the Hub/Lost & Found dashboard
- Implement the lost/found toggle
- Create the main page layout with filter controls
- Coordinate the feed page structure and card spacing
- Review integration of data models with UI
- Ensure the page is usable on mobile

#### Joel Adom Yaw Opoku
- Build the create post form
- Add image upload UI
- Implement the post detail page
- Add item metadata display
- Build the claim CTA section
- Create the basic success/error states for posting

#### Samuel Kofi Ntem Amankwah
- Build search and filtering logic
- Implement category/date/location filtering
- Create item card list behavior
- Add sorting/filter state handling
- Ensure empty states and search no-result states are clean
- Improve mobile responsiveness of the feed

### Deliverables by end of Phase 2
- Users can view items in a feed
- Users can search and filter items
- Users can create a lost/found post
- Users can open a detail page
- Feed is working and usable on mobile

---

## Phase 3 — Claims, Review, and Final Polish
Duration: Days 4–5

Goal:
Complete the claim process, make the review flow functional, and polish the app for presentation and QA.

### Tasks by person

#### Shadrack Dorkenoo
- Build the claim review dashboard
- Implement approve/deny controls
- Manage the state flow for claims
- Finalize overall app consistency
- Coordinate QA and fix critical issues
- Ensure privacy rules are respected in UI

#### Joel Adom Yaw Opoku
- Build the claim request form
- Add proof description validation
- Implement claim submission feedback
- Make claim actions visible from the item detail page
- Ensure claim status states display correctly

#### Samuel Kofi Ntem Amankwah
- Final UI polish:
  - spacing
  - fonts
  - button styling
  - card consistency
- Add loading, success, and error states
- Improve accessibility and form usability
- Test responsiveness across screen sizes
- Help with final bug fixing and demo readiness

### Deliverables by end of Phase 3
- Claim flow is working from submission to review
- Review dashboard is complete
- Frontend is polished and ready for testing
- App is mobile-ready and reasonably production-quality for MVP

---

## Recommended Working Rhythm

To keep this moving in 5 days, I suggest this pattern:

- Morning: one person owns the main task for that phase
- Midday: team reviews structure and handoff
- Afternoon: second person helps with integration and bug fixes
- End of day: quick check on what is complete vs blocked

This keeps the team moving without too much waiting.

---

## Critical Rule for the Team

No one should start a new complex screen until:
- the shared components are done
- the routing structure is clear
- the API data contract is understood

That avoids wasted work and keeps the frontend aligned with the backend.

---

## Suggested Task Ownership Summary

| Person | Phase 1 | Phase 2 | Phase 3 |
|---|---|---|---|
| Shadrack Dorkenoo | App shell, layout, route structure | Feed/dashboard, UI consistency | Claims review, QA, final polish |
| Joel Adom Yaw Opoku | Auth pages, forms | Create post, item detail, claim CTA | Claim submission flow |
| Samuel Kofi Ntem Amankwah | Shared components, filters shell | Search/filter logic, mobile UX | Final polish, accessibility, testing |

---

## Bottom line

This is the fastest clean path:
1. Setup the app shell
2. Build the lost/found core flow
3. Finish claims and polish