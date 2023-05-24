# Notes, Assumptions, and Questions

## Notes
---
I have chosen to add a service layer to decouple the controller form the repo.
I would put this in a seperate project to manage dependencies, but for readability it will remain in `EPM.Mouser.Interview.Web`.
---
Tests to get a feel.
Found there is no validation, bar `ReservedQuantity` being set to 0.
If this were real it would start off the where should we validate convo. In the controller, the service, as close to the db as possible and then float some error up, in a seperate service called whenever etc.
Here I will validate in the controller. Users need to know what happens if their input is rejected.
---
