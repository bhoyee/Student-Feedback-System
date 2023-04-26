
# Student Feedback

API endpoints 

### Status code 
``` 
###### (404) | NOT FOUND
###### (200) | OK
###### (401) |Unauthorized

``` 
``` 

Note - feedback Status :
 
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
```



## API Reference

##### Base url https://sfbapi.azurewebsites.net/

# REGISTRATION
#### (1) User registration (both staff and student)

```http
  POST /api/account/register
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | **Required* |
| `email` | `string` | ** must be sch email |
| `password` | `string` | **Required* |
| `confirmpassword` | `string` | **Required* |
| `departmentId` | `int` | **Required* |

#### status : 200 ok
###### Response message:
Registration Successful . Check email to verify your account

# LOGIN
#### (2) User login (both staff and student)

```http
  POST /api/account/login
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `username`      | `string` | **Required**.
| `password`      | `string` | **Required**. 


#### status : 200 ok

### Returns: 
```
{
    "username": "easy",
    "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW",
    "photoUrl": null,
    "role": [
        "Student"
    ]
}
```
# DEPARTMENT
#### (3) Create Department

```http
  POST /api/departments
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `departmentname` | `string` | **Required** |


#### status : 200 ok

### Returns: 
```
{
    "id": 1,
    "departmentName": "computer sc",
    "createdAt": "2023-04-26T06:41:53.3004964Z",
    "users": null,
    "petitions": null,
    "feedbacks": null
}
```

#### (4) View All Departments
#### Get all departments
```http
  GET /api/departments/all/departments
```

#### status : 200 ok

### Returns: 
```
{
    "id": 1,
    "departmentName": "computer sc",
    "createdAt": "2023-04-26T06:41:53.3004964Z",
    "users": null,
    "petitions": null,
    "feedbacks": null
}
```

#### (5) Get Specific Department details
```http
  GET /api/departments/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | department id |

#### status : 200 ok

### Returns: 
```
{
    "id": 1,
    "departmentName": "computer sc",
    "totalFeedback": 1,
    "totalUsers": 1,
    "totalStudents": 1,
    "totalStaffs": 0,
    "totalOpenFeedback": 1
}
```
# STUDENT
#### (6) Student - view user Profile
```http
  GET /api/users/{username}
```
Note : Get user details but must be login user

Token generated in login must be provided to Authorized the user to be able to get user profile


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `username`      | `string` | login User username |

#### status : 200 ok

### Returns: 
```
{
    "id": 4,
    "userName": "easy",
    "photoUrl": null,
    "email": "a.m.salisu-2021@hull.ac.uk",
    "fullName": "Bola Hammed",
    "created": "2023-04-26T09:17:19.3915387",
    "lastActive": "2023-04-26T10:30:13.1999839",
    "interest": null,
    "photo": null,
    "departmentId": 1,
    "deptName": null,
    "department": {
        "departmentName": "computer sc"
    },
    "role": [
        "Student"
    ]
}
```

#### (7) Student - create feedback
```http
  POST /api/feedbacks/create
```
Note : only a login user can create feedback

Token generated in login must be provided to Authorized the user to be able to create the feedback


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `title`      | `string` | feedback title |
| `content`      | `string` | feedback content |
| `isAnonymous`      | `bool` | false / true |
| `departmentId`      | `int` | department id of where the feedback will be going|


#### status : 200 ok

### Returns: 
Inserted successfully

#### (8) Student - Get student feedbacks
```http
  GET /api/feedbacks/user/{userId}
```
Note : Get all student feedback but must be login user

Token generated in login must be provided to Authorized the user to be able to get user profile


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `username`      | `string` | login User username |

#### status : 200 ok
Note - feedback Status :
```  
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
```
### Returns: 
```
[
    {
        "id": 1,
        "title": "Master Data-Science Scholarship",
        "content": "This to notify all student that Master Data-Science Scholarship is life now you can now apply",
        "senderId": 0,
        "senderName": "easy",
        "status": 0,
        "isAnonymous": false,
        "openFeedbackCount": 0,
        "departmentId": 0,
        "departmentName": "data sc",
        "assignedToId": null,
        "assignedToName": null,
        "dateCreated": "2023-04-26T13:39:35.5875212",
        "targetAudience": null,
        "feedbackReplies": []
    }
]
```


#### Get item

```http
  GET /api/items/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

#### add(num1, num2)

Takes two numbers and returns the sum.

