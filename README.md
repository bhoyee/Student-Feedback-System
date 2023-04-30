
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

Note - Sample Login Accounts :
                
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

# Forgot-password
#### (3) User need to provide registered email

```http
  POST /api/account/forgot-password
```
```
   body
{
  "email": "email@hull.ac.uk"
}
```
#### status : 200 ok

### Sample Returns: 
Note : token will be included in the link what will be generated 
```
Reset password detail send to your mail

```

# Reset-password
#### (4) User click the link send to mail to resent the password , which in

```http
  POST /api/account/reset-password
```
```
You need to verify the token coming via the link if they re valid 

  body sample
{
    "email": "a.g.plex-2021@hull.ac.uk",
    "token": "CfDJ8JQtqeclVCw%3D%3D",
    "password": "mynewpassword123",
    "confirmpassword": "mynewpassword123",

}

```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `token`      | `string` | token will be inside the url link the user will click from mail
| `password`      | `string` | **Required**. 
| `confirmpassword`      | `string` | **Required**.


#### status : 200 ok

### Sample Returns: 
```
Password changed successfully
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

#### (6) Admin Delete User
```http
  POST /api/users/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | user id that need to be deleted |

#### status : 200 ok

```
User deleted successfully
```

#### (7) Admin Delete User
```http
  POST /api/users/{id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | user id that need to be deleted |

#### status : 200 ok

```
User deleted successfully
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

#### (6) Staff reply to feedback
```http
  POST /api/feedbacks/{feedbackId}/reply"
```
```
  reply body
{
    "content": "This  should be send from department or school not student sending this . I will closed this feedback . Thank you!",
    "Status": 3
}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `feedbackId`      | `int` | feedback Id|
| `content`      | `string` | content of the reply|
| `Status`      | `int` |  

        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3|


#### status : 200 ok

### Sample Returns: 
```
Reply added successfully
```


#### (7) Get all feedback in department
```http
  GET /api/departments/department-feedback/
```

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
}


#### (8) Get a specific feedback in department with replies

```http
  GET /api/departments/department-feedback/{feedbackId}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `feedbackId`      | `int` | feedback Id|

#### status : 200 ok

### Sample Returns: 
```
{
    "id": 1,
    "title": "Master Data-Science Scholarship",
    "content": "This to notify all student that Master Data-Science Scholarship is life now you can now apply",
    "senderId": 0,
    "senderName": "Anonymous",
    "senderFullName": "Anonymous",
    "status": 3,
    "isAnonymous": false,
    "openFeedbackCount": 0,
    "departmentId": 1,
    "departmentName": null,
    "assignedToId": null,
    "assignedToName": null,
    "dateCreated": "2023-04-26T13:39:35.5875212",
    "targetAudience": null,
    "feedbackReplies": [
        {
            "id": 2,
            "feedbackId": 0,
            "content": "This  should be send from department or school not student sending this . I will closed this feedback . Thank you!",
            "userId": 9,
            "userFullName": "Adey Salith",
            "user": null,
            "modifiedAt": null,
            "dateCreated": "0001-01-01T00:00:00",
            "isPublic": true,
            "updatedAt": "0001-01-01T00:00:00",
            "status": 3
        }
    ]
}

#### (9) Staff assign feedback to another staff
```http
  POST /api/feedbacks/{feedbackId}/assign
```
```
body 

{
 "recipientId": 12
}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `feedbackId`      | `int` | feedbackId that need to be assigned |

| `recipientId`      | `int` | this is that staff Id that the feedback with be assgin to |

#### status : 200 ok

### Sample Returns: 
```
Feedback with ID {feedbackId} has been assigned to user with ID {recipientId}.
```


#### (10) staff view /get feedback assigined to them
```http
  GET /api/feedbacks/assigned
```
#### status : 200 ok

### Sample Returns: 
```
[
    {
        "id": 1,
        "title": "Master Data-Science Scholarship",
        "content": "This to notify all student that Master Data-Science Scholarship is life now you can now apply",
        "senderId": 0,
        "senderName": "Anonymous",
        "senderFullName": null,
        "status": 2,
        "isAnonymous": true,
        "openFeedbackCount": 0,
        "departmentId": 0,
        "departmentName": "data sc",
        "assignedToId": null,
        "assignedToName": null,
        "dateCreated": "2023-04-26T13:39:35.5875212",
        "targetAudience": null,
        "feedbackReplies": [
            {
                "id": 2,
                "feedbackId": 0,
                "content": "This  should be send from department or school not student sending this . I will closed this feedback . Thank you!",
                "userId": 9,
                "userFullName": "Peter Jide",
                "user": null,
                "modifiedAt": null,
                "dateCreated": "0001-01-01T00:00:00",
                "isPublic": true,
                "updatedAt": "0001-01-01T00:00:00",
                "status": null
            },
            {
                "id": 3,
                "feedbackId": 0,
                "content": "This is not for you . Thank you!",
                "userId": 9,
                "userFullName": "Peter Jide",
                "user": null,
                "modifiedAt": null,
                "dateCreated": "0001-01-01T00:00:00",
                "isPublic": true,
                "updatedAt": "0001-01-01T00:00:00",
                "status": null
            }
        ]
    }
]

```

#### (11) Getting department feedback count 
```http
  GET /api/departments/dept/{departmentId}/feedback-counts
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `departmentId`      | `int` | department ID|


#### status : 200 ok

### Sample Returns: 

```
{
    "totalFeedbacks": 34,
    "totalOpenFeedbacks": 32,
    "totalClosedFeedbacks": 1,
    "totalInProgress": 1
}
```
#### (12) Get all list of closed feedback in dept
```http
  GET /api/departments/{id}/closed-feedbacks
```
#### (13) Get all list of open feedback in dept
```http
  GET /api/departments/{id}/open-feedbacks
```
#### (13) Get all list of inprogress feedback in dept
```http
  GET /api/departments/{id}/inprogress-feedbacks
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | department ID


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
#### (6) Student - Get all feedback logined with Student role user created

```http
  GET /api/feedbacks/user/{userId}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `userId`      | `int` | user login Id |

#### status : 200 ok


#### (7) Student - student viewing specific feedback with replies
```http
  GET /api/feedbacks/user/feedback/{feedbackId}
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `feedbackId`      | `int` | feedbackId  |

#### status : 200 ok


#### (8) Student - student reply to a feedback
```http
  POST /api/feedbacks/user/{feedbackId}/reply
```
```
  reply body
{
    "content": "Just testing this reply out"
}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `feedbackId`      | `int` | feedbackId  |
| `content`      | `string` | content of the response  |

#### status : 200 ok


#### (9) Student - get count of feedbacks
```http
  GET /api/feedbacks/feedback-counts
```

#### status : 200 ok
```
Sample response
{
    "totalFeedbacks": 2,
    "openFeedbacks": 1,
    "closedFeedbacks": 0,
    "inProgressFeedback": 0,
    "resolvedFeedback": 1
}
```

# PETITION API

#### (1) Create Petition (must be login student with verify token in the header)
```http
  POST /api/petition/
```
```
  sample of raw body
{
  "title": "petition",
  "message": "Message of the petition",
  "anonymous": 1,
  "departmentId": 1
}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `petitionType`      | `string` | General / Department |
| `title`      | `string` | petition title  |
| `message`      | `string` | petittion content |
| `anonymous`      | `int` | 1 = true , 0 = false |
| `departmentId`      | `int` | department where the petittion is going |

#### status : 200 ok


#### (2) Vote for Petition
```http
  POST /api/votes/{petitionId}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `petitionId`      | `string` | Petition Id |

#### status : 200 ok


#### (2) View Petitions with total votes
```http
  POST /api/petition/petitionsWithVotes
```


#### status : 200 ok

Sample response
```
{
    "id": 36,
    "title": "Title of the petition",
    "description": "Message of the petition",
    "votes": 3
}
```


#### (3) View Specific Petition with total votes
```http
  POST /api/petition/petitionsWithVotes/{petitionId}

```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `petitionId`      | `string` | Petition Id |

#### status : 200 ok

Sample response
```
{
    "id": 36,
    "title": "Title of the petition",
    "description": "Message of the petition",
    "votes": 3
}
```