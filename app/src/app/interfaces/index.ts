export interface UserForm {
  firstName:string
  lastName:string
  email:string
  password:string
  confirmPassword:string
  confirmEmail:string
  userName:string
}

export interface LoginData {
  email:string
  password:string
}

export interface AccessToken {
  accessToken:string
}

export interface TaskForm {
  title: string
  description:string
  status:string
}

export interface PagedResponse {
  items: Task[]
  page:number
  pageSize:number
  totalCount:number
  totalPages:number
}

export interface Task {
  id:string
  title:string
  description:string
  status:number
  userName:string
  userId:string
  createdAt:string
  updatedAt:string
}

export interface User {
  id:string
  email:string
  firstName:string
  lastName:string
  userName:string
  role:string
}
