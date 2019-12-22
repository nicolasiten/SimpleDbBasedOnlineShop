# Unit Testing
Implement Unit Test for a Simple Database based Online Shop application.
## Test Strategy
- Started from the lowest layer and went up
1 Repositories
2 Services
3 Controllers
## Type of testing
- Good input
- Bad input
- Nulls
## Database Tests
- Write tests were done with an InMemory Database (unique Db for each Test)
- Read tests were done with a Real Database (same Db for each Test --> Test multiuser behavior)
- Implemented base class (DbContextTestBase) to have initialization logic at one place
## Mocks
Mocks were only used if necessary for example to test logic were a StringLocalizer is involved or the Identity classes.