# TodoApp
This  App is created by Mert Alici in response to a given recruitment task.
You may find a brief of the task at the very end.

TodoApp is a simple REST API developed in .Net Core 3.0 environment. It has POST and GET endpoints. 
POST input and GET output have the common structure as given below. 
>{
"Id": 1, // integer
"Name": "Test", // string
"Text": "Test content", // string
"Created": "2019-11-24T18:55:52.7174882+00:00" // DateTimeOffset Object
}

# Working Principles of TodoApp 
- TodoApp offers an SQL Server Database storage option and an InMemory storage option.
- Since Id is a part of the input, IDENTITY_INSERT  mode of the database was turned ON to assign Id (Primary_Key) by the input.
- A plain GET request returns all the Json objects in one array: 
 -       GET https://localhost:44301/api/TodoItems/
- If a positive integer is given as route parameter like below:
-       GET https://localhost:44301/api/TodoItems/1
In this case, TodoApp will return max. 5 object after the given integer route parameter. The example provided above would return the next 5 elements, regardless of their Id values because the input values are NOT sorted by Id.
Let's say the database stores only 4 inputs. Considering the get request above,  the output would be an array of 2nd, 3rd, and 4th Json objects. Please note the usage of ConcurrentBag can change the order of the stored objects. For more information please see ImplementedMethods section.  


# TodoAPP is Great, so How Can You Test It?
Needed Tools : SQL Server, .Net Core 3.0 + , an API Client (eg.Postman)

After you download the repository, you need to make a few simple configurations before running TodoApp. 
## 1) Database Configuration:
        If you would like to use InMemory storage, skip this step, and see How to Switch to InMemory!.
In order to connect TodoApp to your database, you need to change the existing ConnectionString according to your database.
This configuration can be done inside the appsettings.json which is located in the root folder.

If you are using an SQL Server, you can proceed to the next step. If you would like to use a different database server provider (eg. PostgreSQL etc.), you need to make one last configuration. This last step would be changing the ConfigureServices method in the StartUp.cs file which is located in the root folder.

Plesae find the line below and configure it according to your database server.
```sh
services.AddDbContext<TodoContext>(opt => opt.UseSqlServer
(Configuration.GetConnectionString("DbConnection")));.
```

## 2) Build and Run TodoApp:
You can build and run TodoApp using .Net CLI, Visual Studio, Visual Studio Code, etc. 
Please note that Visual Studio may attain a random port while .Net CLI and Visual Studio Code would usually use localhost:5001.

The demonstration is provided using Visual Studio 2019. The port in the red box is what we need. 
If any security questions arise when you run the app, trust and proceed.

![url](https://user-images.githubusercontent.com/42611205/85769651-7a78ef00-b71a-11ea-9e11-c0cea6db4175.png)

Please ignore the weatherforecast part. 

## 3) Sending Requests Through Postman:
Once TodoApp is up and running, you are good to open Postman. 
#### 3.a) POST:
Before you start POST'ing anything, enter the URL that leads to the API controllers. 
Enter https://localhost:"port"/api/TodoItems/  as seen in the image below.

![Post](https://user-images.githubusercontent.com/42611205/85770757-8b763000-b71b-11ea-958b-74c2fbea7016.png)

You can start using TodoApp by posting some Json objects one by one. 
-   You can assign the Id as you whish. It does not need to follow an order.
-   You can NOT POST two entities with the same Id since it is a Primary_Key.

#### 3.b) GET:
GET request returns objects from a database or the memory. Simply toggle to  GET instead of POST. 
  #### 3.b.1) GET All
  In order to GET all of the objects from the database or memory, keep the URL as below and hit SEND button.
  TodoAPP should return all the Json objects in one array. 
  
      https://localhost:<port>/api/TodoItems/
A demonstration is given below: 

![GetAll](https://user-images.githubusercontent.com/42611205/85773047-bc576480-b71d-11ea-8788-87a00b7909fa.png)

#### 3.b.2) GET by Reference:
TodoApp has a GET Endpoint which takes an integer route parameter. Let's call this parameter **start** . 
Given that a number of objects have already been stored in the database, TodoApp will start returning max. 5 objects that come after **start**. For example, if **start** = 3, TodoApp will return, if available, 4th, 5th, 6th, 7th, and 8th Json objects in one array. 
 
 In order to use this functionality, just enter a number after the last "/" as seen below.
 
 ![GetSome](https://user-images.githubusercontent.com/42611205/85776347-ea8a7380-b720-11ea-9467-3c2d153a1c59.png)
 
 **start**=2 in the example above. Therefore, TodoApp returned the next 5 objects in one array. 
 
 If there are only 15 elements stored in the database or memory and **start** is set to 13, the result will look like the image below. Please note that TodoApp did not raise an error, it just returned all the last available objects.
 
 ![GetLastBit](https://user-images.githubusercontent.com/42611205/85777012-84522080-b721-11ea-8352-301c9960c2d1.png)


 # How To Switch To InMemory Storage:
 Although there is actually not any switch function implementation, it is very simple to switch between database storage and InMemory storage. This configuration can be done in the StartUp.cs which is located in the root folder. 
 
Please find the lines which look like the code snippet below in StartUp.cs file.
As seen clearly, the first line adds InMemory storage service and it is currently commented out. The second part of the snipped adds SqlServer service.  You can switch to  InMemory storage by uncommenting the first line and commenting the second part of the code out. With this simple modification, you can build and run TodoApp again to use InMemory storage. 


 ```sh
 //services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList")); 
 
services.AddDbContext<TodoContext>(opt => opt.UseSqlServer
(Configuration.GetConnectionString("DbConnection")));.
```
 Please note that if you are using a database server other than SQL Server, your second part of the code snippet should look different. See 1) Database Configurations.  
 If you use InMemory storage, your data will be lost when TodoApp stops. 
 
 # Implemented Methods
         Methods below can be found in Services.ImplementedMethods.cs.
         Note that it is not possible to use both methods during compile time. Therefore, one of them must be commented out.
 ## 1) GetNonOrderedObjectsByReference:
 This method uses ConcurrentBag as suggested in the task brief. ConcurrentBag is normally used when the order is not important. GetNonOrderedObjectsByReference method takes **start** parameter, which is explained above, and returns  max. 5 Json objects.
 
  ## 2) GetOrderedObjectsByReference:
This method is very similar to the previous method but it uses ArrayList instead of ConcurrentBag. Input order of the Json objects is maintained. Therefore,  when TodoApp receives a Get by Reference request,  returning objects are selected according to their order in the database or in the memory.

# Switching Between Implemented Methods
         The methods are called in Controller.TodoItemsController.cs.
 Since it is not possible to have both methods during the compile-time, one of the methods must be commented out. That's how you can switch between to methods.
 
 Find the lines that looks like the code snippet below in Controller.TodoItemsController.cs. You can uncomment one method and comment out the other one. 
  ```sh
return Ok(implementedMethods.GetNonOrderedObjectsByReference(start, _context));  
//return Ok(implementedMethods.GetOrderedObjectsByReference(start, _context));  
```
 
# Task Brief:

Implement a small REST API in .net core 3.0 that does two things:
- Offers an HTTP Post endpoint that retrieves a Json Object in the Post Body. The object needs to be stored in an in-memory store (e.g. ConcurrentDictionary or ConcurrentBag)
- Offers an HTTP GET endpoint that has an optional route parameter (integer: start) that returns an array of those Json objects from the store with a maximum of 5 entries. When the optional parameter (start) is a positive number, it shall skip that amount of entries from the store and return the next 5 entries.
Optional: Use a Database to store those objects delivered by the API. (e.g. Cosmos db: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator )
Starting point on how to create a web api using .net core 3.0:
- https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
Payload object definition:
The input json object shall have the same structure as the output document and is defined as this:
{
"Id": 1, // integer
"Name": "Test", // string
"Text": "Test content", // string
"Created": "2019-11-24T18:55:52.7174882+00:00" // DateTimeOffset Object
}
Please keep out of scope: Authentication, Authorization, HTTPS, CORS, ...

