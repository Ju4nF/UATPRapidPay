# RapidPay project

### Testing the API
The API can be tested using SWAGGER. 
1. Configure the connection string to SQL Server in the `appsetting.json` file.
2. Run the API, the database should be created by EF Core
3. Register a new user with the `admin` role. For example:
   ```json
	{
    "name": "John",
    "password": "JohnPassword",
    "userName": "jwick",
    "role": "admin"
   }
   ```
   (If the user is not `admin` can only use GetCardBalance endpoint)
5. Login the new user (username and password) to get a token
6. Use the token to authorize and be able to use all endpoints.
