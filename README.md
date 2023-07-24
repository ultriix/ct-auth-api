# Auth API Code-Test

## Quick Start

TBC

<br>
<br>

## Objective

#### dotnet C# practical test 
 
This is an open-ended code test; you can make your answer as simple or as complicated as you want. The goal is to get an idea of how you approach a problem, how you solve it, and what the shape of your code is. Although you are free to spend as long as you like, we don’t expect you to invest hours and hours on it. 
 
For this test, please create a minimal dotnet (C#) REST API that has the following endpoints:  
 
1) Create user: 
    - This endpoint should require name, email and password and create a new user and store in-memory.  
2) Get JWT token: 
    - This endpoint should require an email and password and generate a Jason Web Token (JWT) for the matching user if found in in-memory store. 
3) List users: 
    - This endpoint should list all users in the in-memory store. This endpoint should require bearer authentication using a valid JWT (obtained from above endpoint).  
 
That’s the whole thing. 
 
Please commit your code to a git repository of your choice and send over a link. Please include any notes on any decisions you may have made and any ideas for what you would do in the future given more time.  
 
Things you may want to consider: 
 
- Scalability 
- Testability 
- Maintainability 
- Readability 
- Reusability 
- Performance 


## Potential Solution Steps

1) Generate from .Net CLI Webapi template to get basic solution, add testing project. Strip out any unwanted template code. Add any packages we expect to utilize with basic startup configuration where possible:

- Swagger
- NUnit
- Moq
- JWT Middleware
- In-Memory store
- NetCore Cryptography (for Hashing passwords)

2) Add minimum required stubs / implementation to make structure clearer and allow prepping some basic unit tests:

- Model User
    - string Name
    - string Email
    - string Password
        - We will want to store a hash of the password only and return only hashed passwords when fetching a list of users
- User Controller:
    - ListUsers
    - CreateUser
    - GetJwtToken
- User Service
    - In Memory DB operations related to user
    - Hash / Verify Password logic
- Auth Service 
    - JWT Token logic

3) Implement service layer unit testing. In the interest of time we are going to at least unit test the service layer to assist with development of the remaining tasks.
    >In production we would be more thorough and look to test multiple layers of the solution.

4) Setup JWT middleware and any required configuration. Implement auth service to satisfy token unit test(s).

5) Implement user service logic to create and list users. Seed in memory db with some test users on startup. Implement any password hashing and verify logic as needed. Ensure service level unit tests are passed.

6) Implement controller logic to satisfy brief, configure swagger as needed to allow manual testing.


## Implementation Notes

1) CLI template basic swagger setup so I didn't need to add it, this is easier for manual testing as opposed to Curl or Postman. If I were aiming for production I would also setup for full Https with an SSL certificate. Im using http only for the purposes of the code test.

2) Brief skeleton added for solution and unit tests.

3) Added basic unit tests to ensure service logic keeps to intended scope.
    - Added required key on user model for in memory db. Given that I would normally return a dto from the controller rather than the internal model, I would encode this id if it was required outside of the system.

    - Given more time I would also test the hashing is done correctly on the user password and verify more details of the jwt token than just the email.