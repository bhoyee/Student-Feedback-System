
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

``` 

Note - Sameple Login Accounts :
                
        admin = admin/StuFeed0#,
        Head Of Staff = james/StuFeed0#,
        Academic Staff(Moderator) = bhoyee/StuFeed0#,
        Non Academic Staff = smithluv/StuFeed0#
        Student = easy/StuFeed0#
```
List of role nme (Case-sensitive)
* Moderator
* Staff
* Staff-admin
* Student

## API Reference

##### Base url https://sfbapi.azurewebsites.net/

# REGISTRATION
#### (1) User registration (both staff and student)

```http
  POST /api/account/register
```
```
  Register body
{
    "username": "string",
    "email": "string",
    "password": "string",
    "confirmpassword": "string",
    "departmentId": int
}
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
```
  login body
{
	"username": "string",
	"password": "string"
}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `username`      | `string` | **Required**.
| `password`      | `string` | **Required**. 


#### status : 200 ok

### Sample Returns: 
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
# ADMIN AREA
#### (1) Get all Staffs per department

Only login user with admin role can query this . Token need to be present

```http
  GET /api/admin/department/{departmentId}/staff
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `departmentId` | `INT` | department Id|


#### status : 200 ok

### Sample Returns: 
```
    {
        "username": "james",
        "fullName": "James Centro",
    }
```
#### (2) Assign Staff as Staff-admin(Role)

```http
  POST /api/admin/edit-roles/{username}?roles={Name}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | staff username |
| `Name` | `string` | role name to be assigned to staff |


List of role name (Case-sensitive)
* Moderator
* Staff
* Staff-admin
* Student



#### status : 200 ok

### Sample Returns: 
```
[
    "Staff-admin",
    "Staff"
]
```

#### (3) Remove Staff-admin(Role) from Staff

```http
  POST /api/admin/remove-roles/{username}?roles={Name}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | staff username |
| `Name` | `string` | role name to be removed |


List of role names (Case-sensitive)
* Moderator
* Staff
* Staff-admin
* Student



#### status : 200 ok

### Sample Returns: (this response show the remaining roles available for user)
```
[
    "Staff"
]
```

#### (4) Create Department

```http
  POST /api/departments
```
```
  Department body
{
	"departmentname": "string",
    "category": "string"
}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `departmentname` | `string` | **Required** |
| `category` | `string` | **Required** (academic or non-academic) |


#### status : 200 ok

### Sample Returns: 
```
{
    "id": 2,
    "departmentName": "library",
    "createdAt": "2023-04-27T06:00:20.0563436Z",
    "category": "non-academic",
    "users": null,
    "petitions": null,
    "feedbacks": null,
    "feedbackCount": 0
}
```

#### (5) View All Departments
#### Get all departments
```http
  GET /api/departments/all/departments
```

#### status : 200 ok

### Sample Returns: 
```
{
    "id": 1,
    "departmentName": "library",
    "category": "non-academic",
    "createdAt": "2023-04-26T06:41:53.3004964Z",
    "users": null,
    "petitions": null,
    "feedbacks": null
}
```

#### (6) Get Specific Department details
```http
  GET /api/departments/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | department id |

#### status : 200 ok

### Sample Returns: 
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
# STAFF AREA

Staff with Staff-admin role can perform these tasks

#### (1) Assign Staff as Moderator(Role)

```http
  POST /api/dept/edit-roles/{username}?roles={Name}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | staff username |
| `Name` | `string` | role name to be assigned to staff |


List of role names (Case-sensitive)
* Moderator
* Staff-admin

#### status : 200 ok

### Sample Returns: 
```
[
    "Moderator",
    "Staff"
]
```
#### (2) Remove Moderator OR Staff-admin(Role) from Staff

```http
  POST /api/admin/remove-roles/{username}?roles={Name}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | staff username |
| `Name` | `string` | role name to be removed |

List of role names (Case-sensitive)
* Moderator
* Staff
* Staff-admin
* Student



#### status : 200 ok

### Sample Returns: (this response show the remaining roles available for user)
```
[
    "Staff"
]
```
#### (3) Staff - Get total num of students in department
```http
  GET /api/users/total-students
```
Note : Get total students in department

Token generated in login must be provided with staff role to Authorized the user

#### status : 200 ok

### Sample Return: 
```
1  
```

#### (4) Staff - Get list of all staff in department
```http
  GET /api/departments/{id}/users
```
or you can use the below path for the same reponse

but only user with staff role that use the query
```http
  GET /api/departments/staffs
```
Note : Get all staffs in the department


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | departmentId |

#### status : 200 ok

### Sample Returns: 
```
[
    {
        "userName": "bhoyee",
        "fullName": "Adey Salith",
        "email": "adey.salith@hull.ac.uk"
    },
    {
        "userName": "james",
        "fullName": "James Centro",
        "email": "james.centro@hull.ac.uk"
    }
]
```
#### (4) Staff - Get all feedbacks send to department
```http
  GET /api/departments/department-feedback/
```
only users with Moderator , Staff-admin can view this


Note : If "isAnonymous": false, sender username and fullName will display else it will show "Anonymous"


#### status : 200 ok

### Sample Returns: 
```
[
    {
        "id": 1,
        "title": "Master Data-Science Scholarship",
        "content": "This to notify all student that Master Data-Science Scholarship is life now you can now apply",
        "senderId": 7,
        "senderName": "Anonymous",
        "senderFullName": "Anonymous",
        "status": 0,
        "isAnonymous": true,
        "openFeedbackCount": 0,
        "departmentId": 0,
        "departmentName": null,
        "assignedToId": null,
        "assignedToName": null,
        "dateCreated": "2023-04-26T13:39:35.5875212",
        "targetAudience": null,
        "feedbackReplies": []
    },
    {
        "id": 2,
        "title": "Deadline extension",
        "content": "I will like to extend the submission of my course work due to some reason that arises",
        "senderId": 7,
        "senderName": "easy",
        "senderFullName": "Bola Hammed",
        "status": 0,
        "isAnonymous": false,
        "openFeedbackCount": 0,
        "departmentId": 0,
        "departmentName": null,
        "assignedToId": null,
        "assignedToName": null,
        "dateCreated": "2023-04-28T03:20:15.6384101",
        "targetAudience": null,
        "feedbackReplies": []
    }
]
```
#### (5) Staff with Moderator or Staff-admin role can Create feedback and send to all students in the department

##### While staff with Moderator or Staff-admin in Non academic can send feedback to all student that register on the portal

```http
  POST /api/departments/feedback/create
```
only users with Moderator , Staff-admin can crate this

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `title`      | `string` | feedback title|
| `content`      | `string` | feedback content|
| `isAnonymous`      | `bool` | true / false|
| `targetAudience`      | `string` | Departments / AllStudents|


#### status : 200 ok

### Sample Returns: 
```
{
    "message": "Feedback created successfully"
}
```
#### status : 401 | Unauthorized

```
You can only send feedback to students in your department
```


# STUDENT AREA
#### (1) Student - view user Profile
```http
  GET /api/users/{username}
```
Note : Get user details but must be login user

Token generated in login must be provided to Authorized the user to be able to get user profile


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `username`      | `string` | login User username |

#### status : 200 ok

### Sample Returns: 
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

#### (2) Student - Get all departments user have access to
```http
  GET /api/users/departments
```
Note : Get all departments user get access  to but must be login user

Token generated in login must be provided to Authorized the user

#### status : 200 ok

### Sample Return: 
```
{
    "allDepartments": [
        "computer sc",
        "library",
        "sport-centre",
        "student-centre"
    ]
}
```

#### (3) Student - create feedback
```http
  POST /api/feedbacks/create
```
```
  create feedbacks body
{
    "title": "string",
    "content": "string",
    "isAnonymous": bool,
    "departmentId": int
}
```
Note : only a login user can create feedback

Token generated in login must be provided to Authorized the user


| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `title`      | `string` | feedback title |
| `content`      | `string` | feedback content |
| `isAnonymous`      | `bool` | false / true |
| `departmentId`      | `int` | department id of where the feedback will be going|


#### status : 200 ok

### Returns: 
Inserted successfully

#### (4) Student - Get all feedback created by user
```http
  GET /api/feedbacks/user/{userId}
```
Note : Get all student feedback but must be login user

Token generated in login must be provided to Authorized the user


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

#### (5) Student - Get all feedback send from departments
```http
  GET /api/users/feedback
```
Note : Get all student feedback but must be login user

Token generated in login must be provided to Authorized the user


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
### Sample Returns: 
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

