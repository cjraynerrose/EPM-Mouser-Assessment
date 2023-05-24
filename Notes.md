# Notes, Assumptions, and Questions

## Notes
---
I have chosen to add a service layer to decouple the controller form the repo.
I would put this in a seperate project to manage dependencies, but for readability it will remain in `EPM.Mouser.Interview.Web`.
This is mostly just habit as it adds very little complexity at this point, simply wrapping the repo with a try/catch. But allowing future expansion and giving a definite place to keep any logic that isnt web related for better reuse.

---
Tests to get a feel.
Found there is no validation, bar `ReservedQuantity` being set to 0.
If this were real it would start off the where should we validate convo. In the controller, the service, as close to the db as possible and then float some error up, in a seperate service called whenever etc.
Here I will validate in the controller. Users need to know what happens if their input is rejected.
---
For the `OrderItem` endpoint, my knowledge of HTTP tells me this should be a POST or PUT, not GET.
I am leaving it as GET for now as per the assumption that I cannot change the endpoints.
---
I am not considering any concurrency here.
For example in the `OrderItem` code, the product is fetched from the db, evaluated, updated, then re-committed with no concurrency checks.

## Assumptions
- The WarehouseApi endpoints cannot be modified to a different response type.