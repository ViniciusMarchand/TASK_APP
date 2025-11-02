import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { TaskModal } from '../common/task-modal/task-modal';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TaskService } from '../../services/task-service';
import { Task } from '../../interfaces';
import { Card } from '../common/card/card';
import { CommonModule } from '@angular/common';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatRadioModule } from '@angular/material/radio';
import { AuthService } from '../../services/auth/auth-service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

export interface DialogData {
  updateTasksList: Function;
  closeModal: Function;
  dialogType: 'add' | 'edit';
  task?: Task;
}

@Component({
  selector: 'app-home-page',
  imports: [MatButtonModule, MatDialogModule, Card, CommonModule, MatPaginatorModule, MatRadioModule, MatProgressSpinnerModule],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
})
export class HomePage implements OnInit, OnDestroy {
  readonly dialog = inject(MatDialog);
  private taskService = inject(TaskService);
  private cdRef = inject(ChangeDetectorRef);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  isLoading = false;
  totalPages = 0;
  totalCount = 0;
  page = 1;

  isAdmin: boolean = false


  tasks: Task[] = [];
  private queryParamsSubscription?: Subscription;

  currentPage: string = '1';
  currentFilter: string | null = null;

  ngOnInit(): void {
    this.loadInitialData();

    this.setupQueryParamsListener();

    this.authService.getUserRole()

    this.isAdmin = this.authService.isAdmin();
  }

  private loadInitialData(): void {
    const params = this.activatedRoute.snapshot.queryParamMap;
    this.currentPage = params.get('page') || '1';
    this.currentFilter = params.get('filter');

    this.updateTasksList();
  }

  private setupQueryParamsListener(): void {
    this.queryParamsSubscription = this.activatedRoute.queryParamMap.subscribe((params) => {
      this.handleQueryParamsChange(params);
    });
  }

  private handleQueryParamsChange(params: any): void {
    const newPage = params.get('page') || '1';
    const newFilter = params.get('filter');

    const pageChanged = newPage !== this.currentPage;
    const filterChanged = newFilter !== this.currentFilter;

    if (pageChanged || filterChanged) {
      this.currentPage = newPage;
      this.currentFilter = newFilter;

      this.updateTasksList();
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(TaskModal, {
      data: {
        updateTasksList: () => {
          this.updateTasksList();
        },
        closeModal: () => {
          dialogRef.close();
        },
        dialogType: 'add',
      },
    });

    dialogRef.afterClosed().subscribe((_) => {
    });
  }

  openDialogEdit(task: Task): void {
    const dialogRef = this.dialog.open(TaskModal, {
      data: {
        updateTasksList: () => {
          this.updateTasksList();
        },
        closeModal: () => {
          dialogRef.close();
        },
        dialogType: 'edit',
        task: task,
      },
    });

    dialogRef.afterClosed().subscribe((_) => {
    });
  }

  updateTasksList() {
    this.isLoading = true;
    this.taskService.findAll(this.currentPage, this.currentFilter).subscribe({
      next: (data) => {
        const { items, totalPages, totalCount, page } = data;

        this.tasks = items;
        this.totalPages = totalPages;
        this.totalCount = totalCount;
        this.page = page - 1;

        if (page > totalPages || page < 1) {
          this.router.navigate([], {
            queryParams: {
              page: 1,
            },
          });
        }

        this.cdRef.markForCheck();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro ao carregar tasks:', error);
        this.isLoading = false;
      },
    });
  }

  deleteTask(id: string) {
    this.taskService.delete(id).subscribe({
      next: () => {
        this.updateTasksList();
      },
      error: (error) => {
        console.error('Erro ao deletar task:', error);
      },
    });
  }

  handlePageEvent(event: PageEvent) {
    this.router.navigate([], {
      queryParams: {
        page: event.pageIndex + 1,
      },
      queryParamsHandling: 'merge',
    });
  }

updateFilter(filter: string | null) {
    this.router.navigate([], {
      queryParams: {
        filter: filter,
        page: 1,
      },
      queryParamsHandling: 'merge',
    });
  }

  clearFilter() {
    this.router.navigate([], {
      queryParams: {
        filter: null,
        page: 1,
      },
      queryParamsHandling: 'merge',
    });
  }

  ngOnDestroy(): void {
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }
  }

  logout() {
    this.authService.logout();
  }

}

