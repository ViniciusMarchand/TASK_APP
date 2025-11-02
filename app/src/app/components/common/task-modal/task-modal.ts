import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TaskForm } from '../../../interfaces';
import { TaskService } from '../../../services/task-service';
import { DialogData } from '../../home-page/home-page';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-task-modal',
  imports: [MatDialogModule, MatInputModule, MatButtonModule, ReactiveFormsModule, MatSelectModule, MatProgressSpinnerModule],
  templateUrl: './task-modal.html',
  styleUrl: './task-modal.css',
})
export class TaskModal {
  private taskService = inject(TaskService);

  @Output() updateTasks = new EventEmitter<void>();

  readonly dialogData = inject<DialogData>(MAT_DIALOG_DATA);
  readonly task = this.dialogData.task;

  hasError: Error | null = null;
  isLoading = false;

  taskForm = new FormGroup({
    title: new FormControl(this.task?.title || ''),
    description: new FormControl(this.task?.description ||''),
    status: new FormControl(this.task?.status.toString() ?? ''),
  });

  onSubmit() {
    this.isLoading = true;
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value as TaskForm;
      if(this.dialogData.dialogType == "add") {
        this.taskService.create(formValue).subscribe({
          next: (_) => {
            this.dialogData.updateTasksList();
            this.dialogData.closeModal();
            this.isLoading = false;
          },
          error: (error) => {
            this.hasError = error;
            this.isLoading = false;
          },
        });
      } else {
        this.taskService.edit(this.task?.id, formValue).subscribe({
          next: (data) => {
            this.dialogData.updateTasksList();
            this.dialogData.closeModal();
          },
          error: (error) => {
            this.hasError = error;
          },
        });
      }


    }
  }
}
