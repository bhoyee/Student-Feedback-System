
# Student Feedback

API endpoints 

### Status code 
###### (404) | NOT FOUND
###### (200) | OK


## API Reference

##### Base url https://sfbapi.azurewebsites.net/


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

#### (6) Student - create feedback
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



