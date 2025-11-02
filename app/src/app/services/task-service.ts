import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { baseUrl } from '../../constants';
import { PagedResponse, TaskForm } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private http = inject(HttpClient)

  create(task:TaskForm):Observable<any> {
    return this.http.post(`${baseUrl}/tasks`, task);
  }

  edit(id:string = "",task:TaskForm) {
    return this.http.put(`${baseUrl}/tasks/${id}`, task);
  }

  findAll(pageNumber = "1", statusFilter: string | undefined | null):Observable<PagedResponse> {
    return this.http.get<PagedResponse>(`${baseUrl}/tasks/paged?PageNumber=${pageNumber}&PageSize=10${statusFilter ? `&StatusFilter=${statusFilter}` : ''}`);
  }

  delete(id:string):Observable<boolean> {
    return this.http.delete<boolean>(`${baseUrl}/tasks/${id}`);
  }

}
